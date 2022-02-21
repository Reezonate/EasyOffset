using UnityEngine;

namespace EasyOffset {
    public class SwingBenchmarkController : MonoBehaviour {
        #region Serialized

        [SerializeField] private BenchmarkTrails benchmarkTrails;
        [SerializeField] private SwingIndicators swingIndicators;

        [SerializeField] private GameObject swingVisuals;

        #endregion

        #region Visibility

        public void UpdateVisibility(
            bool isSwingVisible
        ) {
            swingVisuals.SetActive(isSwingVisible);
        }

        #endregion

        #region Interaction

        public void SetValues(
            bool isLeft,
            Vector3 planePosition,
            Quaternion planeRotation,
            float tipDeviation,
            float pivotDeviation,
            float swingCurveAngle,
            float minimalSwingAngle,
            float maximalSwingAngle,
            float fullSwingAngleRequirement
        ) {
            swingIndicators.SetValues(
                isLeft,
                planePosition,
                planeRotation,
                tipDeviation,
                pivotDeviation,
                swingCurveAngle,
                minimalSwingAngle,
                maximalSwingAngle,
                fullSwingAngleRequirement
            );
        }

        public void UpdateCameraPosition(Vector3 position) {
            benchmarkTrails.SetLookAt(position);
            swingIndicators.SetLookAt(position);
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