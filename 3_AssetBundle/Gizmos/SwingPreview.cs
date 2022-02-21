using UnityEngine;

namespace EasyOffset {
    public class SwingPreview : MonoBehaviour {
        #region Serialized

        [SerializeField] private SimpleTrail closeTrail;
        [SerializeField] private SimpleTrail farTrail;

        #endregion

        #region Interaction

        public void SetLookAt(Vector3 lookAt) {
            closeTrail.SetLookAt(lookAt);
            farTrail.SetLookAt(lookAt);
        }

        public void SetVisible(bool value) {
            gameObject.SetActive(value);
        }

        #endregion
    }
}