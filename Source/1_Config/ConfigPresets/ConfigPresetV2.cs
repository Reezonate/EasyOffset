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
        public bool LeftSaberHasReference { get; }
        public Quaternion LeftSaberReferenceRotation { get; }

        public Vector3 RightSaberPivotPosition { get; }
        public Quaternion RightSaberRotation => TransformUtils.RotationFromEuler(_rightSaberRotationEuler);
        public float RightSaberZOffset { get; }
        public bool RightSaberHasReference { get; }
        public Quaternion RightSaberReferenceRotation { get; }

        #endregion

        #region Constructor

        public ConfigPresetV2(
            long unixTimestamp,
            ControllerType controllerType,
            Vector3 leftSaberPivotPosition,
            Vector3 leftSaberRotationEuler,
            float leftSaberZOffset,
            bool leftSaberHasReference,
            Quaternion leftSaberReferenceRotation,
            Vector3 rightSaberPivotPosition,
            Vector3 rightSaberRotationEuler,
            float rightSaberZOffset,
            bool rightSaberHasReference,
            Quaternion rightSaberReferenceRotation
        ) {
            UnixTimestamp = unixTimestamp;
            ControllerType = controllerType;

            LeftSaberPivotPosition = leftSaberPivotPosition;
            _leftSaberRotationEuler = leftSaberRotationEuler;
            LeftSaberZOffset = leftSaberZOffset;
            LeftSaberHasReference = leftSaberHasReference;
            LeftSaberReferenceRotation = leftSaberReferenceRotation;

            RightSaberPivotPosition = rightSaberPivotPosition;
            _rightSaberRotationEuler = rightSaberRotationEuler;
            RightSaberZOffset = rightSaberZOffset;
            RightSaberHasReference = rightSaberHasReference;
            RightSaberReferenceRotation = rightSaberReferenceRotation;
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
                ["LeftSaberHasReference"] = LeftSaberHasReference,
                ["LeftSaberReference"] = SerializeQuaternion(LeftSaberReferenceRotation),

                ["RightSaberPivotPosition"] = SerializeVector3(RightSaberPivotPosition / PositionUnitScale),
                ["RightSaberRotationEuler"] = SerializeVector3(_rightSaberRotationEuler),
                ["RightSaberZOffset"] = RightSaberZOffset / PositionUnitScale,
                ["RightSaberHasReference"] = RightSaberHasReference,
                ["RightSaberReference"] = SerializeQuaternion(RightSaberReferenceRotation),
            };
        }

        public static ConfigPresetV2 Deserialize(JObject jObject) {
            bool leftSaberHasReference, rightSaberHasReference;
            Quaternion leftSaberReferenceRotation, rightSaberReferenceRotation;

            try {
                leftSaberHasReference = jObject.GetValueUnsafe<bool>("LeftSaberHasReference");
                leftSaberReferenceRotation = ParseQuaternion(jObject, "LeftSaberReference");
            } catch (Exception) {
                leftSaberHasReference = false;
                leftSaberReferenceRotation = Quaternion.identity;
            }

            try {
                rightSaberHasReference = jObject.GetValueUnsafe<bool>("RightSaberHasReference");
                rightSaberReferenceRotation = ParseQuaternion(jObject, "RightSaberReference");
            } catch (Exception) {
                rightSaberHasReference = false;
                rightSaberReferenceRotation = Quaternion.identity;
            }

            return new ConfigPresetV2(
                jObject.GetValueUnsafe<long>("unixTimestamp"),
                ControllerTypeUtils.NameToTypeOrDefault(jObject.GetValueUnsafe<string>("controllerType")),
                ParseVector(jObject, "LeftSaberPivotPosition") * PositionUnitScale,
                ParseVector(jObject, "LeftSaberRotationEuler"),
                jObject.GetValueUnsafe<float>("LeftSaberZOffset") * PositionUnitScale,
                leftSaberHasReference,
                leftSaberReferenceRotation,
                ParseVector(jObject, "RightSaberPivotPosition") * PositionUnitScale,
                ParseVector(jObject, "RightSaberRotationEuler"),
                jObject.GetValueUnsafe<float>("RightSaberZOffset") * PositionUnitScale,
                rightSaberHasReference,
                rightSaberReferenceRotation
            );
        }

        private static JToken SerializeQuaternion(Quaternion rotation) {
            return new JObject {
                ["x"] = rotation.x,
                ["y"] = rotation.y,
                ["z"] = rotation.z,
                ["w"] = rotation.w,
            };
        }

        private static Quaternion ParseQuaternion(JObject jObject, string key) {
            var tmp = jObject.GetValueUnsafe<JObject>(key);
            return new Quaternion(
                tmp.GetValueUnsafe<float>("x"),
                tmp.GetValueUnsafe<float>("y"),
                tmp.GetValueUnsafe<float>("z"),
                tmp.GetValueUnsafe<float>("w")
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
            var tmp = jObject.GetValueUnsafe<JObject>(key);
            return new Vector3(
                tmp.GetValueUnsafe<float>("x"),
                tmp.GetValueUnsafe<float>("y"),
                tmp.GetValueUnsafe<float>("z")
            );
        }

        #endregion
    }
}