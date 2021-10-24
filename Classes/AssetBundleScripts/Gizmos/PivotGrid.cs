using UnityEngine;

namespace EasyOffset.AssetBundleScripts {
    public class PivotGrid : MonoBehaviour {
        #region Serialized

        [SerializeField] private Material materialXY;
        [SerializeField] private Material materialXZ;
        [SerializeField] private Material materialYZ;

        [SerializeField] private GameObject prefabXY;
        [SerializeField] private GameObject prefabXZ;
        [SerializeField] private GameObject prefabYZ;

        [SerializeField] private Transform target;

        [SerializeField] private float scale;
        [SerializeField] private int resolution;

        #endregion

        #region ShaderProperties

        private static readonly int CellSizePropertyId = Shader.PropertyToID("_CellSize");
        private static readonly int OriginPropertyId = Shader.PropertyToID("_Origin");

        #endregion

        #region Start

        private Material _materialInstanceXY;
        private Material _materialInstanceXZ;
        private Material _materialInstanceYZ;

        private float _step;

        private void Start() {
            _step = scale / resolution;

            InstantiateMaterials();
            InstantiatePlanes();
        }

        private void InstantiateMaterials() {
            _materialInstanceXY = InstantiateMaterial(materialXY, _step);
            _materialInstanceXZ = InstantiateMaterial(materialXZ, _step);
            _materialInstanceYZ = InstantiateMaterial(materialYZ, _step);
        }

        private void InstantiatePlanes() {
            var root = transform;

            var offset = -scale / 2;
            for (var i = 0; i <= resolution; i++, offset += _step) {
                InstantiatePlane(root, prefabXY, _materialInstanceXY, new Vector3(0, 0, offset));
            }

            offset = -scale / 2;
            for (var i = 0; i <= resolution; i++, offset += _step) {
                InstantiatePlane(root, prefabXZ, _materialInstanceXZ, new Vector3(0, offset, 0));
            }

            offset = -scale / 2;
            for (var i = 0; i <= resolution; i++, offset += _step) {
                InstantiatePlane(root, prefabYZ, _materialInstanceYZ, new Vector3(offset, 0, 0));
            }
        }

        private static Material InstantiateMaterial(Material material, float step) {
            var result = Instantiate(material);
            result.SetFloat(CellSizePropertyId, step);
            return result;
        }

        private static void InstantiatePlane(Transform root, GameObject prefab, Material material, Vector3 localPosition) {
            var planeObject = Instantiate(prefab, root, false);
            planeObject.transform.localPosition = localPosition;
            var meshRenderer = planeObject.GetComponent<MeshRenderer>();
            meshRenderer.material = material;
        }

        #endregion

        #region Update

        private void Update() {
            var targetPosition = target.position;

            transform.rotation = Quaternion.identity;
            transform.position = new Vector3(
                Mathf.RoundToInt(targetPosition.x / _step) * _step,
                Mathf.RoundToInt(targetPosition.y / _step) * _step,
                Mathf.RoundToInt(targetPosition.z / _step) * _step
            );

            _materialInstanceXY.SetVector(OriginPropertyId, targetPosition);
            _materialInstanceXZ.SetVector(OriginPropertyId, targetPosition);
            _materialInstanceYZ.SetVector(OriginPropertyId, targetPosition);
        }

        #endregion
    }
}