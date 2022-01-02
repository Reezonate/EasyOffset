using TMPro;
using UnityEngine;

namespace EasyOffset {
    public class OrthonormalBasis : MonoBehaviour {
        #region Serialized

        [SerializeField] private Transform pointerTransform;
        [SerializeField] private GameObject axlesVisuals;
        [SerializeField] private GameObject pointerVisuals;
        [SerializeField] private TextMeshPro textMesh;
        [SerializeField] private Transform textRoot;

        [SerializeField] private Material pointerMaterial;
        [SerializeField] private MeshRenderer pointerMeshRenderer;

        [SerializeField] private Material xArrowMaterial;
        [SerializeField] private MeshRenderer xArrowMeshRenderer;

        [SerializeField] private Material yArrowMaterial;
        [SerializeField] private MeshRenderer yArrowMeshRenderer;

        [SerializeField] private Material zArrowMaterial;
        [SerializeField] private MeshRenderer zArrowMeshRenderer;

        #endregion

        #region ShaderProperties

        private static readonly int ScalePropertyId = Shader.PropertyToID("_Scale");
        private static readonly int AlphaPropertyId = Shader.PropertyToID("_Alpha");

        #endregion

        #region Start

        private Material _pointerMaterialInstance;
        private Material _xArrowMaterialInstance;
        private Material _yArrowMaterialInstance;
        private Material _zArrowMaterialInstance;
        private bool _isReady;

        private void Start() {
            _pointerMaterialInstance = Instantiate(pointerMaterial);
            pointerMeshRenderer.material = _pointerMaterialInstance;

            _xArrowMaterialInstance = Instantiate(xArrowMaterial);
            xArrowMeshRenderer.material = _xArrowMaterialInstance;

            _yArrowMaterialInstance = Instantiate(yArrowMaterial);
            yArrowMeshRenderer.material = _yArrowMaterialInstance;

            _zArrowMaterialInstance = Instantiate(zArrowMaterial);
            zArrowMeshRenderer.material = _zArrowMaterialInstance;

            _targetAlpha = _currentAlpha = NoFocusAlpha;
            _isReady = true;
        }

        #endregion

        #region Update

        private const float NoFocusAlpha = 0.2f;
        private const float FocusAlpha = 1.0f;

        private float _targetAlpha;
        private float _currentAlpha;

        private void Update() {
            var t = Time.deltaTime * 10f;
            _currentAlpha = Mathf.Lerp(_currentAlpha, _targetAlpha, t);

            _pointerMaterialInstance.SetFloat(AlphaPropertyId, _currentAlpha);
            _xArrowMaterialInstance.SetFloat(AlphaPropertyId, _currentAlpha);
            _yArrowMaterialInstance.SetFloat(AlphaPropertyId, _currentAlpha);
            _zArrowMaterialInstance.SetFloat(AlphaPropertyId, _currentAlpha);
        }

        #endregion

        #region Interaction

        public void SetFocus(bool value) {
            _targetAlpha = value ? FocusAlpha : NoFocusAlpha;
        }

        public void SetTextLookAt(Vector3 worldPosition) {
            UpdateTextLookAt(worldPosition);
        }

        public void SetCoordinates(
            Vector3 coordinates
        ) {
            if (!_isReady) return;
            _pointerMaterialInstance.SetVector(ScalePropertyId, coordinates);
            pointerTransform.localScale = coordinates;

            UpdateTextString(coordinates);
            UpdateTextPosition(coordinates);
        }

        public void SetVisible(
            bool isBasisVisible,
            bool isPointerVisible,
            bool isTextVisible
        ) {
            axlesVisuals.SetActive(isBasisVisible);
            pointerVisuals.SetActive(isPointerVisible);
            textMesh.gameObject.SetActive(isTextVisible);
        }

        #endregion

        #region Text

        private void UpdateTextString(Vector3 coordinates) {
            var xString = (coordinates.x * 100.0).ToString("0.0");
            var yString = (coordinates.y * 100.0).ToString("0.0");
            var zString = (coordinates.z * 100.0).ToString("0.0");

            textMesh.text = $"<color=red>{xString}</color>  <color=green>{yString}</color>  <color=blue>{zString}</color>";
        }

        private void UpdateTextPosition(Vector3 coordinates) {
            textRoot.localPosition = coordinates;
        }

        private void UpdateTextLookAt(Vector3 lookAt) {
            textMesh.transform.LookAt(lookAt, Vector3.up);
        }

        #endregion
    }
}