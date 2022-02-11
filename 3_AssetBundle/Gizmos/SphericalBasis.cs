using TMPro;
using UnityEngine;

namespace EasyOffset {
    public class SphericalBasis : MonoBehaviour {
        #region Serialized

        [SerializeField] private Material material;
        [SerializeField] private MeshRenderer meshRenderer;
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

        private static readonly int StraightSwingPlanePropertyId = Shader.PropertyToID("_StraightSwingPlane");
        private static readonly int StraightSwingPlaneMultiplierPropertyId = Shader.PropertyToID("_StraightSwingPlaneMultiplier");

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
            UpdateMaterial();
        }

        #endregion

        #region UpdateMaterials

        private Vector3 _wristRotationAxis;
        private bool _wristPlaneVisible;

        private Vector4 _upDownPlaneVector;
        private Vector4 _leftRightPlaneVector;
        private bool _previousDirectionVisible;

        private Vector3 _orthoDirection = Vector3.forward;
        private Vector2 _sphericalCoordinates;

        private void UpdateMaterial() {
            if (!_isReady) return;

            _materialInstance.SetFloat(AlphaPropertyId, _currentAlpha);
            _materialInstance.SetFloat(ScalePropertyId, _currentScale);
            _materialInstance.SetFloat(FadeRadiusPropertyId, _currentFadeRadius);

            _materialInstance.SetVector(StraightSwingPlanePropertyId, _wristRotationAxis);
            _materialInstance.SetFloat(StraightSwingPlaneMultiplierPropertyId, _wristPlaneVisible ? 1f : 0f);

            _materialInstance.SetVector(UpDownPlanePropertyId, _upDownPlaneVector);
            _materialInstance.SetVector(LeftRightPlanePropertyId, _leftRightPlaneVector);
            _materialInstance.SetFloat(PlanesMultiplierPropertyId, _previousDirectionVisible ? 1f : 0f);

            _materialInstance.SetVector(SphericalCoordinatesPropertyId, _sphericalCoordinates);
            _materialInstance.SetVector(OrthoDirectionPropertyId, _orthoDirection);
        }

        #endregion

        #region LateUpdate

        private void LateUpdate() {
            var t = Time.deltaTime * 10;
            _currentAlpha = Mathf.Lerp(_currentAlpha, _targetAlpha, t);
            _currentScale = Mathf.Lerp(_currentScale, _targetScale, t);
            _currentFadeRadius = Mathf.Lerp(_currentFadeRadius, _targetFadeRadius, t);
            sphereTransform.localPosition = -_orthoDirection * (_currentScale - 1.0f);
            sphereTransform.localScale = new Vector3(_currentScale, _currentScale, _currentScale);
        }

        #endregion

        #region Interaction

        private const float NoFocusAlpha = 0.2f;
        private const float FocusAlpha = 1.0f;

        private const float NoFocusFadeRadius = 20 * Mathf.Deg2Rad;
        private const float FocusFadeRadius = 50 * Mathf.Deg2Rad;

        public void SetWristValues(Vector3 rotationAxis, bool visible) {
            _wristRotationAxis = rotationAxis;
            _wristPlaneVisible = visible;
            UpdateMaterial();
        }

        public void SetFocus(bool value) {
            _targetAlpha = value ? FocusAlpha : NoFocusAlpha;
            _targetFadeRadius = value ? FocusFadeRadius : NoFocusFadeRadius;
        }

        public void Zoom(float magnitude) {
            _targetScale = magnitude;
        }

        public void SetTextLookAt(Vector3 lookAt) {
            UpdateTextRotation(lookAt);
        }

        public void SetPivotPosition(Vector3 pivotPosition) {
            transform.localPosition = pivotPosition;
        }

        public void SetPreviousDirection(Vector3 orthoDirection, bool visible) {
            var localUp = transform.InverseTransformDirection(Vector3.up);
            var lookFromCenter = Quaternion.LookRotation(orthoDirection, localUp);

            var upDownPlane = new Plane(lookFromCenter * Vector3.right, orthoDirection);
            _upDownPlaneVector = upDownPlane.normal;
            _upDownPlaneVector.w = upDownPlane.distance;

            var leftRightPlane = new Plane(lookFromCenter * Vector3.up, orthoDirection);
            _leftRightPlaneVector = leftRightPlane.normal;
            _leftRightPlaneVector.w = leftRightPlane.distance;

            _previousDirectionVisible = visible;

            UpdateMaterial();
        }

        public void SetDirection(Vector3 orthoDirection) {
            _orthoDirection = orthoDirection;
            _sphericalCoordinates = TransformUtils.OrthoToSphericalDirection(orthoDirection);

            UpdateTextString(_sphericalCoordinates);
            UpdateTextPosition(orthoDirection);
            UpdateMaterial();
        }

        public void SetVisible(bool value) {
            gameObject.SetActive(value);

            if (!value) return;
            _currentAlpha = _targetAlpha;
            _currentScale = _targetScale;
            _currentFadeRadius = _targetFadeRadius;
        }

        #endregion

        #region Text

        private void UpdateTextString(Vector2 sphericalRadians) {
            var xString = (sphericalRadians.x * Mathf.Rad2Deg).ToString("0.00");
            var yString = (sphericalRadians.y * Mathf.Rad2Deg).ToString("0.00");

            textMesh.text = $"<mspace=0.5em><color=red>X:{xString}°</color>\n<color=green>Y:{yString}°</color></mspace>";
            textMesh.alignment = TextAlignmentOptions.Midline;
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