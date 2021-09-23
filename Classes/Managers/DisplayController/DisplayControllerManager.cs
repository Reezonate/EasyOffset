using System;
using EasyOffset.Configuration;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace EasyOffset {
    [UsedImplicitly]
    public class DisplayControllerManager : IInitializable, IDisposable {
        #region Inject

        private readonly MainSettingsModelSO _mainSettings;

        protected DisplayControllerManager(
            MainSettingsModelSO mainSettings
        ) {
            _mainSettings = mainSettings;
        }

        #endregion

        #region Init/Dispose

        public void Initialize() {
            Abomination.TransformsUpdatedEvent += UpdateHands;
            PluginConfig.ControllerTypeChangedEvent += SetControllerType;
            SetControllerType(PluginConfig.DisplayControllerType);
        }

        public void Dispose() {
            Abomination.TransformsUpdatedEvent -= UpdateHands;
            PluginConfig.ControllerTypeChangedEvent -= SetControllerType;
            DestroyObjects();
        }

        #endregion

        #region SetControllerType

        private ControllerType _controllerType = ControllerType.None;
        private GameObject _rightControllerObject;
        private GameObject _leftControllerObject;

        private bool _instantiated;

        private void SetControllerType(ControllerType controllerType) {
            if (controllerType == _controllerType) return;

            DestroyObjects();
            
            if (controllerType != ControllerType.None) {
                _rightControllerObject = Object.Instantiate(ControllerTypeUtils.GetRightControllerPrefab(controllerType));
                _leftControllerObject = Object.Instantiate(ControllerTypeUtils.GetLeftControllerPrefab(controllerType));
                Object.DontDestroyOnLoad(_rightControllerObject);
                Object.DontDestroyOnLoad(_leftControllerObject);
                _instantiated = true;
            }

            _controllerType = controllerType;
        }

        private void DestroyObjects() {
            if (!_instantiated) return;
            Object.Destroy(_leftControllerObject);
            Object.Destroy(_rightControllerObject);
            _instantiated = false;
        }

        #endregion

        #region Update

        private void UpdateHands(Vector3 leftPosition, Quaternion leftRotation, Vector3 rightPosition, Quaternion rightRotation) {
            if (_controllerType == ControllerType.None) return;
            UpdateControllerTransform(leftPosition, leftRotation, _leftControllerObject.transform);
            UpdateControllerTransform(rightPosition, rightRotation, _rightControllerObject.transform);
        }

        private void UpdateControllerTransform(Vector3 rawPosition, Quaternion rawRotation, Transform controllerObjectTransform) {
            TransformUtils.ApplyRoomOffset(_mainSettings, ref rawPosition, ref rawRotation);
            controllerObjectTransform.position = rawPosition;
            controllerObjectTransform.rotation = rawRotation;
        }

        #endregion
    }
}