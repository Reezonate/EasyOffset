using UnityEngine;

namespace EasyOffset.AssetBundleScripts {
    public class SwingBenchmarkController : MonoBehaviour {
        #region Serialized

        [SerializeField] private BenchmarkTrails benchmarkTrails;
        [SerializeField] private BenchmarkResults benchmarkResults;

        #endregion

        #region Visibility

        private bool _isGlobalVisible;
        private bool _isConeVisible;

        public void SetVisible(bool value) {
            _isGlobalVisible = value;
            UpdateVisibility();
        }

        public void ShowCone() {
            _isConeVisible = true;
            UpdateVisibility();
        }

        public void HideCone() {
            _isConeVisible = false;
            UpdateVisibility();
        }

        private void UpdateVisibility() {
            benchmarkTrails.SetVisible(_isGlobalVisible);
            benchmarkResults.SetVisible(_isGlobalVisible && _isConeVisible);
        }

        #endregion

        #region Interaction

        public void SetValues(
            Vector3 planePosition,
            Quaternion planeRotation,
            float tipDeviation,
            float pivotDeviation,
            float pivotHeight,
            float minimalSwingAngle,
            float maximalSwingAngle
        ) {
            benchmarkResults.SetValues(
                planePosition,
                planeRotation,
                tipDeviation,
                pivotDeviation,
                pivotHeight,
                minimalSwingAngle,
                maximalSwingAngle
            );
        }

        public void UpdateCameraPosition(Vector3 position) {
            benchmarkTrails.SetLookAt(position);
            benchmarkResults.SetLookAt(position);
        }

        public void UpdateSaberTransform(Vector3 position, Quaternion rotation) {
            benchmarkTrails.UpdateSaberTransform(position, rotation);
        }

        public void StartTracking() {
            benchmarkTrails.StartTracking();
        }

        public void StopTracking() {
            benchmarkTrails.StopTracking();
        }

        #endregion
    }
}