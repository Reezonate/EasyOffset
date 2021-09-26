using UnityEngine;

namespace EasyOffset.SyncedWithUnity {
    public class TextTransformFixer : MonoBehaviour {
        [SerializeField] private Vector3 worldOffset;
        [SerializeField] private Transform target;

        private void Update() {
            transform.position = target.position + worldOffset;
        }
    }
}