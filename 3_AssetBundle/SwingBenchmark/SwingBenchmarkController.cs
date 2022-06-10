using UnityEngine;

namespace EasyOffset {
    public class SwingBenchmarkController : MonoBehaviour {
        #region Serialized

        [SerializeField] private ReeTrail trail;
        [SerializeField] private SwingIndicators swingIndicators;

        #endregion

        #region Visibility

        public void UpdateVisibility(bool isSwingVisible) {
            gameObject.SetActive(isSwingVisible);
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
            swingIndicators.SetLookAt(position);
        }

        public void UpdateSaberTransform(Vector3 position, Quaternion rotation) {
            trail.transform.SetPositionAndRotation(position, rotation);
        }

        public void SetTrailLifetime(int frames) {
            trail.SetLifetime(frames);
        }

        public void StartTracking() {
            trail.StartTracking();
        }

        public void StopTracking() {
            trail.StopTracking();
        }

        #endregion
    }
}