using System;
using EasyOffset.Configuration;
using UnityEngine;

namespace EasyOffset {
    public static class Abomination {
        #region Transforms

        public static readonly ReeTransform LeftControllerTransform = new(Vector3.zero, Quaternion.identity);
        public static readonly ReeTransform RightControllerTransform = new(Vector3.zero, Quaternion.identity);
        public static event Action<ReeTransform, ReeTransform> TransformsUpdatedEvent;

        public static void UpdateTransforms(Vector3 leftPosition, Quaternion leftRotation, Vector3 rightPosition, Quaternion rightRotation) {
            if (PluginConfig.PositionalSmoothing > 0f) {
                var tPos = Time.deltaTime * PluginConfig.PositionalSmoothing;
                LeftControllerTransform.Position = Vector3.Lerp(LeftControllerTransform.Position, leftPosition, tPos);
                RightControllerTransform.Position = Vector3.Lerp(RightControllerTransform.Position, rightPosition, tPos);
            } else {
                LeftControllerTransform.Position = leftPosition;
                RightControllerTransform.Position = rightPosition;
            }

            if (PluginConfig.RotationalSmoothing > 0f) {
                var tRot = Time.deltaTime * PluginConfig.RotationalSmoothing;
                LeftControllerTransform.Rotation = Quaternion.Lerp(LeftControllerTransform.Rotation, leftRotation, tRot);
                RightControllerTransform.Rotation = Quaternion.Lerp(RightControllerTransform.Rotation, rightRotation, tRot);
            } else {
                LeftControllerTransform.Rotation = leftRotation;
                RightControllerTransform.Rotation = rightRotation;
            }

            TransformsUpdatedEvent?.Invoke(LeftControllerTransform, RightControllerTransform);
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