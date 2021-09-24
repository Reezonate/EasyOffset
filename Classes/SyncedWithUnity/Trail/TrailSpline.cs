using UnityEngine;

namespace EasyOffset.SyncedWithUnity {
    public class TrailSpline {
        private readonly CyclicBuffer<CubicBezierCurve> _splines;
        private readonly CyclicBuffer<Vector3> _handles;

        private Vector3 _linearFrom = Vector3.zero;
        private Vector3 _linearTo = Vector3.zero;

        public TrailSpline(
            int capacity
        ) {
            _splines = new CyclicBuffer<CubicBezierCurve>(capacity);
            _handles = new CyclicBuffer<Vector3>(3);
        }

        public bool Add(Vector3 point) {
            _linearTo = _linearFrom;
            _linearFrom = point;

            if (!_handles.Add(point)) return false;
            var buffer = _handles.GetBuffer();
            var spline = new CubicBezierCurve(
                buffer[0],
                buffer[1],
                buffer[2]
            );
            _splines.Add(spline);
            return true;
        }

        public void FillArray(Vector3[] destination) {
            const float linearWeight = 1f;
            var splinesWeight = _splines.Size * 3f;
            var totalWeight = linearWeight + splinesWeight;
            var linearAmplitude = linearWeight / totalWeight;
            var splinesAmplitude = splinesWeight / totalWeight;
            var splinesBuffer = _splines.GetBuffer();

            for (var i = 0; i < destination.Length; i++) {
                var t = (float) i / (destination.Length - 1);
                if (t <= linearAmplitude) {
                    var localT = (linearAmplitude - t) / linearAmplitude;
                    destination[i] = GetPointLinear(localT);
                } else {
                    var localT = (t - linearAmplitude) / splinesAmplitude;
                    destination[i] = GetPointSplines(splinesBuffer, localT);
                }
            }
        }

        private Vector3 GetPointLinear(float localT) {
            return _linearFrom + (_linearTo - _linearFrom) * localT;
        }

        private Vector3 GetPointSplines(CubicBezierCurve[] buffer, float localT) {
            var tPerSpline = 1f / _splines.Size;
            var splineIndex = (int) (localT / tPerSpline);
            if (splineIndex >= _splines.Size) splineIndex = _splines.Size - 1;
            var splineT = (localT - tPerSpline * splineIndex) / tPerSpline;
            return buffer[splineIndex].GetPoint(splineT);
        }
    }
}