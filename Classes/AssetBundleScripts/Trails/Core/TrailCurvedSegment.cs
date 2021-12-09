namespace EasyOffset.AssetBundleScripts {
    public class TrailCurvedSegment {
        private readonly TrailNode _p00;
        private readonly TrailNode _p01;
        private readonly TrailNode _v00;
        private readonly TrailNode _v01;

        private TrailNode _p10;
        private TrailNode _p11;
        private TrailNode _v10;

        public TrailCurvedSegment(
            TrailNode handleNodeA,
            TrailNode handleNodeB,
            TrailNode handleNodeC
        ) {
            _p00 = (handleNodeA + handleNodeB) / 2.0f;
            _p01 = handleNodeB;
            var p02 = (handleNodeB + handleNodeC) / 2.0f;
            _v00 = _p01 - _p00;
            _v01 = p02 - _p01;
        }

        public TrailNode Evaluate(float t) {
            _p10 = _p00 + _v00 * t;
            _p11 = _p01 + _v01 * t;
            _v10 = _p11 - _p10;
            return _p10 + _v10 * t;
        }
    }
}