using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace EasyOffset {
    public class ConfigPresetV1 : IConfigPreset {
        #region Constants

        public const string Version = "1.0";

        public string ConfigVersion => Version;

        #endregion

        #region Properties

        public long UnixTimestamp { get; }
        public ControllerType ControllerType { get; }
        public Vector3 RightHandPivotPosition { get; }
        public Vector3 RightHandSaberDirection { get; }
        public float RightHandZOffset { get; }
        public Vector3 LeftHandPivotPosition { get; }
        public Vector3 LeftHandSaberDirection { get; }
        public float LeftHandZOffset { get; }

        #endregion

        #region Constructor

        public ConfigPresetV1(
            long unixTimestamp,
            ControllerType controllerType,
            Vector3 rightHandPivotPosition,
            Vector3 rightHandSaberDirection,
            float rightHandZOffset,
            Vector3 leftHandPivotPosition,
            Vector3 leftHandSaberDirection,
            float leftHandZOffset
        ) {
            UnixTimestamp = unixTimestamp;
            ControllerType = controllerType;
            RightHandPivotPosition = rightHandPivotPosition;
            RightHandSaberDirection = rightHandSaberDirection;
            RightHandZOffset = rightHandZOffset;
            LeftHandPivotPosition = leftHandPivotPosition;
            LeftHandSaberDirection = leftHandSaberDirection;
            LeftHandZOffset = leftHandZOffset;
        }

        #endregion

        #region Serialization

        public JObject Serialize() {
            return new() {
                ["version"] = ConfigVersion,
                ["unixTimestamp"] = UnixTimestamp,
                ["controllerType"] = ControllerTypeUtils.TypeToName(ControllerType),

                ["rightHandPivotPositionX"] = RightHandPivotPosition.x,
                ["rightHandPivotPositionY"] = RightHandPivotPosition.y,
                ["rightHandPivotPositionZ"] = RightHandPivotPosition.z,
                ["rightHandSaberDirectionX"] = RightHandSaberDirection.x,
                ["rightHandSaberDirectionY"] = RightHandSaberDirection.y,
                ["rightHandSaberDirectionZ"] = RightHandSaberDirection.z,
                ["rightHandZOffset"] = RightHandZOffset,

                ["leftHandPivotPositionX"] = LeftHandPivotPosition.x,
                ["leftHandPivotPositionY"] = LeftHandPivotPosition.y,
                ["leftHandPivotPositionZ"] = LeftHandPivotPosition.z,
                ["leftHandSaberDirectionX"] = LeftHandSaberDirection.x,
                ["leftHandSaberDirectionY"] = LeftHandSaberDirection.y,
                ["leftHandSaberDirectionZ"] = LeftHandSaberDirection.z,
                ["leftHandZOffset"] = LeftHandZOffset,
            };
        }

        public static ConfigPresetV1 Deserialize(JObject jObject) {
            var unixTimestamp = jObject.GetValue("unixTimestamp", StringComparison.OrdinalIgnoreCase)!.Value<long>();
            var controllerName = jObject.GetValue("controllerType", StringComparison.OrdinalIgnoreCase)!.Value<string>();
            var controllerType = ControllerTypeUtils.NameToTypeOrDefault(controllerName);

            var rightHandPivotPosition = ParseVector(jObject, "rightHandPivotPosition");
            var rightHandSaberDirection = ParseVector(jObject, "rightHandSaberDirection");
            var rightHandZOffset = jObject.GetValue("rightHandZOffset", StringComparison.OrdinalIgnoreCase)!.Value<float>();

            var leftHandPivotPosition = ParseVector(jObject, "leftHandPivotPosition");
            var leftHandSaberDirection = ParseVector(jObject, "leftHandSaberDirection");
            var leftHandZOffset = jObject.GetValue("leftHandZOffset", StringComparison.OrdinalIgnoreCase)!.Value<float>();

            return new ConfigPresetV1(
                unixTimestamp,
                controllerType,
                rightHandPivotPosition,
                rightHandSaberDirection,
                rightHandZOffset,
                leftHandPivotPosition,
                leftHandSaberDirection,
                leftHandZOffset
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