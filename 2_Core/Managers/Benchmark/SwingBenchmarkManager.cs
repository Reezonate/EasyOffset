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

            Quaternion referenceRotation;

            switch (_selectedHand) {
                case Hand.Left:
                    PluginConfig.CreateUndoStep("Left Benchmark");
                    _leftWristRotationAxis = _swingAnalyzer.GetWristRotationAxis();
                    referenceRotation = CalculateReferenceRotation(_leftWristRotationAxis, PluginConfig.LeftSaberRotation);
                    PluginConfig.SetLeftSaberReference(_isSwingGood, referenceRotation);
                    break;
                case Hand.Right:
                    PluginConfig.CreateUndoStep("Right Benchmark");
                    _rightWristRotationAxis = _swingAnalyzer.GetWristRotationAxis();
                    referenceRotation = CalculateReferenceRotation(_rightWristRotationAxis, PluginConfig.RightSaberRotation);
                    PluginConfig.SetRightSaberReference(_isSwingGood, referenceRotation);
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

        private static Quaternion CalculateReferenceRotation(Vector3 straightSwingPlaneNormal, Quaternion currentLocalRotation) {
            var currentLocalDirection = TransformUtils.DirectionFromRotation(currentLocalRotation);
            var projectedDirection = new Plane(straightSwingPlaneNormal, 0.0f).ClosestPointOnPlane(currentLocalDirection);
            var lookRotation = Quaternion.LookRotation(projectedDirection, straightSwingPlaneNormal);
            return lookRotation * Quaternion.Euler(0.0f, 0.0f, 90.0f);
        }

        #endregion

        #region AutoFix

        private Vector3 _leftWristRotationAxis = Vector3.forward;
        private Vector3 _rightWristRotationAxis = Vector3.forward;

        private void ApplyAutoFix() {
            if (!_hasAnyResults || !_isSwingGood) return;

            Vector3 directionToFix;
            Vector3 fixedDirection;

            switch (_selectedHand) {
                case Hand.Left:
                    directionToFix = TransformUtils.DirectionFromRotation(PluginConfig.LeftSaberRotation);
                    fixedDirection = new Plane(_leftWristRotationAxis, 0f).ClosestPointOnPlane(directionToFix);
                    PluginConfig.LeftSaberRotation = Quaternion.FromToRotation(directionToFix, fixedDirection) * PluginConfig.LeftSaberRotation;
                    break;
                case Hand.Right:
                    directionToFix = TransformUtils.DirectionFromRotation(PluginConfig.RightSaberRotation);
                    fixedDirection = new Plane(_rightWristRotationAxis, 0f).ClosestPointOnPlane(directionToFix);
                    PluginConfig.RightSaberRotation = Quaternion.FromToRotation(directionToFix, fixedDirection) * PluginConfig.RightSaberRotation;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(_selectedHand), _selectedHand, null);
            }
        }

        #endregion

        #region Visibility

        private void UpdateVisibility() {
            var swingVisible = PluginConfig.AdjustmentMode switch {
                AdjustmentMode.SwingBenchmark => _hasAnyResults,
                AdjustmentMode.Rotation => false,
                AdjustmentMode.RotationAuto => false,
                AdjustmentMode.None => false,
                AdjustmentMode.Basic => false,
                AdjustmentMode.Position => false,
                AdjustmentMode.Precise => false,
                AdjustmentMode.RoomOffset => false,
                _ => throw new ArgumentOutOfRangeException()
            };

            _swingBenchmarkController.UpdateVisibility(swingVisible);
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
            UpdateVisibility();
        }

        private void OnAdjustmentModeChanged(AdjustmentMode value) {
            UpdateVisibility();
        }

        private void Subscribe() {
            PluginConfig.AdjustmentModeChangedEvent += OnAdjustmentModeChanged;
            SwingBenchmarkHelper.OnResetEvent += OnReset;
            SwingBenchmarkHelper.OnAutoFixEvent += ApplyAutoFix;
        }

        private void UnSubscribe() {
            PluginConfig.AdjustmentModeChangedEvent -= OnAdjustmentModeChanged;
            SwingBenchmarkHelper.OnResetEvent -= OnReset;
            SwingBenchmarkHelper.OnAutoFixEvent -= ApplyAutoFix;
        }

        #endregion
    }
}