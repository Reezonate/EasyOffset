using System;
using UnityEngine;

namespace EasyOffset.SyncedWithUnity {
    public class ControllerModel : MonoBehaviour {
        #region Serialized

        [SerializeField] private GameObject oculusCV1Left;
        [SerializeField] private GameObject oculusCV1Right;

        [SerializeField] private GameObject oculusQuest2Left;
        [SerializeField] private GameObject oculusQuest2Right;

        [SerializeField] private GameObject riftSLeft;
        [SerializeField] private GameObject riftSRight;

        [SerializeField] private GameObject valveIndexLeft;
        [SerializeField] private GameObject valveIndexRight;

        [SerializeField] private GameObject viveTracker2;
        [SerializeField] private GameObject viveTracker3;
        [SerializeField] private GameObject vive;

        [SerializeField] private GameObject visuals;

        #endregion

        #region Instance

        private ControllerType _currentType = ControllerType.None;
        private GameObject _instance;
        private bool _spawned;

        private void SetPrefab(
            GameObject prefab
        ) {
            DestroyInstance();
            _instance = Instantiate(prefab, visuals.transform, false);
            _spawned = true;
        }

        private void DestroyInstance() {
            if (!_spawned) return;
            Destroy(_instance);
            _spawned = false;
        }

        #endregion

        #region Interaction

        public void SetControllerType(
            ControllerType controllerType,
            Hand hand
        ) {
            if (controllerType == _currentType) return;
            _currentType = controllerType;

            bool notNull;
            GameObject prefab;

            switch (hand) {
                case Hand.Left:
                    notNull = GetLeftPrefab(controllerType, out prefab);
                    break;
                case Hand.Right:
                    notNull = GetRightPrefab(controllerType, out prefab);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(hand), hand, null);
            }

            if (notNull) {
                SetPrefab(prefab);
            } else {
                DestroyInstance();
            }
        }

        public void SetVisible(bool value) {
            visuals.SetActive(value);
        }

        #endregion

        #region Utils

        private bool GetLeftPrefab(ControllerType controllerType, out GameObject prefab) {
            switch (controllerType) {
                case ControllerType.None:
                    prefab = vive;
                    return false;
                case ControllerType.ValveIndex:
                    prefab = valveIndexLeft;
                    break;
                case ControllerType.OculusQuest2:
                    prefab = oculusQuest2Left;
                    break;
                case ControllerType.OculusRiftS:
                    prefab = riftSLeft;
                    break;
                case ControllerType.OculusCV1:
                    prefab = oculusCV1Left;
                    break;
                case ControllerType.HtcVive:
                    prefab = vive;
                    break;
                case ControllerType.ViveTracker2:
                    prefab = viveTracker2;
                    break;
                case ControllerType.ViveTracker3:
                    prefab = viveTracker3;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(controllerType), controllerType, null);
            }

            return true;
        }

        private bool GetRightPrefab(ControllerType controllerType, out GameObject prefab) {
            switch (controllerType) {
                case ControllerType.None:
                    prefab = vive;
                    return false;
                case ControllerType.ValveIndex:
                    prefab = valveIndexRight;
                    break;
                case ControllerType.OculusQuest2:
                    prefab = oculusQuest2Right;
                    break;
                case ControllerType.OculusRiftS:
                    prefab = riftSRight;
                    break;
                case ControllerType.OculusCV1:
                    prefab = oculusCV1Right;
                    break;
                case ControllerType.HtcVive:
                    prefab = vive;
                    break;
                case ControllerType.ViveTracker2:
                    prefab = viveTracker2;
                    break;
                case ControllerType.ViveTracker3:
                    prefab = viveTracker3;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(controllerType), controllerType, null);
            }

            return true;
        }

        #endregion
    }
}