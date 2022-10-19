using System;
using JetBrains.Annotations;
using UnityEngine.XR;
using Zenject;

namespace EasyOffset {
    [UsedImplicitly]
    public class ReeInputManager : ITickable {
        #region Constructor

        private readonly VRControllersInputManager _vrControllersInputManager;
        private readonly ReeInputDevice _leftReeInputDevice;
        private readonly ReeInputDevice _rightReeInputDevice;

        public ReeInputManager(VRControllersInputManager vrControllersInputManager) {
            _vrControllersInputManager = vrControllersInputManager;
            _leftReeInputDevice = new ReeInputDevice(Hand.Left);
            _rightReeInputDevice = new ReeInputDevice(Hand.Right);
        }

        #endregion

        #region Tick

        public void Tick() {
            if (!PluginConfig.IsModPanelVisible) return;
            _leftReeInputDevice.Update();
            _rightReeInputDevice.Update();
            UpdateTriggerState();
        }

        #endregion

        #region Trigger State

        public static ReeTriggerState TriggerState { get; private set; } = ReeTriggerState.Released;

        private void UpdateTriggerState() {
            UpdateTriggers(out var leftWasPressed, out var leftWasReleased, out var rightWasPressed, out var rightWasReleased);

            switch (TriggerState) {
                case ReeTriggerState.Released:
                {
                    if (rightWasPressed) {
                        TriggerState = ReeTriggerState.RightPressed;
                    } else if (leftWasPressed) {
                        TriggerState = ReeTriggerState.LeftPressed;
                    }

                    break;
                }
                case ReeTriggerState.LeftPressed:
                {
                    if (rightWasPressed) {
                        TriggerState = ReeTriggerState.RightPressed;
                    } else if (leftWasReleased) {
                        TriggerState = ReeTriggerState.Released;
                    }

                    break;
                }
                case ReeTriggerState.RightPressed:
                {
                    if (leftWasPressed) {
                        TriggerState = ReeTriggerState.LeftPressed;
                    } else if (rightWasReleased) {
                        TriggerState = ReeTriggerState.Released;
                    }

                    break;
                }
                default: throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        #region UpdateTriggers

        private const float TriggerThreshold = 0.1f;

        private bool _leftPressed;
        private bool _rightPressed;

        private void UpdateTriggers(out bool leftWasPressed, out bool leftWasReleased, out bool rightWasPressed, out bool rightWasReleased) {
            var isLeftTriggerDown = _vrControllersInputManager.TriggerValue(XRNode.LeftHand) > TriggerThreshold;
            var isRightTriggerDown = _vrControllersInputManager.TriggerValue(XRNode.RightHand) > TriggerThreshold;

            leftWasPressed = !_leftPressed && isLeftTriggerDown;
            leftWasReleased = _leftPressed && !isLeftTriggerDown;

            rightWasPressed = !_rightPressed && isRightTriggerDown;
            rightWasReleased = _rightPressed && !isRightTriggerDown;

            _leftPressed = isLeftTriggerDown;
            _rightPressed = isRightTriggerDown;
        }

        #endregion
    }
}