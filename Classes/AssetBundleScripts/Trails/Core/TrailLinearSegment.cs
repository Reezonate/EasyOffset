namespace EasyOffset.AssetBundleScripts {
    public class TrailLinearSegment {
        private readonly TrailNode _from;
        private readonly TrailNode _amplitude;

        public TrailLinearSegment(TrailNode from, TrailNode to) {
            _from = from;
            _amplitude = to - from;
        }

        public TrailNode Evaluate(float t) {
            return _from + _amplitude * t;
        }
    }
}