namespace EasyOffset.AssetBundleScripts {
    public class TrailLinearSegment {
        private readonly ITrailNode _from;
        private readonly ITrailNode _amplitude;

        public TrailLinearSegment(ITrailNode from, ITrailNode to) {
            _from = from;
            _amplitude = to.Minus(from);
        }

        public ITrailNode Evaluate(float t) {
            return _from.Plus(_amplitude.Times(t));
        }
    }
}