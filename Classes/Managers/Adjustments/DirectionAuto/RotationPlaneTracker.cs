using System.Collections.Generic;
using UnityEngine;

namespace EasyOffset {
    public class RotationPlaneTracker {
        private const int MaxNormalsCount = 60;
        private const float MinimalAngularVelocity = 45.0f;
        private const float MaximalAngularVelocity = 360.0f;
        private const float AngularVelocityAmplitude = MaximalAngularVelocity - MinimalAngularVelocity;

        #region Contructor

        private readonly Vector3[] _localPositions;
        private readonly Vector3[] _previousPositions;
        private readonly Vector3[] _previousVelocities;

        public RotationPlaneTracker(Mesh mesh) {
            _localPositions = mesh.vertices;
            _previousPositions = new Vector3[_localPositions.Length];
            _previousVelocities = new Vector3[_localPositions.Length];
        }

        #endregion

        #region Plane

        private Plane _lastPlane = new Plane(Vector3.forward, 0);
        private readonly List<WeightedNormal> _normalsList = new List<WeightedNormal>();

        public Plane GetPlane() {
            return _lastPlane;
        }

        private void AddNormal(
            Vector3 normal,
            float weight
        ) {
            _normalsList.Add(new WeightedNormal(normal, weight));
            if (_normalsList.Count > MaxNormalsCount) _normalsList.RemoveAt(0);

            var sum = Vector3.zero;
            var divider = 0f;

            foreach (var weightedNormal in _normalsList) {
                sum += weightedNormal.Normal * weightedNormal.Weight;
                divider += weightedNormal.Weight;
            }

            if (divider <= 0) return;
            var resultNormal = sum / divider;
            _lastPlane = new Plane(resultNormal, 0);
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

        public void Reset(Quaternion rotation) {
            _positiveDirection = rotation * Vector3.forward;
            _lastPlane = new Plane(_positiveDirection, 0);
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

                var weight = (angularVelocity - MinimalAngularVelocity) / AngularVelocityAmplitude;
                if (currentPosition == previousPosition || angularVelocity <= weight) continue;

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