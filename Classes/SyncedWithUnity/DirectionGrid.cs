using UnityEngine;

namespace EasyOffset.SyncedWithUnity {
    public class DirectionGrid : MonoBehaviour {
        #region Static

        private static readonly int AlphaPropertyId = Shader.PropertyToID("_Alpha");
        private static readonly int DirectionPropertyId = Shader.PropertyToID("_Direction");
        private const float FadeSpeed = 10.0f;

        #endregion

        #region Serialized

        [SerializeField] private Material material;
        [SerializeField] private MeshRenderer meshRenderer;

        #endregion

        #region Start

        private Material _materialInstance;
        private float _targetAlpha;
        private float _alpha;

        private void Start() {
            _materialInstance = Instantiate(material);
            meshRenderer.material = _materialInstance;
            UpdateVisibility();
        }

        #endregion

        #region Visibility

        private bool _visible;

        public void SetVisible(bool value) {
            _visible = value;
            UpdateVisibility();
        }

        private void UpdateVisibility() {
            meshRenderer.enabled = _visible;
            if (_visible) return;
            _alpha = 0;
        }

        #endregion

        #region Update

        private void Update() {
            if (!_visible) return;
            _alpha = Mathf.Lerp(_alpha, _targetAlpha, Time.deltaTime * FadeSpeed);
            _materialInstance.SetFloat(AlphaPropertyId, _alpha);
        }

        #endregion

        #region Interaction

        public void SetTargetAlpha(float value) {
            _targetAlpha = value;
        }

        public void SetValues(
            Vector3 position,
            Vector3 direction,
            Vector3 pointerDirection
        ) {
            transform.position = position;
            _materialInstance.SetVector(DirectionPropertyId, pointerDirection);

            if (direction == Vector3.zero) return;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        #endregion
    }
}