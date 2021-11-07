using UnityEngine;

namespace EasyOffset.AssetBundleScripts {
    public class AngleIndicator : MonoBehaviour {
        #region Serialized

        [SerializeField] private Material material;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Transform planeTransform;
        [SerializeField] private IndicatorText indicatorText;
        [SerializeField] private float armThickness = 0.001f;
        [SerializeField] private float radius = 1.0f;

        #endregion

        #region Shader Properties

        private static readonly int ScalePropertyId = Shader.PropertyToID("_Scale");
        private static readonly int AnglePropertyId = Shader.PropertyToID("_AngleRadians");
        private static readonly int AngleOffsetPropertyId = Shader.PropertyToID("_AngleOffset");
        private static readonly int ArmThicknessPropertyId = Shader.PropertyToID("_ArmThickness");

        #endregion

        #region Start

        private Material _materialInstance;
        private bool _isReady;

        private void Start() {
            _materialInstance = Instantiate(material);
            meshRenderer.material = _materialInstance;

            planeTransform.localScale = new Vector3(radius, radius, radius);
            _materialInstance.SetFloat(ScalePropertyId, radius);
            _materialInstance.SetFloat(ArmThicknessPropertyId, armThickness);
            _isReady = true;
        }

        #endregion

        #region Interaction

        public void SetTransform(
            Vector3 position,
            Quaternion rotation
        ) {
            planeTransform.position = position;
            planeTransform.rotation = rotation;
        }

        public void SetValues(
            float angleRadians,
            float angleOffset,
            string text
        ) {
            if (!_isReady) return;

            _materialInstance.SetFloat(AnglePropertyId, angleRadians);
            _materialInstance.SetFloat(AngleOffsetPropertyId, angleOffset);

            var textPointerAngle = angleRadians / 2 + angleOffset;
            var textLocalPosition = new Vector3(
                Mathf.Cos(textPointerAngle) * radius,
                Mathf.Sin(textPointerAngle) * radius,
                0f
            );

            indicatorText.SetPosition(planeTransform.position + planeTransform.rotation * textLocalPosition);
            indicatorText.SetText(text);
        }

        public void SetTextOffset(
            Vector2 value
        ) {
            indicatorText.SetOffset(value);
        }

        public void SetLookAt(
            Vector3 lookAt
        ) {
            indicatorText.SetLookAt(lookAt);
        }

        #endregion
    }
}