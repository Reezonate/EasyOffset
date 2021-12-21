using UnityEngine;

namespace EasyOffset {
    public class VelocityIndicator : MonoBehaviour {
        #region Serialized

        [SerializeField] private Material pointMaterial;
        [SerializeField] private MeshRenderer pointMeshRenderer;
        [SerializeField] private SimpleTrail trail;

        [SerializeField] private Color minimalVelocityColor;
        [SerializeField] private Color maximalVelocityColor;

        [SerializeField] private float minimalVelocity;
        [SerializeField] private float maximalVelocity;

        [SerializeField] private float smoothFactor = 10f;

        #endregion

        #region ShaderProperties

        private static readonly int ColorPropertyId = Shader.PropertyToID("_Color");

        #endregion

        #region Start

        private Material _materialInstance;

        private void Start() {
            _materialInstance = Instantiate(pointMaterial);
            pointMeshRenderer.material = _materialInstance;
        }

        #endregion

        #region Update

        private bool _hasPreviousPosition;
        private Vector3 _previousPosition;
        private float _smoothedVelocity;

        private void Update() {
            var currentPosition = transform.position;

            if (_hasPreviousPosition) {
                var velocity = (currentPosition - _previousPosition).magnitude / Time.deltaTime;
                _smoothedVelocity = Mathf.Lerp(_smoothedVelocity, velocity, Time.deltaTime * smoothFactor);
                var t = Mathf.InverseLerp(minimalVelocity, maximalVelocity, _smoothedVelocity);
                var color = Color.Lerp(minimalVelocityColor, maximalVelocityColor, t);
                _materialInstance.SetColor(ColorPropertyId, color);
            }

            _previousPosition = currentPosition;
            _hasPreviousPosition = true;
        }

        #endregion

        #region Interaction

        public void SetLookAt(Vector3 lookAt) {
            trail.SetLookAt(lookAt);
        }

        #endregion
    }
}