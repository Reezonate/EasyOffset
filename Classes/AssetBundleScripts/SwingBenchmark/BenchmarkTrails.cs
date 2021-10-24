using UnityEngine;

namespace EasyOffset.AssetBundleScripts {
    public class BenchmarkTrails : MonoBehaviour {
        #region Serialized

        [SerializeField] private Transform pivotTrailTarget;
        [SerializeField] private Transform saberTransform;
        [SerializeField] private StaticTrail pivotTrail;
        [SerializeField] private StaticTrail tipTrail;
        [SerializeField] private GameObject visuals;

        #endregion

        #region Interaction
        
        public void SetLookAt(Vector3 position) {
            pivotTrailTarget.LookAt(position);
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

        public void SetVisible(bool value) {
            visuals.SetActive(value);
        }

        #endregion
    }
}