using System;
using Zenject;

namespace EasyOffset {
    public abstract class AbstractAdjustmentModeManager : IInitializable, IDisposable {
        #region Constructor

        private readonly AdjustmentMode _adjustmentMode;
        private readonly float _positionalSmoothing;
        private readonly float _rotationalSmoothing;

        protected AbstractAdjustmentModeManager(
            AdjustmentMode adjustmentMode,
            float positionalSmoothing,
            float rotationalSmoothing
        ) {
            _adjustmentMode = adjustmentMode;
            _positionalSmoothing = positionalSmoothing;
            _rotationalSmoothing = rotationalSmoothing;
        }

        #endregion

        #region Smoothing

        private void StartSmoothing() {
            PluginConfig.SmoothingEnabled = true;
            PluginConfig.PositionalSmoothing = _positionalSmoothing;
            PluginConfig.RotationalSmoothing = _rotationalSmoothing;
        }

        private static void StopSmoothing() {
            PluginConfig.SmoothingEnabled = false;
            PluginConfig.PositionalSmoothing = 0f;
            PluginConfig.RotationalSmoothing = 0f;
        }

        #endregion

        #region Logic

        private bool _pressed;
        private Hand _adjustmentHand;

        private void OnAssignedButtonPressed(Hand hand) {
            if (_pressed || PluginConfig.AdjustmentMode != _adjustmentMode) return;

            StartSmoothing();
            GetAdjustmentHand(hand, out var adjustmentHand);

            _adjustmentHand = adjustmentHand;
            _pressed = true;

            GetHandsTransforms(
                adjustmentHand,
                out var adjustmentHandTransform,
                out var freeHandTransform
            );

            OnGrabStarted(adjustmentHand, adjustmentHandTransform, freeHandTransform);
        }

        private void OnTransformsUpdated(ReeTransform leftTransform, ReeTransform rightTransform) {
            if (!_pressed) return;

            GetHandsTransforms(
                _adjustmentHand,
                out var adjustmentHandTransform,
                out var freeHandTransform
            );

            OnGrabUpdated(_adjustmentHand, adjustmentHandTransform, freeHandTransform);
        }

        private void OnAssignedButtonReleased(Hand hand) {
            GetAdjustmentHand(hand, out var adjustmentHand);

            if (!_pressed || _adjustmentHand != adjustmentHand) return;
            StopSmoothing();
            _pressed = false;

            GetHandsTransforms(
                _adjustmentHand,
                out var adjustmentHandTransform,
                out var freeHandTransform
            );

            OnGrabFinished(_adjustmentHand, adjustmentHandTransform, freeHandTransform);
            PluginConfig.SaveConfigFile();
        }

        private void OnAdjustmentModeChanged(AdjustmentMode value) {
            if (value == _adjustmentMode) return;
            ForceRelease();
        }

        private void ForceRelease() {
            OnAssignedButtonReleased(Hand.Left);
            OnAssignedButtonReleased(Hand.Right);
        }

        #endregion

        #region Abstract

        protected abstract void OnGrabStarted(
            Hand adjustmentHand,
            ReeTransform adjustmentHandTransform,
            ReeTransform freeHandTransform
        );

        protected abstract void OnGrabUpdated(
            Hand adjustmentHand,
            ReeTransform adjustmentHandTransform,
            ReeTransform freeHandTransform
        );

        protected abstract void OnGrabFinished(
            Hand adjustmentHand,
            ReeTransform adjustmentHandTransform,
            ReeTransform freeHandTransform
        );

        #endregion

        #region Utils

        private static void GetAdjustmentHand(Hand buttonPressedHand, out Hand adjustmentHand) {
            adjustmentHand = buttonPressedHand switch {
                Hand.Left => PluginConfig.UseFreeHand ? Hand.Right : Hand.Left,
                Hand.Right => PluginConfig.UseFreeHand ? Hand.Left : Hand.Right,
                _ => throw new ArgumentOutOfRangeException(nameof(buttonPressedHand), buttonPressedHand, null)
            };
        }

        private static void GetHandsTransforms(
            Hand adjustmentHand,
            out ReeTransform adjustmentHandTransform,
            out ReeTransform freeHandTransform
        ) {
            switch (adjustmentHand) {
                case Hand.Left:
                    adjustmentHandTransform = Abomination.LeftControllerTransform;
                    freeHandTransform = Abomination.RightControllerTransform;
                    break;
                case Hand.Right:
                    adjustmentHandTransform = Abomination.RightControllerTransform;
                    freeHandTransform = Abomination.LeftControllerTransform;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(adjustmentHand), adjustmentHand, null);
            }
        }

        #endregion

        #region Subscription

        public void Initialize() {
            Abomination.AssignedButtonPressedEvent += OnAssignedButtonPressed;
            Abomination.AssignedButtonReleasedEvent += OnAssignedButtonReleased;
            Abomination.TransformsUpdatedEvent += OnTransformsUpdated;
            PluginConfig.AdjustmentModeChangedEvent += OnAdjustmentModeChanged;
        }

        public void Dispose() {
            ForceRelease();

            Abomination.AssignedButtonPressedEvent -= OnAssignedButtonPressed;
            Abomination.AssignedButtonReleasedEvent -= OnAssignedButtonReleased;
            Abomination.TransformsUpdatedEvent -= OnTransformsUpdated;
            PluginConfig.AdjustmentModeChangedEvent -= OnAdjustmentModeChanged;
        }

        #endregion
    }
}