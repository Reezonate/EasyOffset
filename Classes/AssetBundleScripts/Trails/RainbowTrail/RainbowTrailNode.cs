using UnityEngine;

namespace EasyOffset.AssetBundleScripts {
    public readonly struct RainbowTrailNode : ITrailNode {
        public readonly Vector3 LeftPosition;
        public readonly Vector3 RightPosition;
        private readonly Vector3 _amplitude;

        public RainbowTrailNode(
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

        #region Implementation

        public ITrailNode Plus(ITrailNode other) {
            var tmp = (RainbowTrailNode) other;
            return new RainbowTrailNode(
                LeftPosition + tmp.LeftPosition,
                RightPosition + tmp.RightPosition
            );
        }

        public ITrailNode Minus(ITrailNode other) {
            var tmp = (RainbowTrailNode) other;
            return new RainbowTrailNode(
                LeftPosition - tmp.LeftPosition,
                RightPosition - tmp.RightPosition
            );
        }

        public ITrailNode Div(float number) {
            return new RainbowTrailNode(
                LeftPosition / number,
                RightPosition / number
            );
        }

        public ITrailNode Times(float number) {
            return new RainbowTrailNode(
                LeftPosition * number,
                RightPosition * number
            );
        }

        #endregion
    }
}