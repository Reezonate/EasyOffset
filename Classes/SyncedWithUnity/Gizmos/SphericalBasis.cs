using TMPro;
using UnityEngine;

namespace EasyOffset.SyncedWithUnity {
    public class SphericalBasis : MonoBehaviour {
        #region Serialized

        [SerializeField] private Material material;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private GameObject visuals;
        [SerializeField] private TextMeshPro textMesh;
        [SerializeField] private Transform textRoot;

        #endregion

        #region ShaderProperties

        private static readonly int SphericalCoordinatesPropertyId = Shader.PropertyToID("_SphericalCoordinates");
        private static readonly int PrevSphericalCoordinatesPropertyId = Shader.PropertyToID("_PrevSphericalCoordinates");
        private static readonly int OrthoDirectionPropertyId = Shader.PropertyToID("_OrthoDirection");
        private static readonly int ScalePropertyId = Shader.PropertyToID("_Scale");
        private static readonly int AlphaPropertyId = Shader.PropertyToID("_Alpha");

        #endregion

        #region Start

        private Material _materialInstance;
        private bool _isReady;

        private float _currentAlpha;
        private float _targetAlpha;

        private float _currentScale;
        private float _targetScale;

        private void Start() {
            _materialInstance = Instantiate(material);
            meshRenderer.material = _materialInstance;
            _currentScale = _targetScale = 1.0f;
            _currentAlpha = _targetAlpha = NoFocusAlpha;
            _isReady = true;
        }

        #endregion

        #region Update

        private void Update() {
            _currentAlpha = Mathf.Lerp(_currentAlpha, _targetAlpha, Time.deltaTime * 10);
            _currentScale = Mathf.Lerp(_currentScale, _targetScale, Time.deltaTime * 10);
            _materialInstance.SetFloat(AlphaPropertyId, _currentAlpha);
            _materialInstance.SetFloat(ScalePropertyId, _currentScale);
        }

        #endregion

        #region Interaction

        private const float NoFocusAlpha = 0.2f;
        private const float FocusAlpha = 1.0f;

        public void SetFocus(bool value) {
            _targetAlpha = value ? FocusAlpha : NoFocusAlpha;
        }

        public void Zoom(
            float magnitude
        ) {
            if (!_isReady) return;
            _targetScale = 1 / magnitude;
        }

        public void SetTextLookAt(
            Vector3 lookAt
        ) {
            UpdateTextRotation(lookAt);
        }

        public void SetPreviousDirection(
            Vector3 orthoDirection,
            bool visible
        ) {
            if (!_isReady) return;
            
            var sphericalCoordinates = TransformUtils.OrthoToSphericalDirection(orthoDirection);
            var v4 = (Vector4) sphericalCoordinates;
            v4.z = visible ? 1 : 0;
            _materialInstance.SetVector(PrevSphericalCoordinatesPropertyId, v4);
        }

        public void SetDirection(
            Vector3 orthoDirection
        ) {
            if (!_isReady) return;

            var sphericalCoordinates = TransformUtils.OrthoToSphericalDirection(orthoDirection);
            _materialInstance.SetVector(SphericalCoordinatesPropertyId, sphericalCoordinates);
            _materialInstance.SetVector(OrthoDirectionPropertyId, orthoDirection);

            UpdateTextString(sphericalCoordinates);
            UpdateTextPosition(orthoDirection);
        }

        public void SetVisible(bool value) {
            visuals.SetActive(value);
        }

        #endregion

        #region Text

        private void UpdateTextString(Vector2 sphericalRadians) {
            var xString = (-sphericalRadians.x * Mathf.Rad2Deg).ToString("0.00");
            var yString = (sphericalRadians.y * Mathf.Rad2Deg).ToString("0.00");

            textMesh.text = $"<mspace=0.5em><color=red>X:{xString}°</color>\n<color=green>Y:{yString}°</color></mspace>";
        }

        private void UpdateTextPosition(Vector3 orthoDirection) {
            textRoot.localPosition = orthoDirection;
        }

        private void UpdateTextRotation(Vector3 lookAt) {
            textRoot.LookAt(lookAt, Vector3.up);
        }

        #endregion
    }
}