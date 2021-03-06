using System;
using System.IO;
using System.Reflection;
using IPA.Utilities;
using Newtonsoft.Json.Linq;
using UnityEngine;

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
                IsValveController = (OpenVRHelper.VRControllerManufacturerName) manufacturerName == OpenVRHelper.VRControllerManufacturerName.Valve;
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

        #region ImportFromSaberTailor

        public static ConfigImportResult ImportFromSaberTailor() {
            if (!IsMigrationPossible) return ConfigImportResult.DevicelessFail;

            if (!ParseSaberTailorConfig(
                    out var useBaseGameAdjustmentMode,
                    out var gripLeftPosition,
                    out var gripRightPosition,
                    out var gripLeftRotation,
                    out var gripRightRotation
                )) return ConfigImportResult.ParseFail;

            ConfigConversions.FromTailor(
                useBaseGameAdjustmentMode,
                IsValveController,
                IsVRModeOculus,
                ZOffset,
                ZOffset,
                gripLeftPosition,
                gripRightPosition,
                gripLeftRotation,
                gripRightRotation,
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

        #region ExportToSettings

        public static ConfigExportResult ExportToSettings() {
            if (!IsMigrationPossible) return ConfigExportResult.DevicelessFail;

            ConfigConversions.ToBaseGame(
                IsValveController,
                IsVRModeOculus,
                PluginConfig.RightSaberTranslation,
                PluginConfig.RightSaberRotation,
                out var position,
                out var rotationEuler
            );

            PluginConfig.MainSettingsModel.controllerPosition.value = position;
            PluginConfig.MainSettingsModel.controllerRotation.value = rotationEuler;

            return ConfigExportResult.Success;
        }

        #endregion

        #region ExportToSaberTailor

        public static ConfigExportResult ExportToSaberTailor(bool useBaseGameAdjustmentMode = true) {
            if (!IsMigrationPossible) return ConfigExportResult.DevicelessFail;

            ConfigConversions.ToTailor(
                useBaseGameAdjustmentMode,
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
                useBaseGameAdjustmentMode,
                gripLeftPosition,
                gripRightPosition,
                gripLeftRotation,
                gripRightRotation
            );

            return result ? ConfigExportResult.Success : ConfigExportResult.WriteFail;
        }

        #endregion

        #region SaberTailorConfig

        private static readonly string TailorConfigPath = Path.Combine(UnityGame.UserDataPath, "SaberTailor.json");
        private static readonly string TailorExportConfigPath = Path.Combine(UnityGame.UserDataPath, "SaberTailor.EasyOffsetExported.json");

        private const float UnitScale = 0.001f;

        private static bool ParseSaberTailorConfig(
            out bool useBaseGameAdjustmentMode,
            out Vector3 gripLeftPosition,
            out Vector3 gripRightPosition,
            out Vector3 gripLeftRotation,
            out Vector3 gripRightRotation
        ) {
            try {
                var rawFileString = File.ReadAllText(TailorConfigPath);
                var jObject = JObject.Parse(rawFileString);

                useBaseGameAdjustmentMode = jObject.GetValue("UseBaseGameAdjustmentMode", StringComparison.OrdinalIgnoreCase)!.Value<bool>();
                gripLeftPosition = VectorFromJObject(jObject, "GripLeftPosition") * UnitScale;
                gripRightPosition = VectorFromJObject(jObject, "GripRightPosition") * UnitScale;
                gripLeftRotation = VectorFromJObject(jObject, "GripLeftRotation");
                gripRightRotation = VectorFromJObject(jObject, "GripRightRotation");

                return true;
            } catch (Exception) {
                useBaseGameAdjustmentMode = false;
                gripLeftPosition = Vector3.zero;
                gripLeftRotation = Vector3.zero;
                gripRightPosition = Vector3.zero;
                gripRightRotation = Vector3.zero;
                return false;
            }
        }

        private static bool WriteSaberTailorConfig(
            bool useBaseGameAdjustmentMode,
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
                    ["UseBaseGameAdjustmentMode"] = useBaseGameAdjustmentMode,
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
            var vectorObject = jObject[key]!.Value<JObject>();
            return new Vector3(
                vectorObject.GetValue("x", StringComparison.OrdinalIgnoreCase)!.Value<float>(),
                vectorObject.GetValue("y", StringComparison.OrdinalIgnoreCase)!.Value<float>(),
                vectorObject.GetValue("z", StringComparison.OrdinalIgnoreCase)!.Value<float>()
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