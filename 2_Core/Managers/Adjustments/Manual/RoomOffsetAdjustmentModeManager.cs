using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.XR;
using Zenject;

namespace EasyOffset {
    [UsedImplicitly]
    public class RoomOffsetAdjustmentModeManager : AbstractAdjustmentModeManager, ITickable {
        #region Constructor

        private IVRPlatformHelper _vrPlatformHelper;

        public RoomOffsetAdjustmentModeManager(
            IVRPlatformHelper vrPlatformHelper
        ) : base(
            AdjustmentMode.RoomOffset,
            6f,
            6f
        ) {
            _vrPlatformHelper = vrPlatformHelper;
        }

        #endregion

        #region HMD position tracking

        public static event Action<Vector3> OnHmdPositionChangedEvent;

        public void Tick() {
            if (PluginConfig.AdjustmentMode != AdjustmentMode.RoomOffset) return;
            if (!_vrPlatformHelper.GetNodePose(XRNode.Head, 0, out var headPos, out _)) return;
            TransformUtils.ApplyRoomOffsetToVector(ref headPos);
            OnHmdPositionChangedEvent?.Invoke(headPos);
        }

        #endregion

        #region Logic

        private Vector3 _grabPosition;
        private Vector3 _grabRoomCenter;

        protected override void OnGrabStarted(
            Hand adjustmentHand,
            ReeTransform adjustmentHandTransform,
            ReeTransform freeHandTransform
        ) {
            _grabPosition = adjustmentHandTransform.Position;
            _grabRoomCenter = PluginConfig.MainSettingsModel.roomCenter.value;
        }

        protected override void OnGrabUpdated(
            Hand adjustmentHand,
            ReeTransform adjustmentHandTransform,
            ReeTransform freeHandTransform
        ) {
            var positionChange = adjustmentHandTransform.Position - _grabPosition;
            var result = _grabRoomCenter - positionChange;

            PluginConfig.MainSettingsModel.roomCenter.value = new Vector3(
                PluginConfig.AllowRoomXChange ? result.x : _grabRoomCenter.x,
                PluginConfig.AllowRoomYChange ? result.y : _grabRoomCenter.y,
                PluginConfig.AllowRoomZChange ? result.z : _grabRoomCenter.z
            );
        }

        protected override void OnGrabFinished(
            Hand adjustmentHand,
            ReeTransform adjustmentHandTransform,
            ReeTransform freeHandTransform
        ) { }

        #endregion
    }
}