using UnityEngine;

namespace EasyOffset {
    public class LegacyGimbal : MonoBehaviour {
        #region Serialized

        [SerializeField] private Transform arrowsTransform;
        [SerializeField] private Transform gimbalRingXTransform;
        [SerializeField] private Transform gimbalRingYTransform;
        [SerializeField] private Material gimbalRingXMaterial;
        [SerializeField] private Material gimbalRingYMaterial;
        [SerializeField] private MeshRenderer gimbalRingXMeshRenderer;
        [SerializeField] private MeshRenderer gimbalRingYMeshRenderer;

        #endregion

        #region Shader properties

        private static readonly int AngleOffsetPropertyId = Shader.PropertyToID("_AngleOffset");

        #endregion

        #region Start

        private Material _gimbalRingXMaterialInstance;
        private Material _gimbalRingYMaterialInstance;
        private bool _isReady;

        private void Start() {
            _gimbalRingXMaterialInstance = Instantiate(gimbalRingXMaterial);
            _gimbalRingYMaterialInstance = Instantiate(gimbalRingYMaterial);
            gimbalRingXMeshRenderer.material = _gimbalRingXMaterialInstance;
            gimbalRingYMeshRenderer.material = _gimbalRingYMaterialInstance;
            _isReady = true;
            UpdateMaterials();
        }

        #endregion

        #region UpdateMaterials

        private float _xAngleOffset;
        private float _yAngleOffset;

        private void UpdateMaterials() {
            if (!_isReady) return;
            _gimbalRingXMaterialInstance.SetFloat(AngleOffsetPropertyId, _xAngleOffset);
            _gimbalRingYMaterialInstance.SetFloat(AngleOffsetPropertyId, _yAngleOffset);
        }

        #endregion

        #region Interaction

        public void SetValues(
            Vector3 hiddenConfigPosition,
            Vector3 hiddenConfigRotation,
            Vector3 legacyConfigPosition,
            Vector3 legacyConfigRotation
        ) {
            var finalPosition = legacyConfigPosition + hiddenConfigPosition;
            var finalRotation = legacyConfigRotation + hiddenConfigRotation;
            
            gimbalRingYTransform.localRotation = Quaternion.Euler(0.0f, finalRotation.y, 0.0f);
            gimbalRingXTransform.localRotation = Quaternion.Euler(finalRotation.x, 0.0f, 0.0f);
            arrowsTransform.localPosition = finalPosition;

            _xAngleOffset = -legacyConfigRotation.x * Mathf.Deg2Rad;
            _yAngleOffset = legacyConfigRotation.y * Mathf.Deg2Rad;
            UpdateMaterials();
        }

        public void SetVisible(bool value) {
            gameObject.SetActive(value);
        }

        #endregion
    }
}