using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EasyOffset {
    public static class WeightedListExtensions {
        #region Average

        public static Vector3 GetAverage(this WeightedList<Vector3> list) {
            var result = Vector3.zero;
            var totalWeight = 0.0f;

            foreach (var entry in list.Entries) {
                totalWeight += entry.Weight;
                result += entry.Value * entry.Weight;
            }

            return totalWeight > 0 ? result / totalWeight : Vector3.zero;
        }

        #endregion

        #region Deviation

        public static float GetDeviationFromPoint(this WeightedList<Vector3> list, Vector3 point) {
            var meanSquareDeviation = list.Entries
                .Select(entry => (entry.Value - point).magnitude)
                .Select(distance => Mathf.Pow(distance, 2))
                .Average();

            return Mathf.Sqrt(meanSquareDeviation);
        }

        public static float GetDeviationFromPlane(this WeightedList<Vector3> list, Plane plane) {
            var meanSquareDeviation = list.Entries
                .Select(entry => plane.GetDistanceToPoint(entry.Value))
                .Select(distance => Mathf.Pow(distance, 2))
                .Average();

            return Mathf.Sqrt(meanSquareDeviation);
        }

        #endregion

        #region CalculatePlane

        public static Plane CalculatePlane(this WeightedList<Vector3> list, Vector3 initialNormal) {
            var averagePoint = list.GetAverage();

            var convergenceMatrix = list.Entries
                .Select(entry => (entry.Value - averagePoint))
                .Select(relative3 => new Vector4(relative3.x, relative3.y, relative3.z, 1))
                .Select(relative4 => MathUtils.GetOuterProduct(relative4, relative4))
                .Aggregate(Matrix4x4.zero, MathUtils.MatrixSum)
                .inverse;

            Vector4 normal = initialNormal;

            for (var i = 0; i < 20; i++) {
                normal = convergenceMatrix * normal;
                normal = normal.normalized;
            }

            return new Plane(normal, averagePoint);
        }

        #endregion

        #region GetMinMaxAngles

        public static void GetSwingAngles(
            this WeightedList<Vector3> list,
            Vector3 origin,
            Quaternion rotation,
            out float minAngle,
            out float maxAngle
        ) {
            var inverseRotation = Quaternion.Inverse(rotation);

            var swingData = list.Entries
                .Select(entry => inverseRotation * (entry.Value - origin))
                .Select(localPosition => Mathf.Atan2(localPosition.y, localPosition.x))
                .ToArray();

            AnalyzeSwingData(
                swingData,
                out var absoluteMinimumAngle,
                out var absoluteMaximumAngle,
                out var localMinimums,
                out var localMaximums
            );

            var angleMargin = Mathf.Abs(absoluteMaximumAngle - absoluteMinimumAngle) / 6;
            minAngle = GetAverageAngle(localMinimums, absoluteMinimumAngle, angleMargin);
            maxAngle = GetAverageAngle(localMaximums, absoluteMaximumAngle, angleMargin);
        }

        private static float GetAverageAngle(
            IEnumerable<float> angles,
            float filterAnchor,
            float filterMargin
        ) {
            var sum = 0f;
            var num = 0;

            foreach (var angle in angles) {
                var difference = Mathf.Abs(angle - filterAnchor);
                if (difference > filterMargin) continue;

                sum += angle;
                num += 1;
            }

            return (num > 0) ? sum / num : 0f;
        }

        private static void AnalyzeSwingData(
            float[] swingData,
            out float minimalSwingAngle,
            out float maximalSwingAngle,
            out List<float> localMinimums,
            out List<float> localMaximums
        ) {
            localMinimums = new List<float>();
            localMaximums = new List<float>();
            minimalSwingAngle = 0f;
            maximalSwingAngle = 0f;

            var previousAngle = 0f;
            var isPreviousDirectionPositive = false;
            var hasPreviousDirection = false;

            var count = 0;
            foreach (var angle in swingData) {
                if (count++ == 0) {
                    minimalSwingAngle = angle;
                    maximalSwingAngle = angle;
                    localMinimums.Add(angle);
                    localMaximums.Add(angle);
                    previousAngle = angle;
                } else {
                    if (angle > maximalSwingAngle) maximalSwingAngle = angle;
                    if (angle < minimalSwingAngle) minimalSwingAngle = angle;

                    var angularVelocity = angle - previousAngle;
                    var isDirectionPositive = angularVelocity >= 0;

                    if (hasPreviousDirection && isDirectionPositive != isPreviousDirectionPositive) {
                        if (isPreviousDirectionPositive) {
                            localMaximums.Add(previousAngle);
                        } else {
                            localMinimums.Add(previousAngle);
                        }
                    }

                    hasPreviousDirection = true;
                    previousAngle = angle;
                    isPreviousDirectionPositive = isDirectionPositive;
                }
            }

            if (localMaximums.Count > 1) localMaximums.RemoveAt(0);
            if (localMinimums.Count > 1) localMinimums.RemoveAt(0);

            var lastAngle = swingData.Last();

            if (lastAngle >= maximalSwingAngle) {
                maximalSwingAngle = lastAngle;
                localMaximums.Add(lastAngle);
            }

            if (lastAngle <= minimalSwingAngle) {
                minimalSwingAngle = lastAngle;
                localMinimums.Add(lastAngle);
            }
        }

        #endregion
    }
}