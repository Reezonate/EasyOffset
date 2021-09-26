using UnityEngine;

namespace EasyOffset.SyncedWithUnity {
    public class Pivot : MonoBehaviour {
        #region Serialized

        [SerializeField] private Transform pointTransform;
        [SerializeField] private GameObject visuals;

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

        #endregion
    }
}