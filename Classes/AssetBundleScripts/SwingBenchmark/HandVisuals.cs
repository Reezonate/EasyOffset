using UnityEngine;

namespace EasyOffset.AssetBundleScripts {
    public class HandVisuals : MonoBehaviour {
        #region Serialized

        [SerializeField] private Transform handTransform;

        #endregion

        #region Interaction

        public void UpdateTransform(
            Vector3 worldPosition,
            Quaternion worldRotation
        ) {
            handTransform.position = worldPosition;
            handTransform.rotation = worldRotation;
        }

        #endregion
    }
}