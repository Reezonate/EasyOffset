using TMPro;
using UnityEngine;

namespace EasyOffset.SyncedWithUnity {
    public class OrthonormalBasis : MonoBehaviour {
        #region Serialized

        [SerializeField] private Transform pointerTransform;
        [SerializeField] private Material pointerMaterial;
        [SerializeField] private MeshRenderer pointerMeshRenderer;
        [SerializeField] private GameObject visuals;
        [SerializeField] private TextMeshPro textMesh;
        [SerializeField] private Transform textRoot;

        #endregion

        #region ShaderProperties

        private static readonly int ScalePropertyId = Shader.PropertyToID("_Scale");

        #endregion

        #region Start

        private Material _materialInstance;
        private bool _isReady;

        private void Start() {
            _materialInstance = Instantiate(pointerMaterial);
            pointerMeshRenderer.material = _materialInstance;
            _isReady = true;
        }

        #endregion

        #region Interaction

        public void SetTextLookAt(Vector3 worldPosition) {
            UpdateTextLookAt(worldPosition);
        }

        public void SetCoordinates(
            Vector3 coordinates
        ) {
            if (!_isReady) return;
            _materialInstance.SetVector(ScalePropertyId, coordinates);
            pointerTransform.localScale = coordinates;
            
            UpdateTextString(coordinates);
            UpdateTextPosition(coordinates);
        }

        public void SetVisible(bool value) {
            visuals.SetActive(value);
        }

        #endregion

        #region Text

        private void UpdateTextString(Vector3 coordinates) {
            var xString = (coordinates.x * 100.0).ToString("0.0");
            var yString = (coordinates.y * 100.0).ToString("0.0");
            var zString = (coordinates.z * 100.0).ToString("0.0");

            textMesh.text = $"<mspace=0.43em><color=red>{xString}</color>  <color=green>{yString}</color>  <color=blue>{zString}</color></mspace>";
        }

        private void UpdateTextPosition(Vector3 coordinates) {
            textRoot.localPosition = coordinates;
        }

        private void UpdateTextLookAt(Vector3 lookAt) {
            textMesh.transform.LookAt(lookAt, Vector3.up);
        }

        #endregion
    }
}