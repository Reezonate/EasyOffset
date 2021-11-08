using System;
using System.IO;
using System.Reflection;
using IPA.Utilities;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace EasyOffset.Configuration {
    internal static class ConfigMigration {
        #region Constructor

        static ConfigMigration() {
            PluginConfig.VRPlatformHelperChangedEvent += OnVRPlatformHelperChanged;
            OnVRPlatformHelperChanged(PluginConfig.VRPlatformHelper);
        }

        #endregion

        #region Variables

        public static float ZOffset = Defaults.ZOffset;
        private static bool _isMigrationPossible;
        private static bool _isValveController;
        private static bool _isVRModeOculus;

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
                _isMigrationPossible = false;
                _isValveController = false;
                _isVRModeOculus = false;
            } else {
                _isMigrationPossible = true;
                _isValveController = (OpenVRHelper.VRControllerManufacturerName)manufacturerName == OpenVRHelper.VRControllerManufacturerName.Valve;
            }
        }

        private static void UseOculusVRHelper() {
            _isMigrationPossible = true;
            _isValveController = false;
            _isVRModeOculus = true;
        }

        private static void UseDevicelessVRHelper() {
            _isMigrationPossible = false;
            _isValveController = false;
            _isVRModeOculus = false;
        }

        #endregion

        #region ImportFromSettings

        public static MigrationResult ImportFromSettings() {
            if (!_isMigrationPossible) return MigrationResult.DevicelessFail;

            var position = PluginConfig.MainSettingsModel.controllerPosition.value;
            var rotationEuler = PluginConfig.MainSettingsModel.controllerRotation.value;

            ConfigConversions.FromBaseGame(
                _isValveController,
                _isVRModeOculus,
                ZOffset,
                position,
                rotationEuler,
                out var leftPivotPosition,
                out var rightPivotPosition,
                out var leftSaberDirection,
                out var rightSaberDirection
            );

            PluginConfig.LeftHandZOffset = ZOffset;
            PluginConfig.LeftHandPivotPosition = leftPivotPosition;
            PluginConfig.LeftHandSaberDirection = leftSaberDirection;

            PluginConfig.RightHandZOffset = ZOffset;
            PluginConfig.RightHandPivotPosition = rightPivotPosition;
            PluginConfig.RightHandSaberDirection = rightSaberDirection;

            return MigrationResult.Success;
        }

        #endregion

        #region ImportFromSaberTailor

        public static MigrationResult ImportFromSaberTailor() {
            if (!_isMigrationPossible) return MigrationResult.DevicelessFail;

            if (!ParseSaberTailorConfig(
                out var useBaseGameAdjustmentMode,
                out var gripLeftPosition,
                out var gripRightPosition,
                out var gripLeftRotation,
                out var gripRightRotation
            )) return MigrationResult.ParseFail;

            ConfigConversions.FromTailor(
                useBaseGameAdjustmentMode,
                _isValveController,
                _isVRModeOculus,
                ZOffset,
                ZOffset,
                gripLeftPosition,
                gripRightPosition,
                gripLeftRotation,
                gripRightRotation,
                out var leftPivotPosition,
                out var rightPivotPosition,
                out var leftSaberDirection,
                out var rightSaberDirection
            );

            PluginConfig.LeftHandZOffset = ZOffset;
            PluginConfig.LeftHandPivotPosition = leftPivotPosition;
            PluginConfig.LeftHandSaberDirection = leftSaberDirection;

            PluginConfig.RightHandZOffset = ZOffset;
            PluginConfig.RightHandPivotPosition = rightPivotPosition;
            PluginConfig.RightHandSaberDirection = rightSaberDirection;

            return MigrationResult.Success;
        }

        #endregion

        #region ParseSaberTailorConfig

        private static readonly string TailorConfigPath = Path.Combine(UnityGame.UserDataPath, "SaberTailor.json");

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
                gripLeftPosition = ParseVector(jObject, "GripLeftPosition") * UnitScale;
                gripRightPosition = ParseVector(jObject, "GripRightPosition") * UnitScale;
                gripLeftRotation = ParseVector(jObject, "GripLeftRotation");
                gripRightRotation = ParseVector(jObject, "GripRightRotation");

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