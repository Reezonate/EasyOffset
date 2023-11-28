using System;
using System.Collections.Generic;
using UnityEngine.XR;

namespace EasyOffset {
    public class ReeInputDevice {
        #region Constructor

        private readonly XRNode _xrNode;
        private readonly Hand _hand;

        public ReeInputDevice(Hand hand) {
            _hand = hand;
            _xrNode = hand switch {
                Hand.Left => XRNode.LeftHand,
                Hand.Right => XRNode.RightHand,
                _ => throw new ArgumentOutOfRangeException(nameof(hand), hand, null)
            };
        }

        #endregion

        #region Poll

        private const int PollRate = 500;

        private InputDevice _inputDevice;
        private readonly List<FeatureTracker> _buttonFeatures = new();
        private bool _initialized;
        private int _timer;

        private void PollDevice() {
            if (_initialized) return;
            if (_timer++ < PollRate) return;

            var devices = new List<InputDevice>();
            InputDevices.GetDevicesAtXRNode(_xrNode, devices);

            if (devices.Count > 0) {
                _inputDevice = devices[0];
                _buttonFeatures.Clear();

                var features = new List<InputFeatureUsage>();
                _inputDevice.TryGetFeatureUsages(features);

                foreach (var feature in features) {
                    switch (feature.name) {
                        case "TriggerButton ": {
                            if (feature.type != typeof(bool)) continue;
                            _buttonFeatures.Add(new FeatureTracker(feature.As<bool>(), ControllerButton.TriggerButton));
                            break;
                        }
                        case "GripButton": {
                            if (feature.type != typeof(bool)) continue;
                            _buttonFeatures.Add(new FeatureTracker(feature.As<bool>(), ControllerButton.GripButton));
                            break;
                        }
                        case "PrimaryButton":
                        case "MenuButton": {
                            if (feature.type != typeof(bool)) continue;
                            _buttonFeatures.Add(new FeatureTracker(feature.As<bool>(), ControllerButton.PrimaryButton));
                            break;
                        }
                        case "SecondaryButton": {
                            if (feature.type != typeof(bool)) continue;
                            _buttonFeatures.Add(new FeatureTracker(feature.As<bool>(), ControllerButton.SecondaryButton));
                            break;
                        }
                        case "Primary2DAxisClick": {
                            if (feature.type != typeof(bool)) continue;
                            _buttonFeatures.Add(new FeatureTracker(feature.As<bool>(), ControllerButton.Primary2DAxisClick));
                            break;
                        }
                        default: {
                            if (feature.type != typeof(bool)) continue;
                            Plugin.Log.Notice($"Unknown input button feature: - {feature.name}");
                            break;
                        }
                    }
                }

                _initialized = true;
            }

            _timer = 0;
        }

        #endregion

        #region Update

        public void Update() {
            PollDevice();

            foreach (var tracker in _buttonFeatures) {
                if (!_initialized) break;
                if (tracker.UpdateButtonState(_inputDevice, _hand)) continue;
                _initialized = _inputDevice.isValid;
            }
        }

        #endregion

        #region FeatureTracker

        private class FeatureTracker {
            private readonly InputFeatureUsage<bool> _inputFeatureUsage;
            private readonly ControllerButton _controllerButton;
            private bool _flag;

            public FeatureTracker(InputFeatureUsage<bool> inputFeatureUsage, ControllerButton controllerButton) {
                _inputFeatureUsage = inputFeatureUsage;
                _controllerButton = controllerButton;
                _flag = false;
            }

            public bool UpdateButtonState(InputDevice inputDevice, Hand hand) {
                if (!inputDevice.TryGetFeatureValue(_inputFeatureUsage, out var value)) return false;

                if (value) {
                    if (_flag) return true;
                    Abomination.OnButtonPressed(hand, _controllerButton);
                    _flag = true;
                } else {
                    if (!_flag) return true;
                    Abomination.OnButtonReleased(hand, _controllerButton);
                    _flag = false;
                }

                return true;
            }
        }

        #endregion
    }
}