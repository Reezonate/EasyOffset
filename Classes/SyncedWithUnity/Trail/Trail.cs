using UnityEngine;

namespace EasyOffset.SyncedWithUnity {
    public class Trail : MonoBehaviour {
        #region Serialized

        [SerializeField] private Material material;
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Transform trailRendererTransform;
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

        public void ResetMesh() {
            _trailMesh = new TrailMesh(lifetime, resolution, width);
            meshFilter.mesh = _trailMesh.Mesh;
        }

        #endregion

        #region Update

        private void Update() {
            trailRendererTransform.position = Vector3.zero;
            trailRendererTransform.rotation = Quaternion.identity;

            var tr = transform;
            var worldPosition = tr.position;
            var worldDirection = tr.forward;
            SetValues(worldPosition, worldDirection);
        }

        #endregion

        #region SetValues

        private Vector3 _previousPosition = Vector3.zero;
        private float _speed;

        private void SetValues(
            Vector3 position,
            Vector3 normal
        ) {
            _trailMesh.Update(position, normal);

            var cutTangent = position - _previousPosition;
            _previousPosition = position;

            var deltaTime = Time.deltaTime;
            var travelDistance = cutTangent.magnitude;
            var immediateSpeed = travelDistance / deltaTime - 0.2f;
            _speed = Mathf.Lerp(_speed, immediateSpeed, deltaTime * 2f);

            var alpha = Mathf.Clamp(_speed / 3f, 0f, 1f);
            _materialInstance.color = new Color(1f, 1f, 1f, alpha);
        }

        #endregion
    }
}