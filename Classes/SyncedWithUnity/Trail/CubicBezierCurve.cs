using UnityEngine;

namespace EasyOffset.SyncedWithUnity {
    public class CubicBezierCurve {
        private readonly Vector3 _p00;
        private readonly Vector3 _p01;
        private readonly Vector3 _v00;
        private readonly Vector3 _v01;

        private Vector3 _p10;
        private Vector3 _p11;
        private Vector3 _v10;

        public CubicBezierCurve(
            Vector3 handlePoint0,
            Vector3 handlePoint1,
            Vector3 handlePoint2
        ) {
            _p00 = (handlePoint0 + handlePoint1) / 2;
            _p01 = handlePoint1;
            var p02 = (handlePoint1 + handlePoint2) / 2;
            _v00 = _p01 - _p00;
            _v01 = p02 - _p01;
        }

        public Vector3 GetPoint(float t) {
            _p10 = _p00 + _v00 * t;
            _p11 = _p01 + _v01 * t;
            _v10 = _p11 - _p10;
            return _p10 + _v10 * t;
        }
    }
}