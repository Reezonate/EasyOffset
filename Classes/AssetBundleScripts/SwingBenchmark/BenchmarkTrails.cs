using UnityEngine;

namespace EasyOffset.AssetBundleScripts {
    public class BenchmarkTrails : MonoBehaviour {
        #region Serialized

        [SerializeField] private Transform saberTransform;
        [SerializeField] private SimpleTrail pivotTrail;
        [SerializeField] private SimpleTrail tipTrail;

        #endregion

        #region Interaction

        public void SetLookAt(Vector3 position) {
            pivotTrail.SetLookAt(position);
        }

        public void UpdateSaberTransform(Vector3 position, Quaternion rotation) {
            saberTransform.position = position;
            saberTransform.rotation = rotation;
        }

        public void StartTracking() {
            pivotTrail.StartTracking();
            tipTrail.StartTracking();
        }

        public void StopTracking() {
            pivotTrail.StopTracking();
            tipTrail.StopTracking();
        }

        #endregion
    }
}