using System;
using System.Collections.Generic;
using UnityEngine.XR;

namespace EasyOffset {
    public class ReeInputDevice {
        private readonly XRNode _xrNode;

        public event Action<ControllerButton> OnButtonPressed;
        public event Action<ControllerButton> OnButtonReleased;

        public ReeInputDevice(XRNode xrNode) {
            _xrNode = xrNode;
        }

        #region Update

        private bool _triggerButtonFlag;
        private bool _gripButtonFlag;
        private bool _primaryButtonFlag;
        private bool _secondaryButtonFlag;
        private bool _primary2dAxisFlag;

        public void Update() {
            PollDevice();
            UpdateButtonState(CommonUsages.triggerButton, ref _triggerButtonFlag, ControllerButton.TriggerButton);
            UpdateButtonState(CommonUsages.primaryButton, ref _primaryButtonFlag, ControllerButton.PrimaryButton);
            UpdateButtonState(CommonUsages.secondaryButton, ref _secondaryButtonFlag, ControllerButton.SecondaryButton);
            UpdateButtonState(CommonUsages.gripButton, ref _gripButtonFlag, ControllerButton.GripButton);
            UpdateButtonState(CommonUsages.primary2DAxisClick, ref _primary2dAxisFlag, ControllerButton.Primary2DAxisClick);
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
                if (!_inputDevice.isValid) _initialized = false;
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