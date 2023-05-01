using System;
using UnityEngine;

namespace EasyOffset {
    internal class ReeTrail : AbstractComputeTrail {
        #region Serialized

        [SerializeField] private float dotOffset;
        [SerializeField] private bool isTracking = true;
        [SerializeField] private bool resetOnEnable = true;

        #endregion

        #region OnEnable

        private void OnEnable() {
            if (resetOnEnable) HardReset();
        }

        #endregion

        #region LateUpdate

        private Vector3 _previousDotPosition = Vector3.zero;

        public void LateUpdate() {
            if (!isTracking) return;

            var t = transform;
            var position = t.position;
            var forward = t.forward;

            var dotPosition = position + forward * dotOffset;
            var velocity = (dotPosition - _previousDotPosition) / Time.deltaTime;

            ComputeTrailNode node;
            node.Position = position;
            node.Data0 = forward;
            node.Data1 = velocity;
            AddNewNode(node);

            _previousDotPosition = dotPosition;
        }

        #endregion

        #region Interaction

        public void SetLifetime(int frames) {
            Lifetime = frames;
        }

        public void StartTracking() {
            isTracking = true;
            HardReset();
        }

        public void StopTracking() {
            isTracking = false;
        }

        #endregion
    }
}