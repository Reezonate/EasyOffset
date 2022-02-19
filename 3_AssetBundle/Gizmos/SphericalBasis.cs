using TMPro;
using UnityEngine;

namespace EasyOffset {
    public class SphericalBasis : MonoBehaviour {
        #region Serialized

        [SerializeField] private Material material;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Transform sphereTransform;

        [SerializeField] private Transform textRoot;
        [SerializeField] private TextMeshPro xText;
        [SerializeField] private TextMeshPro yText;
        [SerializeField] private TextMeshPro balanceText;
        [SerializeField] private TextMeshPro curveText;

        #endregion

        #region ShaderProperties

        private static readonly int OrthoDirectionPropertyId = Shader.PropertyToID("_OrthoDirection");
        private static readonly int PreviousOrthoDirectionPropertyId = Shader.PropertyToID("_PreviousOrthoDirection");
        private static readonly int PointerPlanePropertyId = Shader.PropertyToID("_PointerPlane");
        private static readonly int PreviousPointerMultiplierPropertyId = Shader.PropertyToID("_PreviousPointerMultiplier");

        private static readonly int StraightSwingVerticalPlanePropertyId = Shader.PropertyToID("_StraightSwingVerticalPlane");
        private static readonly int StraightSwingHorizontalPlanePropertyId = Shader.PropertyToID("_StraightSwingHorizontalPlane");
        private static readonly int StraightSwingPlaneMultiplierPropertyId = Shader.PropertyToID("_StraightSwingPlaneMultiplier");

        private static readonly int ScalePropertyId = Shader.PropertyToID("_Scale");
        private static readonly int AlphaPropertyId = Shader.PropertyToID("_Alpha");
        private static readonly int FadeRadiusPropertyId = Shader.PropertyToID("_FadeRadius");

        #endregion

        #region Lerp Ranges & Variables

        private float _currentFocus, _targetFocus;
        private float _currentScale, _targetScale;
        private float _xRotationOffset, _yRotationOffset;

        private static readonly Range TextOffsetXRange = new Range(2.5f, 6.0f);
        private static readonly Range TextOffsetYRange = new Range(5.0f, 7.0f);
        private static readonly Range AlphaRange = new Range(0.2f, 1.0f);
        private static readonly Range FadeRadiusRange = new Range(20 * Mathf.Deg2Rad, 50 * Mathf.Deg2Rad);

        #endregion

        #region Start

        private Material _materialInstance;
        private bool _isReady;

        private void Start() {
            _materialInstance = Instantiate(material);
            meshRenderer.material = _materialInstance;
            _currentScale = _targetScale = 1.0f;
            _currentFocus = _targetFocus = 0.0f;
            _xRotationOffset = TextOffsetXRange.Start;
            _yRotationOffset = TextOffsetYRange.Start;
            _isReady = true;
            UpdateMaterial();
        }

        #endregion

        #region UpdateMaterials

        private float _alpha, _fadeRadius;
        private bool _hasReference;

        private Vector4 _straightSwingVerticalPlane;
        private Vector4 _straightSwingHorizontalPlane;

        private Vector3 _orthoDirection = Vector3.forward;
        private Vector3 _previousOrthoDirection = Vector3.forward;
        private Vector4 _pointerPlaneVector;

        private void UpdateMaterial() {
            if (!_isReady) return;

            _materialInstance.SetFloat(AlphaPropertyId, _alpha);
            _materialInstance.SetFloat(ScalePropertyId, _currentScale);
            _materialInstance.SetFloat(FadeRadiusPropertyId, _fadeRadius);

            _materialInstance.SetVector(StraightSwingVerticalPlanePropertyId, _straightSwingVerticalPlane);
            _materialInstance.SetVector(StraightSwingHorizontalPlanePropertyId, _straightSwingHorizontalPlane);
            _materialInstance.SetFloat(StraightSwingPlaneMultiplierPropertyId, _hasReference ? 1f : 0f);

            _materialInstance.SetVector(OrthoDirectionPropertyId, _orthoDirection);
            _materialInstance.SetVector(PreviousOrthoDirectionPropertyId, _previousOrthoDirection);
            _materialInstance.SetVector(PointerPlanePropertyId, _pointerPlaneVector);
            _materialInstance.SetFloat(PreviousPointerMultiplierPropertyId, _focused ? 1f : 0f);
        }

