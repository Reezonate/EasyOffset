using UnityEngine;

namespace EasyOffset {
    public class Pivot : MonoBehaviour {
        #region Interaction

        public void SetVisible(bool value) {
            gameObject.SetActive(value);
        }

        #endregion
    }
}