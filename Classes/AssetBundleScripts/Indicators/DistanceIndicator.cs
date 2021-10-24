using UnityEngine;

namespace EasyOffset.AssetBundleScripts {
    public class DistanceIndicator : MonoBehaviour {
        #region Serialized

        [SerializeField] private Material material;
        [SerializeField] private MeshRenderer planeMeshRenderer;
        [SerializeField] private Transform planeTransform;
        [SerializeField] private IndicatorText indicatorText;

        [SerializeField] private float planeWidth = 1;
        [SerializeField] private float lineMargin;

        [SerializeField] private Vector2 textOffset;
        [SerializeField] private bool flipTextPosition;

        #endregion

        #region ShaderProperties

        private static readonly int ScalePropertyId = Shader.PropertyToID("_Scale");
        private static readonly int LineMarginPropertyId = Shader.PropertyToID("_LineMargin");

        #endregion

        #region Start

        private Material _materialInstance;
        private bool _ready;

        private void Start() {
            _materialInstance = Instantiate(material);
            planeMeshRenderer.material = _materialInstance;
            _materialInstance.SetFloat(LineMarginPropertyId, lineMargin);

            UpdateTextPosition();

            _ready = true;
        }

        #endregion

        #region Update

        private void Update() {
            var rotation = _direction == Vector3.zero ? Quaternion.identity : Quaternion.LookRotation(_direction, _lookDirection);
            planeTransform.rotation = rotation;
        }

        private void UpdateTextPosition() {
            if (flipTextPosition) {
                var offset = new Vector2(-textOffset.x, textOffset.y);

                indicatorText.SetPosition(_toPosition);
                indicatorText.SetOffset(offset);
            } else {
                indicatorText.SetPosition(_fromPosition);
                indicatorText.SetOffset(textOffset);
            }
        }

        #endregion

        #region Interaction

        private Vector3 _fromPosition = Vector3.zero;
        private Vector3 _toPosition = Vector3.zero;
        private Vector3 _lookDirection = Vector3.up;
        private Vector3 _direction = Vector3.forward;

        public void SetValues(
            Vector3 from,
            Vector3 to,
            string text
        ) {
            if (!_ready) return;

            var relative = to - from;
            var distance = relative.magnitude;

            _fromPosition = from;
            _toPosition = to;
            _direction = relative.normalized;

            var planePosition = from - _direction * lineMargin;
            var planeLength = distance + lineMargin * 2;
            var planeScale = new Vector3(planeWidth, 1.0f, planeLength);

            planeTransform.localScale = planeScale;
            planeTransform.position = planePosition;
            _materialInstance.SetVector(ScalePropertyId, planeScale);

            UpdateTextPosition();
            indicatorText.SetText(text);
        }

        public void SetLookAt(
            Vector3 lookAt
        ) {
            _lookDirection = lookAt - _fromPosition;
            indicatorText.SetLookAt(lookAt);
        }

        #endregion
    }
}