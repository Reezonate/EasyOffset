using System;
using EasyOffset.Configuration;
using UnityEngine;

namespace EasyOffset {
    public static class Abomination {
        #region Transforms

        public static Vector3 LeftPosition = Vector3.zero;
        public static Quaternion LeftRotation = Quaternion.identity;
        public static Vector3 RightPosition = Vector3.zero;
        public static Quaternion RightRotation = Quaternion.identity;
        public static event Action<Vector3, Quaternion, Vector3, Quaternion> TransformsUpdatedEvent;

        public static void UpdateTransforms(Vector3 leftPosition, Quaternion leftRotation, Vector3 rightPosition, Quaternion rightRotation) {
            if (PluginConfig.SmoothingEnabled) {
                var tPos = Time.deltaTime * PluginConfig.PositionalSmoothing;
                var tRot = Time.deltaTime * PluginConfig.RotationalSmoothing;
                LeftPosition = Vector3.Lerp(LeftPosition, leftPosition, tPos);
                LeftRotation = Quaternion.Lerp(LeftRotation, leftRotation, tRot);
                RightPosition = Vector3.Lerp(RightPosition, rightPosition, tPos);
                RightRotation = Quaternion.Lerp(RightRotation, rightRotation, tRot);
            } else {
                LeftPosition = leftPosition;
                LeftRotation = leftRotation;
                RightPosition = rightPosition;
                RightRotation = rightRotation;
            }

            TransformsUpdatedEvent?.Invoke(LeftPosition, LeftRotation, RightPosition, RightRotation);
        }

        #endregion

        #region AssignedButtonEvents

        public static event Action<Hand> AssignedButtonPressedEvent;
        public static event Action<Hand> AssignedButtonReleasedEvent;

        public static void OnAssignedButtonPressed(Hand hand) {
            AssignedButtonPressedEvent?.Invoke(hand);
        }

        public static void OnAssignedButtonReleased(Hand hand) {
            AssignedButtonReleasedEvent?.Invoke(hand);
        }

        #endregion
    }
}