namespace EasyOffset.AssetBundleScripts {
    public class TrailCurvedSegment {
        private readonly ITrailNode _p00;
        private readonly ITrailNode _p01;
        private readonly ITrailNode _v00;
        private readonly ITrailNode _v01;

        private ITrailNode _p10;
        private ITrailNode _p11;
        private ITrailNode _v10;

        public TrailCurvedSegment(
            ITrailNode handleNodeA,
            ITrailNode handleNodeB,
            ITrailNode handleNodeC
        ) {
            _p00 = handleNodeA.Plus(handleNodeB).Div(2.0f);
            _p01 = handleNodeB;
            var p02 = handleNodeB.Plus(handleNodeC).Div(2.0f);
            _v00 = _p01.Minus(_p00);
            _v01 = p02.Minus(_p01);
        }

        public ITrailNode Evaluate(float t) {
            _p10 = _p00.Plus(_v00.Times(t));
            _p11 = _p01.Plus(_v01.Times(t));
            _v10 = _p11.Minus(_p10);
            return _p10.Plus(_v10.Times(t));
        }
    }
}