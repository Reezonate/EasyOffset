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
        #region Constructor

        private readonly SwingBenchmarkController _swingBenchmarkController;

        public SwingBenchmarkManager() {
            var gameObject = Object.Instantiate(BundleLoader.SwingBenchmarkController);
            Object.DontDestroyOnLoad(gameObject);
            _swingBenchmarkController = gameObject.GetComponent<SwingBenchmarkController>();
        }

        #endregion

        #region Data

        private const int MaximalCapacity = 120;

        private readonly WeightedList<Vector3> _tipPositions = new(MaximalCapacity);
        private readonly WeightedList<Vector3> _pivotPositions = new(MaximalCapacity);

        #endregion

        #region StartTracking

        public void StartTracking() {
            _tipPositions.Clear();
            _pivotPositions.Clear();

            _swingBenchmarkController.StartTracking();
            _swingBenchmarkController.HideCone();
        }

        #endregion

        #region UpdateTracking

        public void UpdateTracking(
            Vector3 tipPosition,
            Vector3 pivotPosition,
            Quaternion saberRotation
        ) {
            _tipPositions.Add(tipPosition, 1.0f);
            _pivotPositions.Add(pivotPosition, 1.0f);
            _swingBenchmarkController.UpdateSaberTransform(pivotPosition, saberRotation);

            var averagePivotPosition = _pivotPositions.GetAverage();
            var pivotDeviation = _pivotPositions.GetDeviationFromPoint(averagePivotPosition);
            var tipMovementPlane = _tipPositions.CalculatePlane(_cameraRotation * Vector3.left);
            var tipDeviation = _tipPositions.GetDeviationFromPlane(tipMovementPlane);
            var pivotHeight = tipMovementPlane.GetDistanceToPoint(averagePivotPosition);
            var planePosition = tipMovementPlane.ClosestPointOnPlane(averagePivotPosition);
            var planeRotation = Quaternion.LookRotation(tipMovementPlane.normal, Vector3.up);

            _tipPositions.GetSwingAngles(
                planePosition,
                planeRotation,
                out var minimalSwingAngle,
                out var maximalSwingAngle
            );

            _swingBenchmarkController.SetValues(
                planePosition,
                planeRotation,
                tipDeviation,
                pivotDeviation,
                pivotHeight,
                minimalSwingAngle,
                maximalSwingAngle
            );

            _swingBenchmarkController.ShowCone();
        }

        #endregion

        #region StopTracking

        public void StopTracking() { }

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

            OnAdjustmentModeChanged(PluginConfig.AdjustmentMode);
        }

        public void Dispose() {
            UnSubscribe();

            if (_swingBenchmarkController != null) {
                Object.Destroy(_swingBenchmarkController);
            }
        }

        #endregion

        #region Events handling

        private void OnAdjustmentModeChanged(AdjustmentMode value) {
            _swingBenchmarkController.SetVisible(value == AdjustmentMode.SwingBenchmark);
        }

        private void Subscribe() {
            PluginConfig.AdjustmentModeChangedEvent += OnAdjustmentModeChanged;
        }

        private void UnSubscribe() {
            PluginConfig.AdjustmentModeChangedEvent -= OnAdjustmentModeChanged;
        }

        #endregion
    }
}