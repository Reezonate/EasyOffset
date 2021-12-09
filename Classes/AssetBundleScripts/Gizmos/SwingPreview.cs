using UnityEngine;

namespace EasyOffset.AssetBundleScripts {
    public class SwingPreview : MonoBehaviour {
        #region Serialized

        [SerializeField] private GameObject visuals;
        [SerializeField] private SimpleTrail closeTrail;
        [SerializeField] private SimpleTrail farTrail;

        #endregion

        #region Interaction

        public void SetLookAt(Vector3 lookAt) {
            closeTrail.SetLookAt(lookAt);
            farTrail.SetLookAt(lookAt);
        }

        public void SetSaberRotation(Quaternion rotation) {
            transform.localRotation = rotation;
        }

        public void SetVisible(bool value) {
            visuals.gameObject.SetActive(value);
        }

        #endregion
    }
}