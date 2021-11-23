using UnityEngine;
using UnityEngine.Rendering;

namespace EasyOffset.AssetBundleScripts {
    public class RainbowTrail : MonoBehaviour {
        #region Serialized

        [SerializeField] private Material material;
        [SerializeField] private int lengthResolution = 100;
        [SerializeField] private int widthResolution = 2;
        [SerializeField] private int lifetime = 30;
        [SerializeField] private float halfWidth = 0.01f;
        [SerializeField] private bool isTracking = true;

        #endregion

        #region Start & Enable

        private readonly Matrix4x4 _meshTransformMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);

        private RainbowTrailMesh _mesh;
        private Material _materialInstance;

        private void Start() {
            ResetMesh();
            _materialInstance = Instantiate(material);
        }

        private void OnEnable() {
            ResetMesh();
        }

        private void ResetMesh() {
            _mesh = new RainbowTrailMesh(lifetime, widthResolution, lengthResolution);
        }

        #endregion

        #region LookAt

        private Vector3 _lookAt;
        private bool _hasLookAt;

        public void SetLookAt(Vector3 worldPosition) {
            _lookAt = worldPosition;
            _hasLookAt = true;
        }

        #endregion

        #region Tracking

        public void StartTracking() {
            isTracking = true;
            ResetMesh();
        }

        public void StopTracking() {
            isTracking = false;
        }

        #endregion

        #region LateUpdate

        private void LateUpdate() {
            UpdateMesh();
            DrawMesh();
        }

        #endregion

        #region UpdateMesh

        private Vector3 _previousPosition = Vector3.zero;
        private bool _hasPreviousPosition;

        private void UpdateMesh() {
            if (!isTracking) return;

            var position = transform.position;
            if (!GetVelocity(position, out var velocity)) return;

            var normal = _hasLookAt ? _lookAt - position : transform.forward;
            var widthOffsetDirection = Quaternion.AngleAxis(90f, normal) * velocity.normalized;

            _mesh.AddNode(new RainbowTrailNode(
                position - widthOffsetDirection * halfWidth,
                position + widthOffsetDirection * halfWidth
            ));
        }

        private bool GetVelocity(Vector3 position, out Vector3 velocity) {
            if (!_hasPreviousPosition) {
                _previousPosition = position;
                _hasPreviousPosition = true;
                velocity = Vector3.zero;
                return false;
            }

            velocity = position - _previousPosition;
            _previousPosition = position;
            return true;
        }

        #endregion

        #region DrawMesh

        private void DrawMesh() {
            Graphics.DrawMesh(
                _mesh.Mesh,
                _meshTransformMatrix,
                _materialInstance,
                gameObject.layer,
                null,
                0,
                null,
                ShadowCastingMode.Off,
                false,
                null,
                LightProbeUsage.Off
            );
        }

        #endregion
    }
}