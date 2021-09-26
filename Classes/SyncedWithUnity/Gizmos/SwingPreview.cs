using UnityEngine;

namespace EasyOffset.SyncedWithUnity {
    public class SwingPreview : MonoBehaviour {
        #region Serialized

        [SerializeField] private GameObject visuals;
        [SerializeField] private Trail closeTrail;
        [SerializeField] private Trail farTrail;

        #endregion

        #region Interaction

        public void SetSaberRotation(Quaternion rotation) {
            transform.localRotation = rotation;
        }

        public void SetVisible(bool value) {
            if (value) {
                closeTrail.ResetMesh();
                farTrail.ResetMesh();
            }

            visuals.gameObject.SetActive(value);
        }

        #endregion
    }
}