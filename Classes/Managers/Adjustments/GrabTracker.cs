using System;
using EasyOffset.Configuration;
using UnityEngine;
using Zenject;

namespace EasyOffset {
    public abstract class GrabTracker : IInitializable, IDisposable {
        #region Constructor

        protected readonly MainSettingsModelSO MainSettingsModel;
        private readonly GripButtonAction _requiredAction;
        private readonly bool _useSmoothing;

        protected GrabTracker(
            MainSettingsModelSO mainSettingsModel,
            GripButtonAction requiredAction,
            bool useSmoothing
        ) {
            MainSettingsModel = mainSettingsModel;
            _requiredAction = requiredAction;
            _useSmoothing = useSmoothing;
        }

        #endregion

        #region Logic

        private bool _pressed;
        private Hand _pressedHand;

        private void OnGripButtonPressed(Hand hand) {
            if (_pressed || PluginConfig.GripButtonAction != _requiredAction) return;
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

        private void OnGripButtonReleased(Hand hand) {
            MirrorHandIfNecessary(ref hand);

            if (!_pressed || _pressedHand != hand) return;
            if (_useSmoothing) PluginConfig.DisableSmoothing();
            _pressed = false;

            GetHandTransforms(hand, out var position, out var rotation);
            OnGrabFinished(hand, position, rotation);
        }

        private void OnGripButtonActionChanged(GripButtonAction action) {
            if (action == _requiredAction) return;
            ForceRelease();
        }

        private void ForceRelease() {
            OnGripButtonReleased(Hand.Left);
            OnGripButtonReleased(Hand.Right);
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
            Abomination.GripButtonPressedEvent += OnGripButtonPressed;
            Abomination.GripButtonReleasedEvent += OnGripButtonReleased;
            Abomination.TransformsUpdatedEvent += OnTransformsUpdated;

            PluginConfig.GripButtonActionChangedEvent += OnGripButtonActionChanged;
        }

        private void UnSubscribe() {
            Abomination.GripButtonPressedEvent -= OnGripButtonPressed;
            Abomination.GripButtonReleasedEvent -= OnGripButtonReleased;
            Abomination.TransformsUpdatedEvent -= OnTransformsUpdated;

            PluginConfig.GripButtonActionChangedEvent -= OnGripButtonActionChanged;
        }

        #endregion
    }
}