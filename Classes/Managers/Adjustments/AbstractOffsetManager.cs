using System;
using EasyOffset.Configuration;
using UnityEngine;
using Zenject;

namespace EasyOffset {
    public abstract class AbstractOffsetManager : IInitializable, IDisposable {
        #region Constructor

        protected readonly MainSettingsModelSO MainSettingsModel;
        private readonly AdjustmentMode _adjustmentMode;
        private readonly bool _useSmoothing;

        protected AbstractOffsetManager(
            MainSettingsModelSO mainSettingsModel,
            AdjustmentMode adjustmentMode,
            bool useSmoothing
        ) {
            MainSettingsModel = mainSettingsModel;
            _adjustmentMode = adjustmentMode;
            _useSmoothing = useSmoothing;
        }

        #endregion

        #region Logic

        private bool _pressed;
        private Hand _pressedHand;

        private void OnAssignedButtonPressed(Hand hand) {
            if (_pressed || PluginConfig.AdjustmentMode != _adjustmentMode) return;
            if (_useSmoothing) PluginConfig.EnableSmoothing(MainSettingsModel);
            MirrorHandIfNecessary(ref hand);

            _pressedHand = hand;
            _pressed = true;

            GetHandTransforms(hand, out var position, out var rotation);
            OnGrabStarted(hand, position, rotation);
        }

        private void OnTransformsUpdated(Vector3 leftPos, Quaternion leftRot, Vector3 rightPos, Quaternion rightRot) {
            if (!_pressed) return;
            GetHandTransforms(_pressedHand, out var position, out var rotation);
            OnGrabUpdated(_pressedHand, position, rotation);
        }

        private void OnAssignedButtonReleased(Hand hand) {
            MirrorHandIfNecessary(ref hand);

            if (!_pressed || _pressedHand != hand) return;
            if (_useSmoothing) PluginConfig.DisableSmoothing();
            _pressed = false;

            GetHandTransforms(hand, out var position, out var rotation);
            OnGrabFinished(hand, position, rotation);
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

        protected abstract void OnGrabStarted(Hand hand, Vector3 controllerPosition, Quaternion controllerRotation);

        protected abstract void OnGrabUpdated(Hand hand, Vector3 controllerPosition, Quaternion controllerRotation);

        protected abstract void OnGrabFinished(Hand hand, Vector3 controllerPosition, Quaternion controllerRotation);

        #endregion

        #region Utils

        private static void MirrorHandIfNecessary(ref Hand hand) {
            if (!PluginConfig.UseFreeHand) return;

            hand = hand switch {
                Hand.Left => Hand.Right,
                Hand.Right => Hand.Left,
                _ => throw new ArgumentOutOfRangeException(nameof(hand), hand, null)
            };
        }

        private static void GetHandTransforms(Hand hand, out Vector3 position, out Quaternion rotation) {
            switch (hand) {
                case Hand.Left:
                    position = Abomination.LeftPosition;
                    rotation = Abomination.LeftRotation;
                    break;
                case Hand.Right:
                    position = Abomination.RightPosition;
                    rotation = Abomination.RightRotation;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(hand), hand, null);
            }
        }

        #endregion

        #region Subscription

        public void Initialize() {
            Subscribe();
        }

        public void Dispose() {
            UnSubscribe();
            ForceRelease();
        }

        private void Subscribe() {
            Abomination.AssignedButtonPressedEvent += OnAssignedButtonPressed;
            Abomination.AssignedButtonReleasedEvent += OnAssignedButtonReleased;
            Abomination.TransformsUpdatedEvent += OnTransformsUpdated;

            PluginConfig.AdjustmentModeChangedEvent += OnAdjustmentModeChanged;
        }

        private void UnSubscribe() {
            Abomination.AssignedButtonPressedEvent -= OnAssignedButtonPressed;
            Abomination.AssignedButtonReleasedEvent -= OnAssignedButtonReleased;
            Abomination.TransformsUpdatedEvent -= OnTransformsUpdated;

            PluginConfig.AdjustmentModeChangedEvent -= OnAdjustmentModeChanged;
        }

        #endregion
    }
}