        #endregion

        #region LateUpdate

        private void LateUpdate() {
            var t = Time.deltaTime * 10;

            _currentFocus = Mathf.Lerp(_currentFocus, _targetFocus, t);
            _alpha = AlphaRange.SlideBy(_currentFocus);
            _fadeRadius = FadeRadiusRange.SlideBy(_currentFocus);
            _xRotationOffset = TextOffsetXRange.SlideBy(_currentFocus);
            _yRotationOffset = TextOffsetYRange.SlideBy(_currentFocus);

            _currentScale = Mathf.Lerp(_currentScale, _targetScale, t);
            sphereTransform.localPosition = -_orthoDirection * (_currentScale - 1.0f);
            sphereTransform.localScale = new Vector3(_currentScale, _currentScale, _currentScale);

            UpdateAllTextTransforms();
            UpdateMaterial();
        }

        #endregion

        #region SetHand

        private Hand _hand;

        public void SetHand(Hand hand) {
            _hand = hand;
        }

        #endregion

        #region SetRotations

        public void SetRotations(
            Quaternion rotation,
            bool hasReference,
            Quaternion referenceRotation
        ) {
            _orthoDirection = rotation * Vector3.forward;
            _hasReference = hasReference;

            _straightSwingVerticalPlane = referenceRotation * Vector3.left;
            _straightSwingHorizontalPlane = referenceRotation * Vector3.up;

            var sphericalRadians = TransformUtils.OrthoToSphericalDirection(_orthoDirection);
            TransformUtils.ToReferenceSpace(rotation, referenceRotation, out var curve, out var balance);
            UpdateAllTextStrings(sphericalRadians.x * Mathf.Rad2Deg, sphericalRadians.y * Mathf.Rad2Deg, balance, curve);

            UpdateTextVisibility();
            UpdatePointerPlane();
            UpdateMaterial();
        }

        #endregion

        #region Focus

        private bool _focused;

        public void SetFocus(bool value) {
            if (_focused == value) return;

            _previousOrthoDirection = _orthoDirection;
            UpdatePointerPlane();
            UpdateMaterial();

            _focused = value;
            _targetFocus = value ? 1.0f : 0.0f;
        }

        private void UpdatePointerPlane() {
            _pointerPlaneVector = new Plane(Vector3.zero, _orthoDirection, _previousOrthoDirection).normal;
        }

        #endregion

        #region Zoom

        public void Zoom(float magnitude) {
            _targetScale = magnitude;
        }

        #endregion

        #region SetVisible

        public void SetVisible(bool value) {
            gameObject.SetActive(value);
        }

        #endregion

        #region Text

        private void UpdateTextVisibility() {
            balanceText.gameObject.SetActive(_hasReference);
            curveText.gameObject.SetActive(_hasReference);
        }

        private void UpdateAllTextStrings(float x, float y, float balance, float curve) {
            UpdateTextString(xText, "X", x);
            UpdateTextString(yText, "Y", y);
            UpdateTextString(balanceText, "B", balance);
            UpdateTextString(curveText, "C", _hand == Hand.Right ? curve : -curve);
        }

        private void UpdateAllTextTransforms() {
            var localUp = transform.InverseTransformVector(Vector3.up);
            textRoot.localRotation = Quaternion.LookRotation(_orthoDirection, localUp);

            UpdateTextLocalTransform(xText, -_xRotationOffset, -_yRotationOffset);
            UpdateTextLocalTransform(yText, -_xRotationOffset, _yRotationOffset);
            UpdateTextLocalTransform(balanceText, _xRotationOffset, -_yRotationOffset);
            UpdateTextLocalTransform(curveText, _xRotationOffset, _yRotationOffset);
        }

        private static void UpdateTextString(TMP_Text text, string prefix, float angleDegrees) {
            text.alignment = TextAlignmentOptions.Midline;
            text.text = $"{prefix}: <mspace=0.5em>{angleDegrees:F2}°";
        }

        private static void UpdateTextLocalTransform(Component text, float xRotationOffset, float yRotationOffset) {
            var textTransform = text.transform;

            var rotation = Quaternion.Euler(xRotationOffset, yRotationOffset, 0.0f);
            textTransform.localPosition = rotation * Vector3.forward;
            textTransform.localRotation = rotation;
        }

        #endregion
    }
}