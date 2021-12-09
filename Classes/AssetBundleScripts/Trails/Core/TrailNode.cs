using UnityEngine;

namespace EasyOffset.AssetBundleScripts {
    public readonly struct TrailNode {
        public readonly Vector3 LeftPosition;
        public readonly Vector3 RightPosition;
        private readonly Vector3 _amplitude;

        public TrailNode(
            Vector3 leftPosition,
            Vector3 rightPosition
        ) {
            LeftPosition = leftPosition;
            RightPosition = rightPosition;

            _amplitude = RightPosition - LeftPosition;
        }

        public Vector3 LerpHorizontal(float t) {
            return new Vector3(
                LeftPosition.x + _amplitude.x * t,
                LeftPosition.y + _amplitude.y * t,
                LeftPosition.z + _amplitude.z * t
            );
        }

        #region Math

        public static TrailNode operator +(TrailNode a, TrailNode b) {
            return new TrailNode(
                a.LeftPosition + b.LeftPosition,
                a.RightPosition + b.RightPosition
            );
        }

        public static TrailNode operator -(TrailNode a, TrailNode b) {
            return new TrailNode(
                a.LeftPosition - b.LeftPosition,
                a.RightPosition - b.RightPosition
            );
        }

        public static TrailNode operator /(TrailNode a, float number) {
            return new TrailNode(
                a.LeftPosition / number,
                a.RightPosition / number
            );
        }

        public static TrailNode operator *(TrailNode a, float number) {
            return new TrailNode(
                a.LeftPosition * number,
                a.RightPosition * number
            );
        }

        #endregion
    }
}