using System;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset {
    public class ControllerModel : MonoBehaviour {
        #region Serialized

        [SerializeField]
        private GameObject oculusCV1Left;

        [SerializeField]
        private GameObject oculusCV1Right;

        [SerializeField]
        private GameObject oculusQuest2Left;

        [SerializeField]
        private GameObject oculusQuest2Right;

        [SerializeField]
        private GameObject riftSLeft;

        [SerializeField]
        private GameObject riftSRight;

        [SerializeField]
        private GameObject valveIndexLeft;

        [SerializeField]
        private GameObject valveIndexRight;

        [SerializeField]
        private GameObject pico4Left;

        [SerializeField]
        private GameObject pico4Right;

        [SerializeField]
        private GameObject piMaxSwordLeft;

        [SerializeField]
        private GameObject piMaxSwordRight;

        [SerializeField]
        private GameObject viveTracker2;

        [SerializeField]
        private GameObject viveTracker3;

        [SerializeField]
        private GameObject tundraTracker;

        [SerializeField]
        private GameObject vive;

        #endregion

        #region Properties

        private ControllerType _controllerType = ControllerType.None;

        public ControllerType ControllerType {
            get => _controllerType;
            set {
                if (_controllerType == value) return;
                _controllerType = value;
                UpdateModel();
            }
        }

        private Hand _hand;

        public Hand Hand {
            get => _hand;
            set {
                if (_hand == value) return;
                _hand = value;
                UpdateModel();
            }
        }

        private void UpdateModel() {
            var prefab = GetPrefab(_controllerType, _hand is Hand.Left);

            if (prefab != null) {
                SetPrefab(prefab);
            } else {
                DestroyInstance();
            }
        }

        #endregion

        #region SetVisible

        public void SetVisible(bool value) {
            gameObject.SetActive(value);
        }

        #endregion

        #region Instance

        private GameObject _instance;
        private bool _spawned;

        private void SetPrefab(GameObject prefab) {
            DestroyInstance();
            _instance = Instantiate(prefab, transform, false);
            _spawned = true;
        }

        private void DestroyInstance() {
            if (!_spawned) return;
            Destroy(_instance);
            _spawned = false;
        }

        #endregion

        #region Utils

        [CanBeNull]
        private GameObject GetPrefab(ControllerType controllerType, bool isLeft) {
            switch (controllerType) {
                case ControllerType.None: return null;
                case ControllerType.ValveIndex: return isLeft ? valveIndexLeft : valveIndexRight;
                case ControllerType.OculusQuest2: return isLeft ? oculusQuest2Left : oculusQuest2Right;
                case ControllerType.OculusRiftS: return isLeft ? riftSLeft : riftSRight;
                case ControllerType.OculusCV1: return isLeft ? oculusCV1Left : oculusCV1Right;
                case ControllerType.HtcVive: return vive;
                case ControllerType.Pico4: return isLeft ? pico4Left : pico4Right;
                case ControllerType.PiMaxSword: return isLeft ? piMaxSwordLeft : piMaxSwordRight;
                case ControllerType.ViveTracker2: return viveTracker2;
                case ControllerType.ViveTracker3: return viveTracker3;
                case ControllerType.TundraTracker: return tundraTracker;
                default: throw new ArgumentOutOfRangeException(nameof(controllerType), controllerType, null);
            }
        }

        #endregion
    }
}