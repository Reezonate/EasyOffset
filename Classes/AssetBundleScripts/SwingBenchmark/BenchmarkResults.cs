using System.Text;
using UnityEngine;

namespace EasyOffset.AssetBundleScripts {
    public class BenchmarkResults : MonoBehaviour {
        #region Serialized

        [SerializeField] private SwingPlane swingPlane;
        [SerializeField] private DistanceIndicator tipDeviationIndicator;
        [SerializeField] private DistanceIndicator pivotDeviationIndicator;
        [SerializeField] private DistanceIndicator pivotHeightIndicator;
        [SerializeField] private AngleIndicator minimalSwingAngleIndicator;
        [SerializeField] private AngleIndicator maximalSwingAngleIndicator;
        [SerializeField] private GameObject visuals;

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
            swingPlane.SetValues(planePosition, planeRotation, pivotDeviation, minimalSwingAngle, maximalSwingAngle);

            var forward = planeRotation * Vector3.right;
            var right = planeRotation * Vector3.back;

            UpdateMinimalSwingAngleIndicator(planePosition, planeRotation, minimalSwingAngle);
            UpdateMaximalSwingAngleIndicator(planePosition, planeRotation, maximalSwingAngle);
            UpdateTipDeviationIndicator(planePosition, tipDeviation, forward, right);
            UpdatePivotDeviationIndicator(planePosition, pivotDeviation, forward);
            UpdatePivotHeightIndicator(planePosition, pivotHeight, right);
        }

        public void SetLookAt(
            Vector3 lookAt
        ) {
            tipDeviationIndicator.SetLookAt(lookAt);
            pivotDeviationIndicator.SetLookAt(lookAt);
            pivotHeightIndicator.SetLookAt(lookAt);
            minimalSwingAngleIndicator.SetLookAt(lookAt);
            maximalSwingAngleIndicator.SetLookAt(lookAt);
        }

        public void SetVisible(bool value) {
            visuals.SetActive(value);
        }

        #endregion

        #region Indicators

        private void UpdateMinimalSwingAngleIndicator(Vector3 planePosition, Quaternion planeRotation, float minimalSwingAngle) {
            minimalSwingAngleIndicator.SetValues(
                planePosition,
                planeRotation,
                minimalSwingAngle,
                $"{-minimalSwingAngle * Mathf.Rad2Deg:F2} °"
            );
        }

        private void UpdateMaximalSwingAngleIndicator(Vector3 planePosition, Quaternion planeRotation, float maximalSwingAngle) {
            maximalSwingAngleIndicator.SetValues(
                planePosition,
                planeRotation,
                maximalSwingAngle,
                $"{maximalSwingAngle * Mathf.Rad2Deg:F2} °"
            );
        }

        private void UpdateTipDeviationIndicator(Vector3 planePosition, float tipDeviation, Vector3 forward, Vector3 right) {
            tipDeviationIndicator.SetValues(
                planePosition + forward - right * tipDeviation,
                planePosition + forward + right * tipDeviation,
                $"{tipDeviation * 200:F2} cm"
            );
        }

        private void UpdatePivotDeviationIndicator(Vector3 planePosition, float pivotDeviation, Vector3 forward) {
            pivotDeviationIndicator.SetValues(
                planePosition + forward * pivotDeviation,
                planePosition - forward * pivotDeviation,
                $"{pivotDeviation * 200:F2} cm"
            );
        }

        private void UpdatePivotHeightIndicator(Vector3 planePosition, float pivotHeight, Vector3 right) {
            pivotHeightIndicator.SetValues(
                planePosition - right * pivotHeight,
                planePosition,
                $"{pivotHeight * 100:F2} cm"
            );
        }

        #endregion
    }
}