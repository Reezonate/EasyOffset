using System;
using EasyOffset.Configuration;
using UnityEngine;
using Zenject;

namespace EasyOffset {
    public abstract class AbstractAdjustmentModeManager : IInitializable, IDisposable {
        #region Constructor

        protected readonly MainSettingsModelSO MainSettingsModel;
        private readonly AdjustmentMode _adjustmentMode;
        private readonly float _positionalSmoothing;
        private readonly float _rotationalSmoothing;

        protected AbstractAdjustmentModeManager(
            MainSettingsModelSO mainSettingsModel,
            AdjustmentMode adjustmentMode,
            float positionalSmoothing,
            float rotationalSmoothing
        ) {
            MainSettingsModel = mainSettingsModel;
            _adjustmentMode = adjustmentMode;
            _positionalSmoothing = positionalSmoothing;
            _rotationalSmoothing = rotationalSmoothing;
        }

        #endregion

        #region Smoothing

        private void StartSmoothing() {
            PluginConfig.EnableSmoothing(MainSettingsModel);
            PluginConfig.PositionalSmoothing = _positionalSmoothing;
            PluginConfig.RotationalSmoothing = _rotationalSmoothing;
        }

        private static void StopSmoothing() {
            PluginConfig.DisableSmoothing();
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
                out var adjustmentHandPos,
                out var adjustmentHandRot,
                out var freeHandPos,
                out var freeHandRot
            );

            OnGrabStarted(adjustmentHand, adjustmentHandPos, adjustmentHandRot, freeHandPos, freeHandRot);
        }

        private void OnTransformsUpdated(Vector3 leftPos, Quaternion leftRot, Vector3 rightPos, Quaternion rightRot) {
            if (!_pressed) return;

            GetHandsTransforms(
                _adjustmentHand,
                out var adjustmentHandPos,
                out var adjustmentHandRot,
                out var freeHandPos,
                out var freeHandRot
            );

            OnGrabUpdated(_adjustmentHand, adjustmentHandPos, adjustmentHandRot, freeHandPos, freeHandRot);
        }

        private void OnAssignedButtonReleased(Hand hand) {
            GetAdjustmentHand(hand, out var adjustmentHand);

            if (!_pressed || _adjustmentHand != adjustmentHand) return;
            StopSmoothing();
            _pressed = false;

            GetHandsTransforms(
                _adjustmentHand,
                out var adjustmentHandPos,
                out var adjustmentHandRot,
                out var freeHandPos,
                out var freeHandRot
            );

            OnGrabFinished(_adjustmentHand, adjustmentHandPos, adjustmentHandRot, freeHandPos, freeHandRot);
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
            Vector3 adjustmentHandPos,
            Quaternion adjustmentHandRot,
            Vector3 freeHandPos,
            Quaternion freeHandRot
        );

        protected abstract void OnGrabUpdated(
            Hand adjustmentHand,
            Vector3 adjustmentHandPos,
            Quaternion adjustmentHandRot,
            Vector3 freeHandPos,
            Quaternion freeHandRot
        );

        protected abstract void OnGrabFinished(
            Hand adjustmentHand,
            Vector3 adjustmentHandPos,
            Quaternion adjustmentHandRot,
            Vector3 freeHandPos,
            Quaternion freeHandRot
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
            out Vector3 adjustmentHandPos,
            out Quaternion adjustmentHandRot,
            out Vector3 freeHandPos,
            out Quaternion freeHandRot
        ) {
            switch (adjustmentHand) {
                case Hand.Left:
                    adjustmentHandPos = Abomination.LeftPosition;
                    adjustmentHandRot = Abomination.LeftRotation;
                    freeHandPos = Abomination.RightPosition;
                    freeHandRot = Abomination.RightRotation;
                    break;
                case Hand.Right:
                    adjustmentHandPos = Abomination.RightPosition;
                    adjustmentHandRot = Abomination.RightRotation;
                    freeHandPos = Abomination.LeftPosition;
                    freeHandRot = Abomination.LeftRotation;
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