using UnityEngine;

namespace EasyOffset {
    public class SwingIndicators : MonoBehaviour {
        #region Serialized

        [SerializeField] private Transform swingPlaneTransform;
        [SerializeField] private Material swingPlaneMaterial;
        [SerializeField] private MeshRenderer swingPlaneMeshRenderer;

        [SerializeField] private DistanceIndicator tipDeviationIndicator;
        [SerializeField] private DistanceIndicator pivotDeviationIndicator;
        [SerializeField] private AngleIndicator swingCurveAngleIndicator;
        [SerializeField] private AngleIndicator minimalSwingAngleIndicator;
        [SerializeField] private AngleIndicator maximalSwingAngleIndicator;
        [SerializeField] private AngleIndicator fullSwingAngleIndicator;

        [SerializeField] private Vector2 normalTextOffset;
        [SerializeField] private Vector2 flippedTextOffset;

        #endregion

        #region ShaderProperties

        private static readonly int DataQualityPropertyId = Shader.PropertyToID("_DataQuality");
        private static readonly int PivotDeviationPropertyId = Shader.PropertyToID("_PivotDeviation");
        private static readonly int MinimalAnglePropertyId = Shader.PropertyToID("_MinimalAngleRadians");
        private static readonly int MaximalAnglePropertyId = Shader.PropertyToID("_MaximalAngleRadians");
        private static readonly int AverageAnglePropertyId = Shader.PropertyToID("_AverageAngleRadians");

        #endregion

        #region Start

        private Material _materialInstance;
        private bool _ready;

        private void Start() {
            _materialInstance = Instantiate(swingPlaneMaterial);
            swingPlaneMeshRenderer.material = _materialInstance;
            UpdateTextOffsets();
            _ready = true;
        }

        #endregion

        #region Update

        private float _currentDataQuality;
        private float _targetDataQuality;

        private float _fullSwingAngleRequirement;

        private float _currentMinimalSwingAngle;
        private float _targetMinimalSwingAngle;

        private float _currentMaximalSwingAngle;
        private float _targetMaximalSwingAngle;

        private void Update() {
            var t = Time.deltaTime * 10f;

            _currentDataQuality = Mathf.Lerp(_currentDataQuality, _targetDataQuality, t);
            _currentMinimalSwingAngle = Mathf.Lerp(_currentMinimalSwingAngle, _targetMinimalSwingAngle, t);
            _currentMaximalSwingAngle = Mathf.Lerp(_currentMaximalSwingAngle, _targetMaximalSwingAngle, t);
            var averageSwingAngle = (_currentMaximalSwingAngle + _currentMinimalSwingAngle) / 2;

            _materialInstance.SetFloat(DataQualityPropertyId, _currentDataQuality);
            _materialInstance.SetFloat(MinimalAnglePropertyId, _currentMinimalSwingAngle);
            _materialInstance.SetFloat(MaximalAnglePropertyId, _currentMaximalSwingAngle);
            _materialInstance.SetFloat(AverageAnglePropertyId, averageSwingAngle);

            UpdateAngleIndicatorsSmooth();
        }

        #endregion

        #region Visibility

        private void UpdateVisibility(bool isDataGood) {
            tipDeviationIndicator.gameObject.SetActive(isDataGood);
            pivotDeviationIndicator.gameObject.SetActive(isDataGood);
            swingCurveAngleIndicator.gameObject.SetActive(isDataGood);
            minimalSwingAngleIndicator.gameObject.SetActive(isDataGood);
            maximalSwingAngleIndicator.gameObject.SetActive(isDataGood);
            fullSwingAngleIndicator.gameObject.SetActive(!isDataGood);
        }

        #endregion

        #region SetValues

        private bool _isLeft;
        private bool _flipped;
        private Plane _plane;

        public void SetValues(
            bool isLeft,
            Vector3 planePosition,
            Quaternion planeRotation,
            float tipDeviation,
            float pivotDeviation,
            float swingCurveAngle,
            float minimalSwingAngle,
            float maximalSwingAngle,
            float fullSwingAngleRequirement
        ) {
            if (!_ready) return;

            _isLeft = isLeft;
            _plane = new Plane(planeRotation * Vector3.forward, planePosition);

            UpdateAngles(minimalSwingAngle, maximalSwingAngle, fullSwingAngleRequirement);
            UpdateSwingPlane(planePosition, planeRotation, pivotDeviation);
            UpdateAngleIndicatorsImmediate(planePosition, planeRotation, swingCurveAngle);
            UpdateDistanceIndicators(planePosition, planeRotation, tipDeviation, pivotDeviation);
        }

        public void SetLookAt(Vector3 lookAt) {
            tipDeviationIndicator.SetLookAt(lookAt);
            pivotDeviationIndicator.SetLookAt(lookAt);
            swingCurveAngleIndicator.SetLookAt(lookAt);
            minimalSwingAngleIndicator.SetLookAt(lookAt);
            maximalSwingAngleIndicator.SetLookAt(lookAt);
            fullSwingAngleIndicator.SetLookAt(lookAt);

            var flipIndicators = _plane.GetDistanceToPoint(lookAt) < 0;
            if (_flipped == flipIndicators) return;
            _flipped = flipIndicators;
            UpdateTextOffsets();
        }

