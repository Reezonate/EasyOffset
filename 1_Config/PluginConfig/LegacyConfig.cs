using System;
using UnityEngine;

namespace EasyOffset;

internal static partial class PluginConfig {
    #region ApplyLegacyConfig

    public static void ApplyLegacyConfig(
        Vector3 leftHandPosition,
        Vector3 leftHandRotation,
        Vector3 rightHandPosition,
        Vector3 rightHandRotation
    ) {
        ConfigConversions.FromTailor(
            true,
            ConfigMigration.IsValveController,
            ConfigMigration.IsVRModeOculus,
            LeftHandZOffset,
            RightHandZOffset,
            leftHandPosition,
            rightHandPosition,
            leftHandRotation,
            rightHandRotation,
            out var leftPivotPosition,
            out var rightPivotPosition,
            out var leftSaberDirection,
            out var rightSaberDirection
        );

        DisableChangeEvent();
        LeftHandPivotPosition = leftPivotPosition;
        RightHandPivotPosition = rightPivotPosition;
        LeftHandSaberDirection = leftSaberDirection;
        RightHandSaberDirection = rightSaberDirection;
        EnableChangeEvent();
        NotifyConfigWasChanged();
    }

    #endregion

    #region GetLegacyConfig

    public static void GetLegacyConfig(
        out Vector3 leftHandPosition,
        out Vector3 leftHandRotation,
        out Vector3 rightHandPosition,
        out Vector3 rightHandRotation
    ) {
        ConfigConversions.ToTailor(
            true,
            ConfigMigration.IsValveController,
            ConfigMigration.IsVRModeOculus,
            LeftHandTranslation,
            LeftHandRotation,
            RightHandTranslation,
            RightHandRotation,
            out leftHandPosition,
            out rightHandPosition,
            out leftHandRotation,
            out rightHandRotation
        );
    }

    #endregion
}