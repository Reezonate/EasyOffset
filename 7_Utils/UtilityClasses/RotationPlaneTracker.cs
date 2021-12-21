using System.Collections.Generic;
using UnityEngine;

namespace EasyOffset {
    public class RotationPlaneTracker {
        #region Contructor

        private readonly int _averagingCount;
        private readonly float _minimalAngularVelocity;
        private readonly float _angularVelocityAmplitude;

        private readonly Vector3[] _localPositions;
        private readonly Vector3[] _previousPositions;
        private readonly Vector3[] _previousVelocities;

        public RotationPlaneTracker(
            Mesh mesh,
            int averagingCount,
            float minimalAngularVelocity,
            float maximalAngularVelocity
        ) {
            _localPositions = mesh.vertices;
            _previousPositions = new Vector3[_localPositions.Length];
            _previousVelocities = new Vector3[_localPositions.Length];

            _averagingCount = averagingCount;
            _minimalAngularVelocity = minimalAngularVelocity;
            _angularVelocityAmplitude = maximalAngularVelocity - minimalAngularVelocity;
        }

        #endregion

        #region Plane

        private Vector3 _normal = Vector3.forward;
        private readonly List<WeightedNormal> _normalsList = new();

        public Vector3 GetNormal() {
            return _normal;
        }

        private void AddNormal(
            Vector3 normal,
            float weight
        ) {
            _normalsList.Add(new WeightedNormal(normal, weight));
            if (_normalsList.Count > _averagingCount) _normalsList.RemoveAt(0);

            var sum = Vector3.zero;
            var divider = 0f;

            foreach (var weightedNormal in _normalsList) {
                sum += weightedNormal.Normal * weightedNormal.Weight;
                divider += weightedNormal.Weight;
            }

            if (divider <= 0) return;
            _normal = sum / divider;
        }

        private readonly struct WeightedNormal {
            public readonly Vector3 Normal;
            public readonly float Weight;

            public WeightedNormal(
                Vector3 normal,
                float weight
            ) {
                Normal = normal;
                Weight = weight;
            }
        }

        #endregion

        #region Reset

        private Vector3 _positiveDirection;

        public void Reset(Quaternion rotation, Vector3 positiveDirection) {
            _positiveDirection = positiveDirection;
            _normalsList.Clear();

            AddNormal(_positiveDirection, 1.0f);

            for (var i = 0; i < _localPositions.Length; i++) {
                var currentPosition = rotation * _localPositions[i];
                _previousPositions[i] = currentPosition;
                _previousVelocities[i] = Vector3.zero;
            }
        }

        #endregion

        #region Update

        public bool Update(Quaternion rotation) {
            var deltaTime = Time.deltaTime;
            var sum = Vector3.zero;
            var totalWeight = 0f;

            for (var i = 0; i < _localPositions.Length; i++) {
                var previousPosition = _previousPositions[i];
                var previousVelocity = _previousVelocities[i];
                var currentPosition = rotation * _localPositions[i];
                var currentVelocity = currentPosition - previousPosition;
                var angularVelocity = Vector3.Angle(currentPosition, previousPosition) / deltaTime;
                _previousPositions[i] = currentPosition;
                _previousVelocities[i] = currentVelocity;

                var weight = (angularVelocity - _minimalAngularVelocity) / _angularVelocityAmplitude;
                if (currentPosition == previousPosition || weight <= 0) continue;

                var plane = new Plane(previousPosition, previousPosition + previousVelocity, currentPosition);
                if (Vector3.Dot(plane.normal, _positiveDirection) < 0) plane.Flip();
                sum += plane.normal * weight;
                totalWeight += weight;
            }

            if (totalWeight <= 0) return false;
            var averagePlaneNormal = sum / totalWeight;
            AddNormal(averagePlaneNormal, totalWeight);
            return true;
        }

        #endregion
    }
}