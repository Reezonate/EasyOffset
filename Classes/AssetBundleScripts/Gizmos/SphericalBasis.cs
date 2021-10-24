using TMPro;
using UnityEngine;

namespace EasyOffset.AssetBundleScripts {
    public class SphericalBasis : MonoBehaviour {
        #region Serialized

        [SerializeField] private Material material;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private GameObject visuals;
        [SerializeField] private Transform sphereTransform;
        [SerializeField] private TextMeshPro textMesh;
        [SerializeField] private Transform textRoot;

        #endregion

        #region ShaderProperties

        private static readonly int SphericalCoordinatesPropertyId = Shader.PropertyToID("_SphericalCoordinates");
        private static readonly int OrthoDirectionPropertyId = Shader.PropertyToID("_OrthoDirection");
        private static readonly int UpDownPlanePropertyId = Shader.PropertyToID("_UpDownPlane");
        private static readonly int LeftRightPlanePropertyId = Shader.PropertyToID("_LeftRightPlane");
        private static readonly int PlanesMultiplierPropertyId = Shader.PropertyToID("_PlanesMultiplier");
        private static readonly int ScalePropertyId = Shader.PropertyToID("_Scale");
        private static readonly int AlphaPropertyId = Shader.PropertyToID("_Alpha");
        private static readonly int FadeRadiusPropertyId = Shader.PropertyToID("_FadeRadius");

        #endregion

        #region Start

        private Material _materialInstance;
        private bool _isReady;

        private float _currentFadeRadius;
        private float _targetFadeRadius;

        private float _currentAlpha;
        private float _targetAlpha;

        private float _currentScale;
        private float _targetScale;

        private void Start() {
            _materialInstance = Instantiate(material);
            meshRenderer.material = _materialInstance;
            _currentScale = _targetScale = 1.0f;
            _currentAlpha = _targetAlpha = NoFocusAlpha;
            _currentFadeRadius = _targetFadeRadius = NoFocusFadeRadius;
            _isReady = true;
        }

        #endregion

        #region Update

        private void Update() {
            var t = Time.deltaTime * 10;
            
            _currentAlpha = Mathf.Lerp(_currentAlpha, _targetAlpha, t);
            _materialInstance.SetFloat(AlphaPropertyId, _currentAlpha);
            
            _currentScale = Mathf.Lerp(_currentScale, _targetScale, t);
            _materialInstance.SetFloat(ScalePropertyId, _currentScale);
            
            _currentFadeRadius = Mathf.Lerp(_currentFadeRadius, _targetFadeRadius, t);
            _materialInstance.SetFloat(FadeRadiusPropertyId, _currentFadeRadius);

            sphereTransform.localPosition = -_orthoDirection * (_currentScale - 1.0f);
            sphereTransform.localScale = new Vector3(_currentScale, _currentScale, _currentScale);
        }

        #endregion

        #region Interaction

        private const float NoFocusAlpha = 0.2f;
        private const float FocusAlpha = 1.0f;
        
        private const float NoFocusFadeRadius = 20 * Mathf.Deg2Rad;
        private const float FocusFadeRadius = 50 * Mathf.Deg2Rad;
        
        private Vector3 _orthoDirection = Vector3.forward;

        public void SetFocus(bool value) {
            _targetAlpha = value ? FocusAlpha : NoFocusAlpha;
            _targetFadeRadius = value ? FocusFadeRadius : NoFocusFadeRadius;
        }

        public void Zoom(
            float magnitude
        ) {
            if (!_isReady) return;
            _targetScale = magnitude;
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

            var localUp = transform.InverseTransformDirection(Vector3.up);
            var lookFromCenter = Quaternion.LookRotation(orthoDirection, localUp);

            var upDownPlane = new Plane(lookFromCenter * Vector3.right, orthoDirection);
            var upDownPlaneVector = (Vector4) upDownPlane.normal;
            upDownPlaneVector.w = upDownPlane.distance;

            var leftRightPlane = new Plane(lookFromCenter * Vector3.up, orthoDirection);
            var leftRightPlaneVector = (Vector4) leftRightPlane.normal;
            leftRightPlaneVector.w = leftRightPlane.distance;
            
            var planesMultiplier = visible ? 1f : 0f;

            _materialInstance.SetVector(UpDownPlanePropertyId, upDownPlaneVector);
            _materialInstance.SetVector(LeftRightPlanePropertyId, leftRightPlaneVector);
            _materialInstance.SetFloat(PlanesMultiplierPropertyId, planesMultiplier);
        }

        public void SetDirection(
            Vector3 orthoDirection
        ) {
            if (!_isReady) return;
            
            _orthoDirection = orthoDirection;

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