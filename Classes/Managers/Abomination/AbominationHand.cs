using System;
using EasyOffset.Configuration;
using UnityEngine;

namespace EasyOffset {
    public class AbominationHand {
        #region Transform

        public Vector3 Position = Vector3.zero;
        public Quaternion Rotation = Quaternion.identity;

        public event Action<Vector3, Quaternion> TransformUpdatedEvent;

        public void UpdateTransform(Vector3 position, Quaternion rotation) {
            if (PluginConfig.SmoothingEnabled) {
                Position = Vector3.Lerp(Position, position, Time.deltaTime * PluginConfig.SmoothingSpeed);
                Rotation = Quaternion.Lerp(Rotation, rotation, Time.deltaTime * PluginConfig.SmoothingSpeed);
            } else {
                Position = position;
                Rotation = rotation;
            }

            TransformUpdatedEvent?.Invoke(Position, Rotation);
        }

        #endregion

        #region GripButton

        public event Action GripButtonPressedEvent;
        public event Action GripButtonReleasedEvent;

        public void OnGripButtonPressed() {
            GripButtonPressedEvent?.Invoke();
        }

        public void OnGripButtonReleased() {
            GripButtonReleasedEvent?.Invoke();
        }

        #endregion
    }
}