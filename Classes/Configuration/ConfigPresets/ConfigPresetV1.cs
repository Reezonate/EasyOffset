using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace EasyOffset.Configuration {
    public class ConfigPresetV1 : IConfigPreset {
        public string ConfigVersion => "1.0";

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
            var unixTimestamp = jObject["unixTimestamp"]!.Value<long>();
            var controllerType = ControllerTypeUtils.NameToTypeOrDefault(jObject["controllerType"]!.Value<string>());

            var rightHandPivotPosition = new Vector3(
                jObject["rightHandPivotPositionX"]!.Value<float>(),
                jObject["rightHandPivotPositionY"]!.Value<float>(),
                jObject["rightHandPivotPositionZ"]!.Value<float>()
            );
            var rightHandSaberDirection = new Vector3(
                jObject["rightHandSaberDirectionX"]!.Value<float>(),
                jObject["rightHandSaberDirectionY"]!.Value<float>(),
                jObject["rightHandSaberDirectionZ"]!.Value<float>()
            );
            var rightHandZOffset = jObject["rightHandZOffset"]!.Value<float>();

            var leftHandPivotPosition = new Vector3(
                jObject["leftHandPivotPositionX"]!.Value<float>(),
                jObject["leftHandPivotPositionY"]!.Value<float>(),
                jObject["leftHandPivotPositionZ"]!.Value<float>()
            );
            var leftHandSaberDirection = new Vector3(
                jObject["leftHandSaberDirectionX"]!.Value<float>(),
                jObject["leftHandSaberDirectionY"]!.Value<float>(),
                jObject["leftHandSaberDirectionZ"]!.Value<float>()
            );
            var leftHandZOffset = jObject["leftHandZOffset"]!.Value<float>();

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

        #endregion
    }
}