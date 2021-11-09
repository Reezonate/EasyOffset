using UnityEngine;

namespace EasyOffset.AssetBundleScripts {
    public class StaticTrail : MonoBehaviour {
        #region Serialized

        [SerializeField] private Material material;
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Transform targetTransform;
        [SerializeField] private int lifetime;
        [SerializeField] private int resolution;
        [SerializeField] private float width;

        #endregion

        #region Start

        private TrailMesh _trailMesh;
        private Material _materialInstance;

        private void Start() {
            ResetMesh();
            _materialInstance = Instantiate(material);
            meshRenderer.material = _materialInstance;
        }

        #endregion

        #region ResetMesh

        private void ResetMesh() {
            _trailMesh = new TrailMesh(lifetime, resolution, width);
            meshFilter.mesh = _trailMesh.Mesh;
        }

        #endregion

        #region Update

        private void Update() {
            if (!_updating) return;
            _trailMesh.Update(targetTransform.position, targetTransform.forward);
        }

        #endregion

        #region Interaction

        private bool _updating;

        public void StartTracking() {
            _updating = true;
            ResetMesh();
        }

        public void StopTracking() {
            _updating = false;
        }

        #endregion
    }
}