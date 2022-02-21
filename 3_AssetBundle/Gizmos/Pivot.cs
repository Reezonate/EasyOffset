using UnityEngine;

namespace EasyOffset {
    public class Pivot : MonoBehaviour {
        #region Serialized

        [SerializeField] private VelocityIndicator velocityIndicator;

        #endregion

        #region Interaction

        public void SetVisible(bool value) {
            gameObject.SetActive(value);
        }

        public void SetLookAt(Vector3 lookAt) {
            velocityIndicator.SetLookAt(lookAt);
        }

        #endregion
    }
}