using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace EasyOffset.Configuration {
    public class BaseGameConfigPreset : IConfigPreset {
        #region Constants

        public const string Version = "BaseGame";

        public string ConfigVersion => Version;
        public long UnixTimestamp => 0L;
        public ControllerType ControllerType => ControllerType.None;

        #endregion

        #region Properties

        public Vector3 RightHandPivotPosition { get; }
        public Vector3 RightHandSaberDirection { get; }
        public float RightHandZOffset { get; }
        public Vector3 LeftHandPivotPosition { get; }
        public Vector3 LeftHandSaberDirection { get; }
        public float LeftHandZOffset { get; }

        #endregion

        #region Constructor

        public BaseGameConfigPreset(
            bool isValveController,
            bool isVRModeOculus,
            float zOffset,
            Vector3 position,
            Vector3 rotation
        ) {
            ConfigConversions.FromBaseGame(
                isValveController,
                isVRModeOculus,
                zOffset,
                position,
                rotation,
                out var leftPivotPosition,
                out var rightPivotPosition,
                out var leftSaberDirection,
                out var rightSaberDirection
            );

            LeftHandZOffset = zOffset;
            RightHandZOffset = zOffset;
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

        public static BaseGameConfigPreset Deserialize(JObject jObject) {
            return new(
                jObject.GetValue("IsValveController", StringComparison.OrdinalIgnoreCase)!.Value<bool>(),
                jObject.GetValue("IsVRModeOculus", StringComparison.OrdinalIgnoreCase)!.Value<bool>(),
                jObject.GetValue("ZOffset", StringComparison.OrdinalIgnoreCase)!.Value<float>() * UnitScale,
                ParseVector(jObject, "Position") * UnitScale,
                ParseVector(jObject, "Rotation")
            );
        }

        private static Vector3 ParseVector(JObject jObject, string key) {
            return new(
                jObject.GetValue($"{key}X", StringComparison.OrdinalIgnoreCase)!.Value<float>(),
                jObject.GetValue($"{key}Y", StringComparison.OrdinalIgnoreCase)!.Value<float>(),
                jObject.GetValue($"{key}Z", StringComparison.OrdinalIgnoreCase)!.Value<float>()
            );
        }

        #endregion
    }
}