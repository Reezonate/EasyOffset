using System;
using UnityEngine;
using UnityEngine.XR;
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

        private static IVRPlatformHelper _vrPlatformHelper;

        private static void OnVRPlatformHelperChanged(IVRPlatformHelper vrPlatformHelper) {
            _vrPlatformHelper = vrPlatformHelper;
            IsMigrationPossible = _vrPlatformHelper is UnityXRHelper or OculusVRHelper;
        }

        #endregion

        #region ImportFromSettings

        public static ConfigImportResult ImportFromSettings() {
            if (!IsMigrationPossible) return ConfigImportResult.DevicelessFail;

            var position = PluginConfig.MainSettingsHandler.instance.controllerSettings.positionOffset;
            var rotationEuler = PluginConfig.MainSettingsHandler.instance.controllerSettings.rotationOffset;

            ConfigConversions.FromBaseGame(
                _vrPlatformHelper,
                ZOffset,
                position,
                rotationEuler,
                out var leftPivotPosition,
                out var rightPivotPosition,
                out var leftSaberRotation,
                out var rightSaberRotation
            );

            PluginConfig.CreateUndoStep("Import From Settings");

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

            PluginConfig.CreateUndoStep("Universal Import");

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
            var target = vrController.transform.Find("MenuHandle");
            if (target == null) target = vrController.transform;

            var saberWorldPosition = target.position;
            var saberWorldRotation = target.rotation;

            if (!PluginConfig.VRPlatformHelper.GetNodePose(vrController.node, vrController.nodeIdx, out var controllerWorldPosition, out var controllerWorldRotation)) {
                pivotPosition = Vector3.zero;
                saberRotation = Quaternion.identity;
                return false;
            }

            TransformUtils.ApplyRoomOffset(ref controllerWorldPosition, ref controllerWorldRotation);

            ConfigConversions.Universal(
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
            XRNode xrNode;

            switch (hand) {
                case Hand.Left:
                    translation = TransformUtils.MirrorVector(PluginConfig.LeftSaberTranslation);
                    rotation = TransformUtils.MirrorRotation(PluginConfig.LeftSaberRotation);
                    xrNode = XRNode.LeftHand;
                    break;
                case Hand.Right:
                    translation = PluginConfig.RightSaberTranslation;
                    rotation = PluginConfig.RightSaberRotation;
                    xrNode = XRNode.RightHand;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(hand), hand, null);
            }

            ConfigConversions.ToBaseGame(
                _vrPlatformHelper,
                xrNode,
                translation,
                rotation,
                out var position,
                out var rotationEuler
            );

            var beforePosition = PluginConfig.MainSettingsHandler.instance.controllerSettings.positionOffset;
            var beforeRotation = PluginConfig.MainSettingsHandler.instance.controllerSettings.rotationOffset;

            PluginConfig.CreateUndoStep(
                $"Export To Settings",
                () => {
                    PluginConfig.MainSettingsHandler.instance.controllerSettings.positionOffset = beforePosition;
                    PluginConfig.MainSettingsHandler.instance.controllerSettings.rotationOffset = beforeRotation;
                },
                () => {
                    PluginConfig.MainSettingsHandler.instance.controllerSettings.positionOffset = position;
                    PluginConfig.MainSettingsHandler.instance.controllerSettings.rotationOffset = rotationEuler;
                }
            );

            PluginConfig.MainSettingsHandler.instance.controllerSettings.positionOffset = position;
            PluginConfig.MainSettingsHandler.instance.controllerSettings.rotationOffset = rotationEuler;

            return ConfigExportResult.Success;
        }

        #endregion
    }
}