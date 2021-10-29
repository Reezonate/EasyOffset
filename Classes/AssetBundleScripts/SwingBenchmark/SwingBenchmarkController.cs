using UnityEngine;

namespace EasyOffset.AssetBundleScripts {
    public class SwingBenchmarkController : MonoBehaviour {
        #region Serialized

        [SerializeField] private BenchmarkTrails benchmarkTrails;
        [SerializeField] private SwingIndicators swingIndicators;

        [SerializeField] private GameObject swingVisuals;
        [SerializeField] private HandVisuals leftHandVisuals;
        [SerializeField] private HandVisuals rightHandVisuals;

        #endregion

        #region Visibility

        public void UpdateVisibility(
            bool isSwingVisible,
            bool isLeftHandVisible,
            bool isRightHandVisible
        ) {
            swingVisuals.SetActive(isSwingVisible);
            leftHandVisuals.gameObject.SetActive(isLeftHandVisible);
            rightHandVisuals.gameObject.SetActive(isRightHandVisible);
        }

        #endregion

        #region Interaction

        public void SetValues(
            bool isLeft,
            Vector3 planePosition,
            Quaternion planeRotation,
            float tipDeviation,
            float pivotDeviation,
            float pivotHeight,
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
                pivotHeight,
                minimalSwingAngle,
                maximalSwingAngle,
                fullSwingAngleRequirement
            );
        }

        public void UpdateCameraPosition(Vector3 position) {
            benchmarkTrails.SetLookAt(position);
            swingIndicators.SetLookAt(position);
        }

        public void UpdateHandTransforms(
            Vector3 leftPosition,
            Quaternion leftRotation,
            Vector3 rightPosition,
            Quaternion rightRotation
        ) {
            leftHandVisuals.UpdateTransform(leftPosition, leftRotation);
            rightHandVisuals.UpdateTransform(rightPosition, rightRotation);
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