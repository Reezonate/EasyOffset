using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.XR;
using Zenject;

namespace EasyOffset {
    [UsedImplicitly]
    public class ReeInputManager : ITickable {
        private const float GripThreshold = 1.0f;

        public readonly HandDevice LeftHand;
        public readonly HandDevice RightHand;

        public ReeInputManager() {
            LeftHand = new HandDevice(XRNode.LeftHand);
            RightHand = new HandDevice(XRNode.RightHand);
        }

        public void Tick() {
            LeftHand.Update();
            RightHand.Update();
        }

        public class HandDevice {
            private const int PollRate = 50;

            private readonly XRNode _xrNode;
            private InputDevice _inputDevice;
            private bool _initialized;
            private int _timer;

            private bool _gripState;

            public event Action OnGripButtonPressed;
            public event Action OnGripButtonReleased;

            public HandDevice(
                XRNode xrNode
            ) {
                _xrNode = xrNode;
            }

            public void Update() {
                PollDevice();
                UpdateGripButtonState();
            }

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

            private void UpdateGripButtonState() {
                if (!_initialized) return;

                if (!_inputDevice.TryGetFeatureValue(CommonUsages.grip, out var gripValue)) {
                    _initialized = false;
                    return;
                }

                if (gripValue >= GripThreshold) {
                    Plugin.Log.Info($"GripAxisValue: {gripValue}");
                    if (_gripState) return;
                    OnGripButtonPressed?.Invoke();
                    _gripState = true;
                } else {
                    if (!_gripState) return;
                    OnGripButtonReleased?.Invoke();
                    _gripState = false;
                }
            }
        }
    }
}