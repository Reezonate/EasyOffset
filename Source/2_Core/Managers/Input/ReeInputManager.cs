using System;
using JetBrains.Annotations;
using UnityEngine.XR;
using Zenject;

namespace EasyOffset {
    [UsedImplicitly]
    public class ReeInputManager : ITickable {
        #region Constructor

        private readonly IVRPlatformHelper _vrPlatformHelper;
        private readonly ReeInputDevice _leftReeInputDevice;
        private readonly ReeInputDevice _rightReeInputDevice;

        public ReeInputManager(IVRPlatformHelper vrPlatformHelper) {
            _vrPlatformHelper = vrPlatformHelper;
            _leftReeInputDevice = new ReeInputDevice(Hand.Left);
            _rightReeInputDevice = new ReeInputDevice(Hand.Right);
        }

        #endregion

        #region Tick

        public void Tick() {
            if (!PluginConfig.IsModPanelVisible) return;
            _leftReeInputDevice.Update();
            _rightReeInputDevice.Update();
            UpdateTriggers();
        }

        #endregion

        #region UpdateTriggers

        private const float TriggerThreshold = 0.1f;

        private bool _leftTriggerPressed;
        private bool _rightTriggerPressed;

        private void UpdateTriggers() {
            var isLeftTriggerDown = _vrPlatformHelper.GetTriggerValue(XRNode.LeftHand) > TriggerThreshold;
            var isRightTriggerDown = _vrPlatformHelper.GetTriggerValue(XRNode.RightHand) > TriggerThreshold;

            var leftWasPressed = !_leftTriggerPressed && isLeftTriggerDown;
            var leftWasReleased = _leftTriggerPressed && !isLeftTriggerDown;

            var rightWasPressed = !_rightTriggerPressed && isRightTriggerDown;
            var rightWasReleased = _rightTriggerPressed && !isRightTriggerDown;

            _leftTriggerPressed = isLeftTriggerDown;
            _rightTriggerPressed = isRightTriggerDown;

            UpdateTriggerState(leftWasPressed, leftWasReleased, rightWasPressed, rightWasReleased);
        }

        #endregion

        #region Trigger State

        public static ReeTriggerState TriggerState { get; private set; } = ReeTriggerState.Released;

        private static void UpdateTriggerState(bool leftWasPressed, bool leftWasReleased, bool rightWasPressed, bool rightWasReleased) {
            switch (TriggerState) {
                case ReeTriggerState.Released: {
                    if (rightWasPressed) {
                        TriggerState = ReeTriggerState.RightPressed;
                    } else if (leftWasPressed) {
                        TriggerState = ReeTriggerState.LeftPressed;
                    }

                    break;
                }
                case ReeTriggerState.LeftPressed: {
                    if (rightWasPressed) {
                        TriggerState = ReeTriggerState.RightPressed;
                    } else if (leftWasReleased) {
                        TriggerState = ReeTriggerState.Released;
                    }

                    break;
                }
                case ReeTriggerState.RightPressed: {
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
    }
}