namespace EasyOffset {
    public class TrailSpline {
        private readonly CyclicBuffer<TrailCurvedSegment> _curvedSegments;
        private readonly CyclicBuffer<TrailNode> _handles;

        private TrailLinearSegment _linearSegment;

        public TrailSpline(
            int capacity
        ) {
            _curvedSegments = new CyclicBuffer<TrailCurvedSegment>(capacity);
            _handles = new CyclicBuffer<TrailNode>(3);
        }

        #region Add

        private TrailNode _lastAddedNode;
        private bool _hasFirstNode;

        public bool Add(TrailNode node) {
            if (_hasFirstNode) {
                var linearFrom = (_lastAddedNode + node) / 2.0f;
                _linearSegment = new TrailLinearSegment(linearFrom, node);
                _lastAddedNode = node;
            } else {
                _lastAddedNode = node;
                _hasFirstNode = true;
            }

            if (!_handles.Add(node)) return false;
            var buffer = _handles.GetBuffer();
            _curvedSegments.Add(new TrailCurvedSegment(
                buffer[0],
                buffer[1],
                buffer[2]
            ));
            return true;
        }

        #endregion

        #region FillArray

        public void FillArray(TrailNode[] destination) {
            var splinesBuffer = _curvedSegments.GetBuffer();

            const float linearWeight = 0.5f;
            var splinesWeight = _curvedSegments.Size;
            var totalWeight = linearWeight + splinesWeight;
            var linearAmplitude = linearWeight / totalWeight;
            var splinesAmplitude = splinesWeight / totalWeight;

            for (var i = 0; i < destination.Length; i++) {
                var t = (float) i / (destination.Length - 1);

                if (t <= linearAmplitude) {
                    var localT = 1 - t / linearAmplitude;
                    destination[i] = GetPointLinear(localT);
                } else {
                    var localT = (t - linearAmplitude) / splinesAmplitude;
                    destination[i] = GetPointSplines(splinesBuffer, localT);
                }
            }
        }

        #endregion

        #region Evaluate

        private TrailNode GetPointLinear(float localT) {
            return _linearSegment.Evaluate(localT);
        }

        private TrailNode GetPointSplines(TrailCurvedSegment[] buffer, float localT) {
            var tPerSpline = 1f / _curvedSegments.Size;
            var splineIndex = (int) (localT / tPerSpline);
            if (splineIndex >= _curvedSegments.Size) splineIndex = _curvedSegments.Size - 1;
            var splineT = (localT - tPerSpline * splineIndex) / tPerSpline;
            return buffer[splineIndex].Evaluate(splineT);
        }

        #endregion
    }
}