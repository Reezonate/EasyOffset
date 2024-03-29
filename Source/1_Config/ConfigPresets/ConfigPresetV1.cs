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

        private readonly Vector3 _leftSaberDirection;
        private readonly Vector3 _rightSaberDirection;

        public long UnixTimestamp { get; }
        public ControllerType ControllerType { get; }
        public Vector3 LeftSaberPivotPosition { get; }
        public Quaternion LeftSaberRotation => TransformUtils.RotationFromDirection(_leftSaberDirection);
        public float LeftSaberZOffset { get; }
        public bool LeftSaberHasReference => false;
        public Quaternion LeftSaberReferenceRotation => Quaternion.identity;
        public Vector3 RightSaberPivotPosition { get; }
        public Quaternion RightSaberRotation => TransformUtils.RotationFromDirection(_rightSaberDirection);
        public float RightSaberZOffset { get; }
        public bool RightSaberHasReference => false;
        public Quaternion RightSaberReferenceRotation => Quaternion.identity;

        #endregion

        #region Constructor

        private ConfigPresetV1(
            long unixTimestamp,
            ControllerType controllerType,
            Vector3 leftHandPivotPosition,
            Vector3 leftHandSaberDirection,
            float leftHandZOffset,
            Vector3 rightHandPivotPosition,
            Vector3 rightHandSaberDirection,
            float rightHandZOffset
        ) {
            UnixTimestamp = unixTimestamp;
            ControllerType = controllerType;
            LeftSaberPivotPosition = leftHandPivotPosition;
            _leftSaberDirection = leftHandSaberDirection;
            LeftSaberZOffset = leftHandZOffset;
            RightSaberPivotPosition = rightHandPivotPosition;
            _rightSaberDirection = rightHandSaberDirection;
            RightSaberZOffset = rightHandZOffset;
        }

        #endregion

        #region Serialization

        public JObject Serialize() {
            return new JObject {
                ["version"] = ConfigVersion,
                ["unixTimestamp"] = UnixTimestamp,
                ["controllerType"] = ControllerTypeUtils.TypeToName(ControllerType),

                ["leftHandPivotPositionX"] = LeftSaberPivotPosition.x,
                ["leftHandPivotPositionY"] = LeftSaberPivotPosition.y,
                ["leftHandPivotPositionZ"] = LeftSaberPivotPosition.z,
                ["leftHandSaberDirectionX"] = LeftSaberRotation.x,
                ["leftHandSaberDirectionY"] = LeftSaberRotation.y,
                ["leftHandSaberDirectionZ"] = LeftSaberRotation.z,
                ["leftHandZOffset"] = LeftSaberZOffset,

                ["rightHandPivotPositionX"] = RightSaberPivotPosition.x,
                ["rightHandPivotPositionY"] = RightSaberPivotPosition.y,
                ["rightHandPivotPositionZ"] = RightSaberPivotPosition.z,
                ["rightHandSaberDirectionX"] = RightSaberRotation.x,
                ["rightHandSaberDirectionY"] = RightSaberRotation.y,
                ["rightHandSaberDirectionZ"] = RightSaberRotation.z,
                ["rightHandZOffset"] = RightSaberZOffset,
            };
        }

        public static ConfigPresetV1 Deserialize(JObject jObject) {
            var unixTimestamp = jObject.GetValueUnsafe<long>("unixTimestamp");
            var controllerName = jObject.GetValueUnsafe<string>("controllerType");
            var controllerType = ControllerTypeUtils.NameToTypeOrDefault(controllerName);

            var leftHandPivotPosition = ParseVector(jObject, "leftHandPivotPosition");
            var leftHandSaberDirection = ParseVector(jObject, "leftHandSaberDirection");
            var leftHandZOffset = jObject.GetValueUnsafe<float>("leftHandZOffset");

            var rightHandPivotPosition = ParseVector(jObject, "rightHandPivotPosition");
            var rightHandSaberDirection = ParseVector(jObject, "rightHandSaberDirection");
            var rightHandZOffset = jObject.GetValueUnsafe<float>("rightHandZOffset");

            return new ConfigPresetV1(
                unixTimestamp,
                controllerType,
                leftHandPivotPosition,
                leftHandSaberDirection,
                leftHandZOffset,
                rightHandPivotPosition,
                rightHandSaberDirection,
                rightHandZOffset
            );
        }

        private static Vector3 ParseVector(JObject jObject, string key) {
            return new Vector3(
                jObject.GetValueUnsafe<float>($"{key}X"),
                jObject.GetValueUnsafe<float>($"{key}Y"),
                jObject.GetValueUnsafe<float>($"{key}Z")
            );
        }

        #endregion
    }
}