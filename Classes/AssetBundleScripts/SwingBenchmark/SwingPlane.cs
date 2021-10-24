using UnityEngine;

namespace EasyOffset.AssetBundleScripts {
    public class SwingPlane : MonoBehaviour {
        #region Serialized

        [SerializeField] private Transform swingPlaneTransform;
        [SerializeField] private Material material;
        [SerializeField] private MeshRenderer meshRenderer;

        #endregion

        #region ShaderProperties

        private static readonly int PivotDeviationPropertyId = Shader.PropertyToID("_PivotDeviation");
        private static readonly int MinimalAnglePropertyId = Shader.PropertyToID("_MinimalAngleRadians");
        private static readonly int MaximalAnglePropertyId = Shader.PropertyToID("_MaximalAngleRadians");
        private static readonly int AverageAnglePropertyId = Shader.PropertyToID("_AverageAngleRadians");

        #endregion

        #region Start

        private Material _materialInstance;
        private bool _ready;

        private void Start() {
            _materialInstance = Instantiate(material);
            meshRenderer.material = _materialInstance;
            _ready = true;
        }

        #endregion

        #region Interaction

        public void SetValues(
            Vector3 position,
            Quaternion rotation,
            float pivotDeviation,
            float minimalSwingAngle,
            float maximalSwingAngle
        ) {
            if (!_ready) return;

            swingPlaneTransform.position = position;
            swingPlaneTransform.rotation = rotation;

            var averageSwingAngle = (maximalSwingAngle + minimalSwingAngle) / 2;

            _materialInstance.SetFloat(PivotDeviationPropertyId, pivotDeviation);
            _materialInstance.SetFloat(MinimalAnglePropertyId, minimalSwingAngle);
            _materialInstance.SetFloat(MaximalAnglePropertyId, maximalSwingAngle);
            _materialInstance.SetFloat(AverageAnglePropertyId, averageSwingAngle);
        }

        #endregion
    }
}