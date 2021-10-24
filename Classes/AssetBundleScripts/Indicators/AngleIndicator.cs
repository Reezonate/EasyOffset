using UnityEngine;

namespace EasyOffset.AssetBundleScripts {
    public class AngleIndicator : MonoBehaviour {
        #region Serialized

        [SerializeField] private Material material;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Transform planeTransform;
        [SerializeField] private IndicatorText indicatorText;
        [SerializeField] private float radius = 1.0f;
        
        [SerializeField] private Vector2 textOffset;

        #endregion

        #region Shader Properties

        private static readonly int ScalePropertyId = Shader.PropertyToID("_Scale");
        private static readonly int AnglePropertyId = Shader.PropertyToID("_AngleRadians");

        #endregion

        #region Start

        private Material _materialInstance;
        private bool _isReady;

        private void Start() {
            _materialInstance = Instantiate(material);
            meshRenderer.material = _materialInstance;

            indicatorText.SetOffset(textOffset);
            planeTransform.localScale = new Vector3(radius, radius, radius);
            _materialInstance.SetFloat(ScalePropertyId, radius);
            _isReady = true;
        }

        #endregion

        #region Interaction

        public void SetValues(
            Vector3 position,
            Quaternion rotation,
            float angleRadians,
            string text
        ) {
            if (!_isReady) return;

            planeTransform.position = position;
            planeTransform.rotation = rotation;

            _materialInstance.SetFloat(AnglePropertyId, angleRadians);

            var textPointerAngle = angleRadians / 2;
            var textLocalPosition = new Vector3(
                Mathf.Cos(textPointerAngle) * radius,
                Mathf.Sin(textPointerAngle) * radius,
                0f
            );
            indicatorText.SetPosition(position + rotation * textLocalPosition);
            indicatorText.SetText(text);
        }

        public void SetLookAt(
            Vector3 lookAt
        ) {
            indicatorText.SetLookAt(lookAt);
        }

        #endregion
    }
}