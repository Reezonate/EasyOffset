using System;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace EasyOffset {
    [UsedImplicitly]
    public class SwingBenchmarkManager : IInitializable, IDisposable, ITickable {
        #region Constants

        private const int MaximalCapacity = 200;
        private const float FullSwingAngleRequirement = 140 * Mathf.Deg2Rad;

        #endregion

        #region Variables

        private readonly GizmosManager _gizmosManager;

        private readonly SwingAnalyzer _swingAnalyzer;
        private readonly SwingBenchmarkController _swingBenchmarkController;

        private bool _isSwingGood;
        private Hand _selectedHand = Hand.Left;

        private bool _hasAnyResults;
        private bool _leftHandVisible;
        private bool _rightHandVisible;

        #endregion

        #region Constructor

        public SwingBenchmarkManager(
            GizmosManager gizmosManager
        ) {
            _gizmosManager = gizmosManager;
            _swingAnalyzer = new SwingAnalyzer(MaximalCapacity);

            var gameObject = Object.Instantiate(BundleLoader.SwingBenchmarkController);
            _swingBenchmarkController = gameObject.GetComponent<SwingBenchmarkController>();
        }

        #endregion

        #region Start

        public void Start(Hand hand) {
            _selectedHand = hand;
            _hasAnyResults = true;

            _swingAnalyzer.Reset();
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
            SwingBenchmarkHelper.InvokeStart();
        }

        #endregion

        #region Update

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

            var isLeft = _selectedHand == Hand.Left;
            var swingCurveAngle = Mathf.Asin(pivotHeight);
            var fullSwingAngle = maximalSwingAngle - minimalSwingAngle;

            _isSwingGood = fullSwingAngle > FullSwingAngleRequirement;

            _swingBenchmarkController.SetValues(
                isLeft,
                planePosition,
                planeRotation,
                tipDeviation,
                pivotDeviation,
                swingCurveAngle,
                minimalSwingAngle,
                maximalSwingAngle,
                FullSwingAngleRequirement
            );

            SwingBenchmarkHelper.InvokeUpdate(
                isLeft ? -swingCurveAngle : swingCurveAngle,
                tipDeviation,
                pivotDeviation,
                minimalSwingAngle,
                maximalSwingAngle
            );
        }

        #endregion

        #region Finish

        public void Finish() {
            _swingBenchmarkController.StopTracking();

            switch (_selectedHand) {
                case Hand.Left:
                    _leftHandVisible = _isSwingGood;
                    _leftWristRotationAxis = _swingAnalyzer.GetWristRotationAxis();
                    _leftHandLocalRotation = Quaternion.LookRotation(_leftWristRotationAxis, Vector3.up);
                    break;
                case Hand.Right:
                    _rightHandVisible = _isSwingGood;
                    _rightWristRotationAxis = _swingAnalyzer.GetWristRotationAxis();
                    _rightHandLocalRotation = Quaternion.LookRotation(_rightWristRotationAxis, Vector3.up);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            UpdateVisibility();

            if (_isSwingGood) {
                SwingBenchmarkHelper.InvokeSuccess();
            } else {
                SwingBenchmarkHelper.InvokeFail();
            }
        }

        #endregion

        #region AutoFix

        private void ApplyAutoFix() {
            if (!_hasAnyResults || !_isSwingGood) return;

            Vector3 saberLocalDirection;
            Plane localPlane;

            switch (_selectedHand) {
                case Hand.Left:
                    saberLocalDirection = PluginConfig.LeftHandSaberDirection;
                    localPlane = new Plane(_leftWristRotationAxis, 0f);
                    PluginConfig.LeftHandSaberDirection = localPlane.ClosestPointOnPlane(saberLocalDirection);
                    break;
                case Hand.Right:
                    saberLocalDirection = PluginConfig.RightHandSaberDirection;
                    localPlane = new Plane(_rightWristRotationAxis, 0f);
                    PluginConfig.RightHandSaberDirection = localPlane.ClosestPointOnPlane(saberLocalDirection);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(_selectedHand), _selectedHand, null);
            }
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

            TransformUtils.ApplyRoomOffset(ref leftHandWorldPosition, ref leftHandWorldRotation);
            TransformUtils.ApplyRoomOffset(ref rightHandWorldPosition, ref rightHandWorldRotation);

            _swingBenchmarkController.UpdateHandTransforms(
                leftHandWorldPosition,
                leftHandWorldRotation,
                rightHandWorldPosition,
                rightHandWorldRotation
            );
        }

        #endregion

        #region Visibility

        private void UpdateVisibility() {
            bool swingVisible, handsVisible;

            switch (PluginConfig.AdjustmentMode) {
                case AdjustmentMode.SwingBenchmark:
                    swingVisible = true;
                    handsVisible = true;
                    break;
                case AdjustmentMode.Rotation:
                    swingVisible = false;
                    handsVisible = true;
                    break;
                case AdjustmentMode.RotationAuto:
                case AdjustmentMode.None:
                case AdjustmentMode.Basic:
                case AdjustmentMode.Position:
                case AdjustmentMode.RoomOffset:
                    swingVisible = false;
                    handsVisible = false;
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            _gizmosManager.LeftHandGizmosController.SetWristValues(_leftWristRotationAxis, _leftHandVisible);
            _gizmosManager.RightHandGizmosController.SetWristValues(_rightWristRotationAxis, _rightHandVisible);

            _swingBenchmarkController.UpdateVisibility(
                swingVisible && _hasAnyResults,
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
        }

        #endregion

        #region Events

        private void OnReset() {
            _hasAnyResults = false;
            _leftHandVisible = false;
            _rightHandVisible = false;
            UpdateVisibility();
        }

        private void OnAdjustmentModeChanged(AdjustmentMode value) {
            UpdateVisibility();
        }

        private void Subscribe() {
            PluginConfig.AdjustmentModeChangedEvent += OnAdjustmentModeChanged;
            Abomination.TransformsUpdatedEvent += OnControllerTransformsUpdated;
            SwingBenchmarkHelper.OnResetEvent += OnReset;
            SwingBenchmarkHelper.OnAutoFixEvent += ApplyAutoFix;
        }

        private void UnSubscribe() {
            PluginConfig.AdjustmentModeChangedEvent -= OnAdjustmentModeChanged;
            Abomination.TransformsUpdatedEvent -= OnControllerTransformsUpdated;
            SwingBenchmarkHelper.OnResetEvent -= OnReset;
            SwingBenchmarkHelper.OnAutoFixEvent -= ApplyAutoFix;
        }

        #endregion
    }
}