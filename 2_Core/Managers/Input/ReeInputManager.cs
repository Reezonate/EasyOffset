using System;
using JetBrains.Annotations;
using UnityEngine.EventSystems;
using UnityEngine.XR;
using Zenject;

namespace EasyOffset {
    [UsedImplicitly]
    public class ReeInputManager : IInitializable, IDisposable, ITickable {
        #region Pointer Events Static

        public static event Action<ReeTriggerState> PointerDownAction;
        public static event Action<ReeTriggerState> PointerUpAction;

        private static event Action<PointerEventData> PointerDownActionPrivate;
        private static event Action<PointerEventData> PointerUpActionPrivate;

        public static void NotifyPointerDown(PointerEventData eventData) {
            PointerDownActionPrivate?.Invoke(eventData);
        }

        public static void NotifyPointerUp(PointerEventData eventData) {
            PointerUpActionPrivate?.Invoke(eventData);
        }

        #endregion

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
            _leftReeInputDevice.Update();
            _rightReeInputDevice.Update();
            UpdateTriggerState();
        }

        #endregion

        #region Trigger State

        private ReeTriggerState _triggerState = ReeTriggerState.Released;

        private void UpdateTriggerState() {
            UpdateTriggers(out var leftWasPressed, out var leftWasReleased, out var rightWasPressed, out var rightWasReleased);

            switch (_triggerState) {
                case ReeTriggerState.Released: {
                    if (rightWasPressed) {
                        _triggerState = ReeTriggerState.RightPressed;
                    } else if (leftWasPressed) {
                        _triggerState = ReeTriggerState.LeftPressed;
                    }

                    break;
                }
                case ReeTriggerState.LeftPressed: {
                    if (rightWasPressed) {
                        _triggerState = ReeTriggerState.RightPressed;
                    } else if (leftWasReleased) {
                        _triggerState = ReeTriggerState.Released;
                    }

                    break;
                }
                case ReeTriggerState.RightPressed: {
                    if (leftWasPressed) {
                        _triggerState = ReeTriggerState.LeftPressed;
                    } else if (rightWasReleased) {
                        _triggerState = ReeTriggerState.Released;
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

        #region Pointer Events Instance

        public void Initialize() {
            PointerDownActionPrivate += OnPointerDown;
            PointerUpActionPrivate += OnPointerUp;
        }

        public void Dispose() {
            PointerDownActionPrivate -= OnPointerDown;
            PointerUpActionPrivate -= OnPointerUp;
        }

        private void OnPointerDown(PointerEventData eventData) {
            PointerDownAction?.Invoke(_triggerState);
        }

        private void OnPointerUp(PointerEventData eventData) {
            PointerUpAction?.Invoke(_triggerState);
        }

        #endregion
    }
}