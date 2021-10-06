using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace EasyOffset.Configuration {
    public class SaberTailorConfigPreset : IConfigPreset {
        public string ConfigVersion => "SaberTailor";
        public long UnixTimestamp => 0L;

        public ControllerType ControllerType => ControllerType.None;

        #region Properties

        public Vector3 RightHandPivotPosition { get; }
        public Vector3 RightHandSaberDirection { get; }
        public float RightHandZOffset { get; }
        public Vector3 LeftHandPivotPosition { get; }
        public Vector3 LeftHandSaberDirection { get; }
        public float LeftHandZOffset { get; }

        #endregion

        #region Constructor

        public SaberTailorConfigPreset(
            bool useBaseGameAdjustmentMode,
            bool isValveController,
            bool isVRModeOculus,
            float leftZOffset,
            float rightZOffset,
            Vector3 gripLeftPosition,
            Vector3 gripRightPosition,
            Vector3 gripLeftRotation,
            Vector3 gripRightRotation
        ) {
            ConfigConversions.FromTailor(
                useBaseGameAdjustmentMode,
                isValveController,
                isVRModeOculus,
                leftZOffset,
                rightZOffset,
                gripLeftPosition,
                gripRightPosition,
                gripLeftRotation,
                gripRightRotation,
                out var leftPivotPosition,
                out var rightPivotPosition,
                out var leftSaberDirection,
                out var rightSaberDirection
            );

            LeftHandZOffset = leftZOffset;
            RightHandZOffset = rightZOffset;
            LeftHandPivotPosition = leftPivotPosition;
            RightHandPivotPosition = rightPivotPosition;
            LeftHandSaberDirection = leftSaberDirection;
            RightHandSaberDirection = rightSaberDirection;
        }

        #endregion

        #region Serialize

        public JObject Serialize() {
            throw new NotImplementedException();
        }

        #endregion

        #region Deserialize

        private const float UnitScale = 0.001f;

        public static SaberTailorConfigPreset Deserialize(JObject jObject) {
            return new(
                jObject.GetValue("UseBaseGameAdjustmentMode", StringComparison.OrdinalIgnoreCase)!.Value<bool>(),
                jObject.GetValue("IsValveController", StringComparison.OrdinalIgnoreCase)!.Value<bool>(),
                jObject.GetValue("IsVRModeOculus", StringComparison.OrdinalIgnoreCase)!.Value<bool>(),
                jObject.GetValue("LeftHandZOffset", StringComparison.OrdinalIgnoreCase)!.Value<float>() * UnitScale,
                jObject.GetValue("RightHandZOffset", StringComparison.OrdinalIgnoreCase)!.Value<float>() * UnitScale,
                ParseVector(jObject, "GripLeftPosition") * UnitScale,
                ParseVector(jObject, "GripRightPosition") * UnitScale,
                ParseVector(jObject, "GripLeftRotation"),
                ParseVector(jObject, "GripRightRotation")
            );
        }

        private static Vector3 ParseVector(JObject jObject, string key) {
            var vectorObject = jObject[key]!.Value<JObject>();
            return new Vector3(
                vectorObject.GetValue("x", StringComparison.OrdinalIgnoreCase)!.Value<float>(),
                vectorObject.GetValue("y", StringComparison.OrdinalIgnoreCase)!.Value<float>(),
                vectorObject.GetValue("z", StringComparison.OrdinalIgnoreCase)!.Value<float>()
            );
        }

        #endregion
    }
}