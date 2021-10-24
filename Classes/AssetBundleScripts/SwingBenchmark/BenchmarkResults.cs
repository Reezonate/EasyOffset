using System.Text;
using UnityEngine;

namespace EasyOffset.AssetBundleScripts {
    public class BenchmarkResults : MonoBehaviour {
        #region Serialized

        [SerializeField] private SwingPlane swingPlane;
        [SerializeField] private DistanceIndicator tipDeviationIndicator;
        [SerializeField] private DistanceIndicator pivotDeviationIndicator;
        [SerializeField] private DistanceIndicator pivotHeightIndicator;
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
            var up = planeRotation * Vector3.up;

            var tipDeviationStringBuilder = new StringBuilder();
            tipDeviationStringBuilder.Append((tipDeviation * 200).ToString("F2"));
            tipDeviationStringBuilder.Append(" cm");

            tipDeviationIndicator.SetValues(
                planePosition + forward - right * tipDeviation,
                planePosition + forward + right * tipDeviation,
                tipDeviationStringBuilder.ToString()
            );

            var pivotDeviationStringBuilder = new StringBuilder();
            pivotDeviationStringBuilder.Append((pivotDeviation * 200).ToString("F2"));
            pivotDeviationStringBuilder.Append(" cm");

            pivotDeviationIndicator.SetValues(
                planePosition - up * pivotDeviation,
                planePosition + up * pivotDeviation,
                pivotDeviationStringBuilder.ToString()
            );

            var pivotHeightStringBuilder = new StringBuilder();
            pivotHeightStringBuilder.Append((pivotHeight * 100).ToString("F2"));
            pivotHeightStringBuilder.Append(" cm");

            pivotHeightIndicator.SetValues(
                planePosition - right * pivotHeight,
                planePosition,
                pivotHeightStringBuilder.ToString()
            );
        }

        public void SetLookAt(
            Vector3 lookAt
        ) {
            tipDeviationIndicator.SetLookAt(lookAt);
            pivotDeviationIndicator.SetLookAt(lookAt);
            pivotHeightIndicator.SetLookAt(lookAt);
        }

        public void SetVisible(bool value) {
            visuals.SetActive(value);
        }

        #endregion
    }
}