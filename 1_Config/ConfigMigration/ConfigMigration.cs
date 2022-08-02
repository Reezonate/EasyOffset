using System;
using System.IO;
using System.Reflection;
using IPA.Utilities;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EasyOffset {
    internal static class ConfigMigration {
        #region Constructor

        static ConfigMigration() {
            PluginConfig.VRPlatformHelperChangedEvent += OnVRPlatformHelperChanged;
            OnVRPlatformHelperChanged(PluginConfig.VRPlatformHelper);
        }

        #endregion

        #region Variables

        public static float ZOffset = ConfigDefaults.ZOffset;
        public static bool IsMigrationPossible { get; private set; }

        public static bool IsValveController { get; private set; }
        public static bool IsVRModeOculus { get; private set; }

        #endregion

        #region OnVRPlatformHelperChanged

        private static readonly PropertyInfo OpenVRHelperVRControllerManufacturerNamePropertyInfo = typeof(OpenVRHelper).GetProperty(
            "vrControllerManufacturerName",
            BindingFlags.Instance | BindingFlags.NonPublic
        );

        private static void OnVRPlatformHelperChanged(IVRPlatformHelper vrPlatformHelper) {
            switch (vrPlatformHelper) {
                case OpenVRHelper openVRHelper:
                    UseOpenVRHelper(openVRHelper);
                    break;
                case OculusVRHelper oculusVRHelper:
                    UseOculusVRHelper();
                    break;
                case DevicelessVRHelper devicelessVRHelper:
                    UseDevicelessVRHelper();
                    break;
                default:
                    Plugin.Log.Debug($"Unknown VRPlatformHelper type: {nameof(vrPlatformHelper)}");
                    UseDevicelessVRHelper();
                    return;
            }
        }

        private static void UseOpenVRHelper(OpenVRHelper vrPlatformHelper) {
            var manufacturerName = OpenVRHelperVRControllerManufacturerNamePropertyInfo?.GetValue(vrPlatformHelper);

            if (manufacturerName == null) {
                IsMigrationPossible = false;
                IsValveController = false;
                IsVRModeOculus = false;
            } else {
                IsMigrationPossible = true;
                IsValveController = (OpenVRHelper.VRControllerManufacturerName)manufacturerName == OpenVRHelper.VRControllerManufacturerName.Valve;
            }
        }

        private static void UseOculusVRHelper() {
            IsMigrationPossible = true;
            IsValveController = false;
            IsVRModeOculus = true;
        }

        private static void UseDevicelessVRHelper() {
            IsMigrationPossible = false;
            IsValveController = false;
            IsVRModeOculus = false;
        }

        #endregion

        #region ImportFromSettings

        public static ConfigImportResult ImportFromSettings() {
            if (!IsMigrationPossible) return ConfigImportResult.DevicelessFail;

            var position = PluginConfig.MainSettingsModel.controllerPosition.value;
            var rotationEuler = PluginConfig.MainSettingsModel.controllerRotation.value;

            ConfigConversions.FromBaseGame(
                IsValveController,
                IsVRModeOculus,
                ZOffset,
                position,
                rotationEuler,
                out var leftPivotPosition,
                out var rightPivotPosition,
                out var leftSaberRotation,
                out var rightSaberRotation
            );

            PluginConfig.LeftSaberZOffset = ZOffset;
            PluginConfig.LeftSaberPivotPosition = leftPivotPosition;
            PluginConfig.LeftSaberRotation = leftSaberRotation;

            PluginConfig.RightSaberZOffset = ZOffset;
            PluginConfig.RightSaberPivotPosition = rightPivotPosition;
            PluginConfig.RightSaberRotation = rightSaberRotation;

            return ConfigImportResult.Success;
        }

        #endregion

        #region UniversalImport

        public static ConfigImportResult UniversalImport() {
            if (!IsMigrationPossible) return ConfigImportResult.DevicelessFail;

            var menuPlayerController = Object.FindObjectOfType<MenuPlayerController>();
            if (menuPlayerController == null) return ConfigImportResult.InternalError;

            if (!ImportFromVRController(menuPlayerController.leftController, out var leftPivotPosition, out var leftSaberRotation)) {
                return ConfigImportResult.InternalError;
            }

            if (!ImportFromVRController(menuPlayerController.rightController, out var rightPivotPosition, out var rightSaberRotation)) {
                return ConfigImportResult.InternalError;
            }

            PluginConfig.LeftSaberZOffset = ZOffset;
            PluginConfig.LeftSaberPivotPosition = leftPivotPosition;
            PluginConfig.LeftSaberRotation = leftSaberRotation;

            PluginConfig.RightSaberZOffset = ZOffset;
            PluginConfig.RightSaberPivotPosition = rightPivotPosition;
            PluginConfig.RightSaberRotation = rightSaberRotation;

            return ConfigImportResult.Success;
        }

        private static bool ImportFromVRController(
            VRController vrController,
            out Vector3 pivotPosition,
            out Quaternion saberRotation
        ) {
            var saberWorldPosition = vrController.position;
            var saberWorldRotation = vrController.rotation;

            if (!PluginConfig.VRPlatformHelper.GetNodePose(vrController.node, vrController.nodeIdx, out var controllerWorldPosition, out var controllerWorldRotation)) {
                pivotPosition = Vector3.zero;
                saberRotation = Quaternion.identity;
                return false;
            }

            TransformUtils.ApplyRoomOffset(ref controllerWorldPosition, ref controllerWorldRotation);

            ConfigConversions.Universal(
                IsVRModeOculus,
                controllerWorldPosition,
                controllerWorldRotation,
                saberWorldPosition,
                saberWorldRotation,
                ZOffset,
                out pivotPosition,
                out saberRotation
            );

            return true;
        }

        #endregion

        #region ExportToSettings

        public static ConfigExportResult ExportToSettings(Hand hand) {
            if (!IsMigrationPossible) return ConfigExportResult.DevicelessFail;

            Vector3 translation;
            Quaternion rotation;

            switch (hand) {
                case Hand.Left:
                    translation = TransformUtils.MirrorVector(PluginConfig.LeftSaberTranslation);
                    rotation = TransformUtils.MirrorRotation(PluginConfig.LeftSaberRotation);
                    break;
                case Hand.Right:
                    translation = PluginConfig.RightSaberTranslation;
                    rotation = PluginConfig.RightSaberRotation;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(hand), hand, null);
            }

            ConfigConversions.ToBaseGame(
                IsValveController,
                IsVRModeOculus,
                translation,
                rotation,
                out var position,
                out var rotationEuler
            );

            PluginConfig.MainSettingsModel.controllerPosition.value = position;
            PluginConfig.MainSettingsModel.controllerRotation.value = rotationEuler;

            return ConfigExportResult.Success;
        }

        #endregion

        #region ExportToSaberTailor

        public static ConfigExportResult ExportToSaberTailor() {
            if (!IsMigrationPossible) return ConfigExportResult.DevicelessFail;

            ConfigConversions.ToTailor(
                IsValveController,
                IsVRModeOculus,
                PluginConfig.LeftSaberTranslation,
                PluginConfig.LeftSaberRotation,
                PluginConfig.RightSaberTranslation,
                PluginConfig.RightSaberRotation,
                out var gripLeftPosition,
                out var gripRightPosition,
                out var gripLeftRotation,
                out var gripRightRotation
            );

            var result = WriteSaberTailorConfig(
                gripLeftPosition,
                gripRightPosition,
                gripLeftRotation,
                gripRightRotation
            );

            return result ? ConfigExportResult.Success : ConfigExportResult.WriteFail;
        }

        #endregion

        #region SaberTailorConfig

        private static readonly string TailorExportConfigPath = Path.Combine(UnityGame.UserDataPath, "SaberTailor.EasyOffsetExported.json");

        private const float UnitScale = 0.001f;

        private static bool WriteSaberTailorConfig(
            Vector3 gripLeftPosition,
            Vector3 gripRightPosition,
            Vector3 gripLeftRotation,
            Vector3 gripRightRotation
        ) {
            try {
                var jObject = new JObject {
                    ["ConfigVersion"] = 5,
                    ["IsSaberScaleModEnabled"] = false,
                    ["SaberScaleHitbox"] = false,
                    ["SaberLength"] = 100,
                    ["SaberGirth"] = 100,
                    ["IsTrailModEnabled"] = false,
                    ["IsTrailEnabled"] = true,
                    ["TrailDuration"] = 400,
                    ["TrailGranularity"] = 60,
                    ["TrailWhiteSectionDuration"] = 100,
                    ["IsGripModEnabled"] = true,
                    ["GripLeftPosition"] = JObjectFromVector(gripLeftPosition / UnitScale),
                    ["GripRightPosition"] = JObjectFromVector(gripRightPosition / UnitScale),
                    ["GripLeftRotation"] = JObjectFromVector(gripLeftRotation),
                    ["GripRightRotation"] = JObjectFromVector(gripRightRotation),
                    ["GripLeftOffset"] = JObjectFromVector(Vector3.zero),
                    ["GripRightOffset"] = JObjectFromVector(Vector3.zero),
                    ["ModifyMenuHiltGrip"] = true,
                    ["UseBaseGameAdjustmentMode"] = true,
                    ["SaberPosIncrement"] = 10,
                    ["SaberPosIncValue"] = 1,
                    ["SaberRotIncrement"] = 5,
                    ["SaberPosIncUnit"] = "cm",
                    ["SaberPosDisplayUnit"] = "cm"
                };

                File.WriteAllText(TailorExportConfigPath, jObject.ToString());
                return true;
            } catch (Exception) {
                return false;
            }
        }

        private static Vector3 VectorFromJObject(JObject jObject, string key) {
            var vectorObject = jObject.GetValueUnsafe<JObject>(key);
            return new Vector3(
                vectorObject.GetValueUnsafe<float>("x"),
                vectorObject.GetValueUnsafe<float>("y"),
                vectorObject.GetValueUnsafe<float>("z")
            );
        }

        private static JObject JObjectFromVector(Vector3 vector) {
            return new JObject {
                ["x"] = Mathf.RoundToInt(vector.x),
                ["y"] = Mathf.RoundToInt(vector.y),
                ["z"] = Mathf.RoundToInt(vector.z)
            };
        }

        #endregion
    }
}