        private void UpdateTextOffsets() {
            var offsetA = _flipped ? flippedTextOffset : normalTextOffset;
            var offsetB = _flipped ? normalTextOffset : flippedTextOffset;

            tipDeviationIndicator.SetTextOffset(offsetA, _flipped);
            pivotDeviationIndicator.SetTextOffset(offsetB, false);

            minimalSwingAngleIndicator.SetTextOffset(offsetA);
            maximalSwingAngleIndicator.SetTextOffset(offsetA);
            fullSwingAngleIndicator.SetTextOffset(offsetA);
            swingCurveAngleIndicator.SetTextOffset(offsetA);
        }

        #endregion

        #region UpdateSwingPlane

        private void UpdateSwingPlane(
            Vector3 position,
            Quaternion rotation,
            float pivotDeviation
        ) {
            swingPlaneTransform.position = position;
            swingPlaneTransform.rotation = rotation;
            _materialInstance.SetFloat(PivotDeviationPropertyId, pivotDeviation);
        }

        #endregion

        #region UpdateAngles

        private void UpdateAngles(float minimalSwingAngle, float maximalSwingAngle, float fullSwingAngleRequirement) {
            _targetMinimalSwingAngle = minimalSwingAngle;
            _targetMaximalSwingAngle = maximalSwingAngle;
            _fullSwingAngleRequirement = fullSwingAngleRequirement;

            var fullSwingAngle = maximalSwingAngle - minimalSwingAngle;
            var isDataGood = fullSwingAngle > fullSwingAngleRequirement;

            UpdateVisibility(isDataGood);
            _targetDataQuality = isDataGood ? 1f : 0f;
        }

        #endregion

        #region UpdateAngleIndicators

        private void UpdateAngleIndicatorsImmediate(
            Vector3 planePosition,
            Quaternion planeRotation,
            float swingCurveAngle
        ) {
            minimalSwingAngleIndicator.SetTransform(planePosition, planeRotation);
            maximalSwingAngleIndicator.SetTransform(planePosition, planeRotation);
            fullSwingAngleIndicator.SetTransform(planePosition, planeRotation);

            var tipTrailLength = Mathf.Cos(swingCurveAngle);
            var offset = planeRotation * Vector3.right * tipTrailLength;
            
            swingCurveAngleIndicator.SetTransform(planePosition + offset, planeRotation * Quaternion.Euler(-90, 180, 0));
            swingCurveAngleIndicator.SetRadius(tipTrailLength);
            swingCurveAngleIndicator.SetValues(
                swingCurveAngle,
                0f,
                $"{(_isLeft ? -swingCurveAngle : swingCurveAngle) * Mathf.Rad2Deg:F1}°"
            );
        }

        private void UpdateAngleIndicatorsSmooth() {
            minimalSwingAngleIndicator.SetValues(
                _currentMinimalSwingAngle,
                0f,
                $"{-_targetMinimalSwingAngle * Mathf.Rad2Deg:F1}°"
            );

            maximalSwingAngleIndicator.SetValues(
                _currentMaximalSwingAngle,
                0f,
                $"{_targetMaximalSwingAngle * Mathf.Rad2Deg:F1}°"
            );

            var currentFullSwingAngle = _currentMaximalSwingAngle - _currentMinimalSwingAngle;
            var targetFullSwingAngle = _targetMaximalSwingAngle - _targetMinimalSwingAngle;

            fullSwingAngleIndicator.SetValues(
                currentFullSwingAngle,
                _currentMinimalSwingAngle,
                $"<color=red>{targetFullSwingAngle * Mathf.Rad2Deg:F1}</color>/{_fullSwingAngleRequirement * Mathf.Rad2Deg:F0}°"
            );
        }

        #endregion

        #region UpdateDistanceIndicators

        private void UpdateDistanceIndicators(Vector3 planePosition, Quaternion planeRotation, float tipDeviation, float pivotDeviation) {
            var forward = planeRotation * Vector3.right;
            var right = planeRotation * Vector3.back;
            var up = planeRotation * Vector3.up;

            UpdateTipDeviationIndicator(planePosition, tipDeviation, forward, right);
            UpdatePivotDeviationIndicator(planePosition, pivotDeviation, up);
        }

        private void UpdateTipDeviationIndicator(Vector3 planePosition, float tipDeviation, Vector3 forward, Vector3 right) {
            tipDeviationIndicator.SetValues(
                planePosition + forward - right * tipDeviation,
                planePosition + forward + right * tipDeviation,
                $"{tipDeviation * 200:F2} cm"
            );
        }

        private void UpdatePivotDeviationIndicator(Vector3 planePosition, float pivotDeviation, Vector3 up) {
            pivotDeviationIndicator.SetValues(
                planePosition + up * pivotDeviation,
                planePosition - up * pivotDeviation,
                $"{pivotDeviation * 200:F2} cm"
            );
        }

        #endregion
    }
}