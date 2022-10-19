using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.XR;
using Zenject;
using Object = UnityEngine.Object;

namespace EasyOffset {
    [UsedImplicitly]
    public class SwingBenchmarkManager : IInitializable, IDisposable, ITickable {
        #region Constants

        private const int MinimalCapacity = 60;
        private const int MaximalCapacity = 300;
        private const float ProbeTime = 2.0f;
        private const float FullSwingAngleRequirement = 140 * Mathf.Deg2Rad;

        #endregion

        #region Variables

        private readonly SwingAnalyzer _swingAnalyzer;
        private readonly SwingBenchmarkController _swingBenchmarkController;

        private Hand _selectedHand = Hand.Left;
        private bool _isSwingGood;
        private bool _hasAnyResults;
        private float _swingCurveAngleDeg;

        private Quaternion _leftReferenceRotation = Quaternion.identity;
        private Quaternion _rightReferenceRotation = Quaternion.identity;

        #endregion

        #region Constructor

        public SwingBenchmarkManager() {
            var capacity = (int) Mathf.Clamp(XRDevice.refreshRate * ProbeTime, MinimalCapacity, MaximalCapacity);

            var gameObject = Object.Instantiate(BundleLoader.SwingBenchmarkController);
            _swingBenchmarkController = gameObject.GetComponent<SwingBenchmarkController>();
            _swingBenchmarkController.SetTrailLifetime(capacity);
            _swingAnalyzer = new SwingAnalyzer(capacity);
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
            _swingCurveAngleDeg = swingCurveAngle * Mathf.Rad2Deg;

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

            var averageNormal = _swingAnalyzer.GetAverageLocalPlaneNormal();

            switch (_selectedHand) {
                case Hand.Left:
                    _leftReferenceRotation = CalculateReferenceRotation(averageNormal, PluginConfig.LeftSaberRotation);
                    break;
                case Hand.Right:
                    _rightReferenceRotation = CalculateReferenceRotation(averageNormal, PluginConfig.RightSaberRotation);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            if (_isSwingGood) {
                SwingBenchmarkHelper.InvokeSuccess();
            } else {
                SwingBenchmarkHelper.InvokeFail();
            }
        }

        private Quaternion CalculateReferenceRotation(
            Vector3 averageNormal,
            Quaternion currentLocalRotation
        ) {
            var currentLocalDirection = TransformUtils.DirectionFromRotation(currentLocalRotation);
            var referenceRotation = Quaternion.LookRotation(currentLocalDirection, averageNormal);
            referenceRotation *= Quaternion.Euler(-_swingCurveAngleDeg, 0.0f, 0.0f);
            referenceRotation *= Quaternion.Euler(0.0f, 0.0f, 90.0f);
            return referenceRotation;
        }

        private static Quaternion CalculateReferenceRotationDep(Vector3 straightSwingPlaneNormal, Quaternion currentLocalRotation) {
            var currentLocalDirection = TransformUtils.DirectionFromRotation(currentLocalRotation);
            var projectedDirection = new Plane(straightSwingPlaneNormal, 0.0f).ClosestPointOnPlane(currentLocalDirection);
            var lookRotation = Quaternion.LookRotation(projectedDirection, straightSwingPlaneNormal);
            return lookRotation * Quaternion.Euler(0.0f, 0.0f, 90.0f);
        }

        #endregion

        #region AutoFix

        private void ApplyAutoFix() {
            if (!_hasAnyResults || !_isSwingGood) return;

            switch (_selectedHand) {
                case Hand.Left:
                    PluginConfig.CreateUndoStep("Left Auto-fix");
                    PluginConfig.LeftSaberRotation = TransformUtils.AlignForwardVectors(
                        PluginConfig.LeftSaberRotation,
                        _leftReferenceRotation
                    );
                    break;
                case Hand.Right:
                    PluginConfig.CreateUndoStep("Right Auto-fix");
                    PluginConfig.RightSaberRotation = TransformUtils.AlignForwardVectors(
                        PluginConfig.RightSaberRotation,
                        _rightReferenceRotation
                    );
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(_selectedHand), _selectedHand, null);
            }
        }

        #endregion

        #region OnSetAsReference

        private void OnSetAsReference() {
            if (!_hasAnyResults || !_isSwingGood) return;

            switch (_selectedHand) {
                case Hand.Left:
                    PluginConfig.CreateUndoStep("Set Left Reference");
                    PluginConfig.SetLeftSaberReference(true, _leftReferenceRotation);
                    break;
                case Hand.Right:
                    PluginConfig.CreateUndoStep("Set Right Reference");
                    PluginConfig.SetRightSaberReference(true, _rightReferenceRotation);
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
                AdjustmentMode.PositionAuto => false,
                AdjustmentMode.None => false,
                AdjustmentMode.Basic => false,
                AdjustmentMode.Position => false,
                AdjustmentMode.Direct => false,
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
            SwingBenchmarkHelper.OnSetAsReferenceEvent += OnSetAsReference;
        }

        private void UnSubscribe() {
            PluginConfig.AdjustmentModeChangedEvent -= OnAdjustmentModeChanged;
            SwingBenchmarkHelper.OnResetEvent -= OnReset;
            SwingBenchmarkHelper.OnAutoFixEvent -= ApplyAutoFix;
            SwingBenchmarkHelper.OnSetAsReferenceEvent -= OnSetAsReference;
        }

        #endregion
    }
}