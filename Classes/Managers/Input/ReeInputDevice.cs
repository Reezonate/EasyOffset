using System;
using System.Collections.Generic;
using UnityEngine.XR;

namespace EasyOffset {
    public class ReeInputDevice {
        private readonly XRNode _xrNode;

        public event Action<ControllerButton> OnButtonPressed;
        public event Action<ControllerButton> OnButtonReleased;

        public ReeInputDevice(
            XRNode xrNode
        ) {
            _xrNode = xrNode;
        }

        #region Update

        private bool _gripFlag;
        private bool _primaryFlag;
        private bool _joystickFlag;

        public void Update() {
            PollDevice();
            UpdateButtonState(CommonUsages.gripButton, ref _gripFlag, ControllerButton.Grip);
            UpdateButtonState(CommonUsages.primaryButton, ref _primaryFlag, ControllerButton.Primary);
            UpdateButtonState(CommonUsages.primary2DAxisClick, ref _joystickFlag, ControllerButton.Joystick);
        }

        #endregion

        #region Poll

        private const int PollRate = 50;

        private InputDevice _inputDevice;
        private bool _initialized;
        private int _timer;

        private void PollDevice() {
            if (_initialized) return;
            if (_timer++ < PollRate) return;

            var devices = new List<InputDevice>();
            InputDevices.GetDevicesAtXRNode(_xrNode, devices);

            if (devices.Count > 0) {
                _inputDevice = devices[0];
                _initialized = true;
            }

            _timer = 0;
        }

        #endregion

        #region UpdateButtonState

        private void UpdateButtonState(InputFeatureUsage<bool> inputFeature, ref bool flag, ControllerButton controllerButton) {
            if (!_initialized) return;

            if (!_inputDevice.TryGetFeatureValue(inputFeature, out var value)) {
                _initialized = false;
                return;
            }

            if (value) {
                if (flag) return;
                OnButtonPressed?.Invoke(controllerButton);
                flag = true;
            } else {
                if (!flag) return;
                OnButtonReleased?.Invoke(controllerButton);
                flag = false;
            }
        }

        #endregion
    }
}