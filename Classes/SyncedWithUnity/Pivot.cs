using UnityEngine;

namespace EasyOffset.SyncedWithUnity {
    public class Pivot : MonoBehaviour {
        [SerializeField] private MeshRenderer pointOutlineMeshRenderer;
        [SerializeField] private MeshRenderer pointCenterMeshRenderer;
        [SerializeField] private Transform pointTransform;
        [SerializeField] private PivotGrid grid;

        public void SetVisible(bool value) {
            pointOutlineMeshRenderer.enabled = value;
            pointCenterMeshRenderer.enabled = value;
            grid.SetVisible(value);
        }

        public void SetValues(
            Vector3 pointPosition
        ) {
            pointTransform.position = pointPosition;
        }
    }
}