using UnityEngine;

namespace EasyOffset {
    public class RelativeRotationTracker {
        #region Initialize & Reset

        private bool _initialized;

        public void Initialize(
            Quaternion rotation
        ) {
            _previousRotation = rotation;
            _initialized = true;
        }

        public void Reset() {
            _initialized = false;
        }

        #endregion

        #region Update

        private Quaternion _previousRotation;

        public bool Update(
            Quaternion rotation,
            Vector3 positiveDirection,
            out float angleDegrees,
            out Vector3 axis
        ) {
            if (!_initialized || rotation == _previousRotation) {
                angleDegrees = 0f;
                axis = Vector3.zero;
                return false;
            }

            var relativeRotation = rotation * Quaternion.Inverse(_previousRotation);
            relativeRotation.ToAngleAxis(out angleDegrees, out axis);
            _previousRotation = rotation;

            if (Vector3.Dot(positiveDirection, axis) < 0) axis = -axis;
            return true;
        }

        #endregion
    }
}