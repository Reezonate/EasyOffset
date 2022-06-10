using UnityEngine;

namespace EasyOffset {
    public class SwingPreview : MonoBehaviour {
        #region Interaction

        public void SetVisible(bool value) {
            gameObject.SetActive(value);
        }

        #endregion
    }
}