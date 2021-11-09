using UnityEngine;

namespace EasyOffset.AssetBundleScripts {
    public class Pivot : MonoBehaviour {
        #region Serialized

        [SerializeField] private Transform pointTransform;
        [SerializeField] private GameObject visuals;
        [SerializeField] private VelocityIndicator velocityIndicator;

        #endregion

        #region Interaction

        public void SetPosition(
            Vector3 position
        ) {
            pointTransform.localPosition = position;
        }

        public void SetVisible(bool value) {
            visuals.SetActive(value);
        }

        public void SetLookAt(Vector3 lookAt) {
            velocityIndicator.SetLookAt(lookAt);
        }

        #endregion
    }
}