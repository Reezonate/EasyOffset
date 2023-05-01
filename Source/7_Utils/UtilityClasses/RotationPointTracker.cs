using System.Collections.Generic;
using UnityEngine;

namespace EasyOffset {
    public class RotationPointTracker {
        #region Constructor

        private readonly int _capacity;

        private readonly List<Vector3> _positions;
        private readonly List<Vector3> _directions;
        private readonly List<float> _weights;

        public RotationPointTracker(int capacity) {
            _capacity = capacity;
            _positions = new List<Vector3>(capacity);
            _directions = new List<Vector3>(capacity);
            _weights = new List<float>(capacity);
        }

        #endregion

        #region Update / Clear

        private Vector3 _previousPosition;
        private Quaternion _previousRotation;
        private bool _hasPrevious;

        public void Clear() {
            _positions.Clear();
            _directions.Clear();
            _weights.Clear();
            _hasPrevious = false;
        }

        public void Update(Vector3 position, Quaternion rotation, float deltaTime) {
            if (position == _previousPosition || rotation == _previousRotation || deltaTime <= 0) return;

            if (_hasPrevious) {
                CalculateOriginLine(
                    _previousPosition, _previousRotation,
                    position, rotation,
                    out var worldPoint, out var worldDirection
                );

                var angularVelocity = Quaternion.Angle(_previousRotation, rotation) / deltaTime;

                WorldToLocalLine(
                    position, rotation,
                    worldPoint, worldDirection,
                    out var localPoint, out var localDirection
                );

                if (_positions.Count < _capacity) {
                    _positions.Add(localPoint);
                    _directions.Add(localDirection);
                    _weights.Add(angularVelocity);
                } else {
                    TryReplaceWorst(localPoint, localDirection, angularVelocity);
                }
            }

            _previousPosition = position;
            _previousRotation = rotation;
            _hasPrevious = true;
        }

        private bool TryReplaceWorst(Vector3 position, Vector3 direction, float weight) {
            var worstWeight = float.MaxValue;
            var worstIndex = 0;

            for (var i = 0; i < _positions.Count; i++) {
                if (_weights[i] >= worstWeight) continue;
                worstWeight = _weights[i];
                worstIndex = i;
            }

            if (weight < worstWeight) return false;

            _positions[worstIndex] = position;
            _directions[worstIndex] = direction;
            _weights[worstIndex] = weight;
            return true;
        }

        #endregion

        #region LocalOrigin

        public Vector3 GetLocalOrigin() {
            return CalculatePoint(_positions, _directions);
        }

        #endregion

        #region CalculateOriginLine

        private static void WorldToLocalLine(
            Vector3 worldPosition, Quaternion worldRotation,
            Vector3 worldPoint, Vector3 worldDirection,
            out Vector3 localPoint, out Vector3 localDirection
        ) {
            var inverseRotation = Quaternion.Inverse(worldRotation);
            localPoint = inverseRotation * (worldPoint - worldPosition);
            localDirection = inverseRotation * worldDirection;
        }

        private static void CalculateOriginLine(
            Vector3 positionA, Quaternion rotationA,
            Vector3 positionB, Quaternion rotationB,
            out Vector3 worldPoint, out Vector3 worldDirection
        ) {
            var relativeRotation = rotationB * Quaternion.Inverse(rotationA);
            relativeRotation.ToAngleAxis(out var angle, out worldDirection);

            var diff = positionB - positionA;
            var halfAngle = angle * Mathf.Deg2Rad * 0.5f;
            var rayOrigin = (positionA + positionB) * 0.5f;
            var rayDirection = (Quaternion.AngleAxis(90f, worldDirection) * diff).normalized;
            var rayLength = Mathf.Cos(halfAngle) * diff.magnitude / (Mathf.Sin(halfAngle) * 2);
            worldPoint = rayOrigin + rayDirection * rayLength;
        }

        #endregion

        #region Math

        private static Vector3 CalculatePoint(IList<Vector3> a, IList<Vector3> d) {
            var m = new[] {Vector3.zero, Vector3.zero, Vector3.zero};
            var b = Vector3.zero;

            for (var i = 0; i < a.Count; ++i) {
                var da = Vector3.Dot(d[i], a[i]);

                for (var j = 0; j < 3; ++j) {
                    for (var k = 0; k < 3; ++k) {
                        m[j][k] += d[i][j] * d[i][k];
                    }

                    m[j][j] -= 1;
                    b[j] += d[i][j] * da - a[i][j];
                }
            }

            return Solve(ref m, ref b);
        }

        private static Vector3 Solve(ref Vector3[] m, ref Vector3 b) {
            var n = m.Length;

            for (var dia = 0; dia < n; dia++) {
                var maxRow = dia;
                var max = Mathf.Abs(m[dia][dia]);

                for (var row = dia + 1; row < n; row++) {
                    var tmp = Mathf.Abs(m[row][dia]);
                    if (!(tmp > max)) continue;
                    maxRow = row;
                    max = tmp;
                }

                SwapRows(ref m, ref b, dia, maxRow);

                for (var row = dia + 1; row < n; row++) {
                    var tmp = m[row][dia] / m[dia][dia];
                    for (var col = dia + 1; col < n; col++) {
                        m[row][col] -= tmp * m[dia][col];
                    }

                    m[row][dia] = 0;
                    b[row] -= tmp * b[dia];
                }
            }

            var result = Vector3.zero;

            for (var row = n - 1; row >= 0; row--) {
                var tmp = b[row];
                for (var j = n - 1; j > row; j--) {
                    tmp -= result[j] * m[row][j];
                }

                result[row] = tmp / m[row][row];
            }

            return result;
        }

        private static void SwapRows(ref Vector3[] m, ref Vector3 v, int r1, int r2) {
            if (r1 == r2) return;
            float tmp;
            for (var i = 0; i < m.Length; i++) {
                tmp = m[r1][i];
                m[r1][i] = m[r2][i];
                m[r2][i] = tmp;
            }

            tmp = v[r1];
            v[r1] = v[r2];
            v[r2] = tmp;
        }

        #endregion
    }
}