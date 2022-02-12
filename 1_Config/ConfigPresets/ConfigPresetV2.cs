using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace EasyOffset {
    public class ConfigPresetV2 : IConfigPreset {
        #region Constants

        public const string Version = "2.0";

        public string ConfigVersion => Version;

        #endregion

        #region Properties

        private readonly Vector3 _leftSaberRotationEuler;
        private readonly Vector3 _rightSaberRotationEuler;

        public long UnixTimestamp { get; }
        public ControllerType ControllerType { get; }
        public Vector3 LeftSaberPivotPosition { get; }
        public Quaternion LeftSaberRotation => TransformUtils.RotationFromEuler(_leftSaberRotationEuler);
        public float LeftSaberZOffset { get; }
        public Vector3 RightSaberPivotPosition { get; }
        public Quaternion RightSaberRotation => TransformUtils.RotationFromEuler(_rightSaberRotationEuler);
        public float RightSaberZOffset { get; }

        #endregion

        #region Constructor

        public ConfigPresetV2(
            long unixTimestamp,
            ControllerType controllerType,
            Vector3 leftSaberPivotPosition,
            Vector3 leftSaberRotationEuler,
            float leftSaberZOffset,
            Vector3 rightSaberPivotPosition,
            Vector3 rightSaberRotationEuler,
            float rightSaberZOffset
        ) {
            UnixTimestamp = unixTimestamp;
            ControllerType = controllerType;
            LeftSaberPivotPosition = leftSaberPivotPosition;
            _leftSaberRotationEuler = leftSaberRotationEuler;
            LeftSaberZOffset = leftSaberZOffset;
            RightSaberPivotPosition = rightSaberPivotPosition;
            _rightSaberRotationEuler = rightSaberRotationEuler;
            RightSaberZOffset = rightSaberZOffset;
        }

        #endregion

        #region Serialization

        private const float PositionUnitScale = 0.01f;

        public JObject Serialize() {
            return new JObject {
                ["Version"] = ConfigVersion,
                ["UnixTimestamp"] = UnixTimestamp,
                ["ControllerType"] = ControllerTypeUtils.TypeToName(ControllerType),

                ["LeftSaberPivotPosition"] = SerializeVector3(LeftSaberPivotPosition / PositionUnitScale),
                ["LeftSaberRotationEuler"] = SerializeVector3(_leftSaberRotationEuler),
                ["LeftSaberZOffset"] = LeftSaberZOffset / PositionUnitScale,

                ["RightSaberPivotPosition"] = SerializeVector3(RightSaberPivotPosition / PositionUnitScale),
                ["RightSaberRotationEuler"] = SerializeVector3(_rightSaberRotationEuler),
                ["RightSaberZOffset"] = RightSaberZOffset / PositionUnitScale,
            };
        }

        public static ConfigPresetV2 Deserialize(JObject jObject) {
            return new ConfigPresetV2(
                jObject.GetValue("unixTimestamp", StringComparison.OrdinalIgnoreCase)!.Value<long>(),
                ControllerTypeUtils.NameToTypeOrDefault(jObject.GetValue("controllerType", StringComparison.OrdinalIgnoreCase)!.Value<string>()),
                ParseVector(jObject, "LeftSaberPivotPosition") * PositionUnitScale,
                ParseVector(jObject, "LeftSaberRotationEuler"),
                jObject.GetValue("LeftSaberZOffset", StringComparison.OrdinalIgnoreCase)!.Value<float>() * PositionUnitScale,
                ParseVector(jObject, "RightSaberPivotPosition") * PositionUnitScale,
                ParseVector(jObject, "RightSaberRotationEuler"),
                jObject.GetValue("RightSaberZOffset", StringComparison.OrdinalIgnoreCase)!.Value<float>() * PositionUnitScale
            );
        }

        private static JToken SerializeVector3(Vector3 vector) {
            return new JObject {
                ["x"] = vector.x,
                ["y"] = vector.y,
                ["z"] = vector.z,
            };
        }

        private static Vector3 ParseVector(JObject jObject, string key) {
            var tmp = jObject.GetValue(key, StringComparison.OrdinalIgnoreCase)!.Value<JObject>();
            return new Vector3(
                tmp.GetValue("x", StringComparison.OrdinalIgnoreCase)!.Value<float>(),
                tmp.GetValue("y", StringComparison.OrdinalIgnoreCase)!.Value<float>(),
                tmp.GetValue("z", StringComparison.OrdinalIgnoreCase)!.Value<float>()
            );
        }

        #endregion
    }
}