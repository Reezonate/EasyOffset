using System;
using EasyOffset.AssetBundleScripts;
using EasyOffset.Configuration;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace EasyOffset {
    [UsedImplicitly]
    public class SwingBenchmarkManager : IInitializable, IDisposable, ITickable {
        #region Constants

        private const int MaximalCapacity = 120;
        private const float FullSwingAngleRequirement = 140 * Mathf.Deg2Rad;

        #endregion

        #region Constructor

        private readonly GizmosManager _gizmosManager;
        private readonly MainSettingsModelSO _mainSettingsModel;

        private readonly SwingAnalyzer _swingAnalyzer;
        private readonly SwingBenchmarkController _swingBenchmarkController;

        public SwingBenchmarkManager(
            GizmosManager gizmosManager,
            MainSettingsModelSO mainSettingsModel
        ) {
            _gizmosManager = gizmosManager;
            _mainSettingsModel = mainSettingsModel;
            _swingAnalyzer = new SwingAnalyzer(MaximalCapacity);

            var gameObject = Object.Instantiate(BundleLoader.SwingBenchmarkController);
            Object.DontDestroyOnLoad(gameObject);
            _swingBenchmarkController = gameObject.GetComponent<SwingBenchmarkController>();
        }

        #endregion

        #region Logic

        private bool _isSwingGood;
        private Hand _selectedHand = Hand.Left;

        public void Reset(Hand hand) {
            _swingAnalyzer.Reset();
            _selectedHand = hand;
            _swingBenchmarkController.StartTracking();

            switch (hand) {
                case Hand.Left:
                    _leftHandVisible = false;
                    break;
                case Hand.Right:
                    _rightHandVisible = false;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(hand), hand, null);
            }

            UpdateVisibility();
        }

        public void Update(
            ReeTransform controllerTransform,
            Vector3 pivotWorldPosition,
            Quaternion saberWorldRotation
        ) {
            _swingBenchmarkController.UpdateSaberTransform(pivotWorldPosition, saberWorldRotation);

            var tipWorldPosition = pivotWorldPosition + saberWorldRotation * Vector3.forward;

            _swingAnalyzer.Update(
                controllerTransform,
                tipWorldPosition,
                pivotWorldPosition,
                _cameraRotation * Vector3.left,
                out var planePosition,
                out var planeRotation,
                out var tipDeviation,
                out var pivotDeviation,
                out var pivotHeight,
                out var minimalSwingAngle,
                out var maximalSwingAngle
            );

            _isSwingGood = (maximalSwingAngle - minimalSwingAngle) > FullSwingAngleRequirement;

            _swingBenchmarkController.SetValues(
                _selectedHand == Hand.Left,
                planePosition,
                planeRotation,
                tipDeviation,
                pivotDeviation,
                pivotHeight,
                minimalSwingAngle,
                maximalSwingAngle,
                FullSwingAngleRequirement
            );
        }

        public void Finalize(Vector3 saberLocalDirection) {
            switch (_selectedHand) {
                case Hand.Left:
                    _leftHandVisible = _isSwingGood;
                    _leftWristRotationAxis = _swingAnalyzer.GetWristRotationAxis();
                    _leftHandLocalRotation = Quaternion.LookRotation(_leftWristRotationAxis, saberLocalDirection);
                    break;
                case Hand.Right:
                    _rightHandVisible = _isSwingGood;
                    _rightWristRotationAxis = _swingAnalyzer.GetWristRotationAxis();
                    _rightHandLocalRotation = Quaternion.LookRotation(_rightWristRotationAxis, saberLocalDirection);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            UpdateVisibility();
        }

        #endregion

        #region HandTransform

        private Vector3 _leftWristRotationAxis = Vector3.forward;
        private Vector3 _rightWristRotationAxis = Vector3.forward;
        private Quaternion _leftHandLocalRotation = Quaternion.identity;
        private Quaternion _rightHandLocalRotation = Quaternion.identity;

        private void OnControllerTransformsUpdated(ReeTransform leftControllerTransform, ReeTransform rightControllerTransform) {
            var leftHandWorldPosition = leftControllerTransform.LocalToWorldPosition(PluginConfig.LeftHandPivotPosition);
            var leftHandWorldRotation = leftControllerTransform.LocalToWorldRotation(_leftHandLocalRotation);
            var rightHandWorldPosition = rightControllerTransform.LocalToWorldPosition(PluginConfig.RightHandPivotPosition);
            var rightHandWorldRotation = rightControllerTransform.LocalToWorldRotation(_rightHandLocalRotation);

            TransformUtils.ApplyRoomOffset(_mainSettingsModel, ref leftHandWorldPosition, ref leftHandWorldRotation);
            TransformUtils.ApplyRoomOffset(_mainSettingsModel, ref rightHandWorldPosition, ref rightHandWorldRotation);

            _swingBenchmarkController.UpdateHandTransforms(
                leftHandWorldPosition,
                leftHandWorldRotation,
                rightHandWorldPosition,
                rightHandWorldRotation
            );
        }

        #endregion

        #region Visibility

        private bool _leftHandVisible;
        private bool _rightHandVisible;

        private void UpdateVisibility() {
            bool swingVisible, handsVisible;

            switch (PluginConfig.AdjustmentMode) {
                case AdjustmentMode.SwingBenchmark:
                    swingVisible = true;
                    handsVisible = true;
                    break;
                case AdjustmentMode.DirectionOnly:
                case AdjustmentMode.DirectionAuto:
                    swingVisible = false;
                    handsVisible = true;
                    break;
                case AdjustmentMode.None:
                case AdjustmentMode.Basic:
                case AdjustmentMode.PivotOnly:
                case AdjustmentMode.RoomOffset:
                    swingVisible = false;
                    handsVisible = false;
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            _gizmosManager.LeftHandGizmosController.SetWristValues(_leftWristRotationAxis, _leftHandVisible);
            _gizmosManager.RightHandGizmosController.SetWristValues(_rightWristRotationAxis, _rightHandVisible);

            _swingBenchmarkController.UpdateVisibility(
                swingVisible,
                handsVisible && _leftHandVisible,
                handsVisible && _rightHandVisible
            );
        }

        #endregion

        #region Tick

        private Vector3 _cameraPosition = Vector3.zero;
        private Quaternion _cameraRotation = Quaternion.identity;

        public void Tick() {
            var mainCamera = Camera.main;
            if (mainCamera == null) return;

            var transform = mainCamera.transform;
            _cameraPosition = transform.position;
            _cameraRotation = transform.rotation;

            _swingBenchmarkController.UpdateCameraPosition(_cameraPosition);
        }

        #endregion

        #region Initialize & Dispose

        public void Initialize() {
            Subscribe();
            UpdateVisibility();
        }

        public void Dispose() {
            UnSubscribe();

            if (_swingBenchmarkController != null) {
                Object.Destroy(_swingBenchmarkController.gameObject);
            }
        }

        #endregion

        #region Events

        private void OnAdjustmentModeChanged(AdjustmentMode value) {
            UpdateVisibility();
        }

        private void Subscribe() {
            PluginConfig.AdjustmentModeChangedEvent += OnAdjustmentModeChanged;
            Abomination.TransformsUpdatedEvent += OnControllerTransformsUpdated;
        }

        private void UnSubscribe() {
            PluginConfig.AdjustmentModeChangedEvent -= OnAdjustmentModeChanged;
            Abomination.TransformsUpdatedEvent -= OnControllerTransformsUpdated;
        }

        #endregion
    }
}