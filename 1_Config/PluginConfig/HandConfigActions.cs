using System;
using UnityEngine;

namespace EasyOffset;

internal static partial class PluginConfig {
    #region ConfigWasChangedEvent

    public static event Action ConfigWasChangedEvent;

    private static bool _disableChangeEvent;

    private static void NotifyConfigWasChanged() {
        if (_disableChangeEvent) return;
        ConfigWasChangedEvent?.Invoke();
    }

    private static void DisableConfigChangeEvent() {
        _disableChangeEvent = true;
    }

    private static void EnableConfigChangeEvent() {
        _disableChangeEvent = false;
    }

    #endregion

    #region Mirror

    public static void Mirror(Hand mirrorSource, bool mirrorPosition, bool mirrorRotation) {
        DisableConfigChangeEvent();

        switch (mirrorSource) {
            case Hand.Left:
                if (mirrorPosition) {
                    RightSaberPivotPosition = TransformUtils.MirrorVector(LeftSaberPivotPosition);
                    RightSaberZOffset = LeftSaberZOffset;
                }

                if (mirrorRotation) {
                    RightSaberRotation = TransformUtils.MirrorRotation(LeftSaberRotation);
                }

                break;
            case Hand.Right:
                if (mirrorPosition) {
                    LeftSaberPivotPosition = TransformUtils.MirrorVector(RightSaberPivotPosition);
                    LeftSaberZOffset = RightSaberZOffset;
                }

                if (mirrorRotation) {
                    LeftSaberRotation = TransformUtils.MirrorRotation(RightSaberRotation);
                }

                break;
            default: throw new ArgumentOutOfRangeException(nameof(mirrorSource), mirrorSource, null);
        }

        EnableConfigChangeEvent();
        NotifyConfigWasChanged();
    }

    #endregion

    #region Reset

    public static void ResetOffsets(Hand hand, bool resetPosition, bool resetRotation, bool resetReference) {
        DisableConfigChangeEvent();

        switch (hand) {
            case Hand.Left:
                if (resetPosition) {
                    LeftSaberPivotPosition = ConfigDefaults.LeftSaberPivotPosition;
                    LeftSaberZOffset = ConfigDefaults.ZOffset;
                }

                if (resetRotation) {
                    LeftSaberRotation = ConfigDefaults.LeftSaberRotation;
                }

                if (resetReference) {
                    LeftSaberHasReference = ConfigDefaults.LeftSaberHasReference;
                    LeftSaberReferenceRotation = ConfigDefaults.LeftSaberReferenceRotation;
                }

                break;
            case Hand.Right:
                if (resetPosition) {
                    RightSaberPivotPosition = ConfigDefaults.RightSaberPivotPosition;
                    RightSaberZOffset = ConfigDefaults.ZOffset;
                }

                if (resetRotation) {
                    RightSaberRotation = ConfigDefaults.RightSaberRotation;
                }

                if (resetReference) {
                    RightSaberHasReference = ConfigDefaults.RightSaberHasReference;
                    RightSaberReferenceRotation = ConfigDefaults.RightSaberReferenceRotation;
                }

                break;
            default: throw new ArgumentOutOfRangeException(nameof(hand), hand, null);
        }

        EnableConfigChangeEvent();
        NotifyConfigWasChanged();
    }

    #endregion

    #region SaveConfigFile

    public static void SaveConfigFile() {
        ConfigFileData.Instance.Changed();
    }

    #endregion

    #region ApplyPreset

    public static void ApplyPreset(IConfigPreset preset) {
        DisableConfigChangeEvent();

        LeftSaberPivotPosition = preset.LeftSaberPivotPosition;
        LeftSaberRotation = preset.LeftSaberRotation;
        LeftSaberZOffset = preset.LeftSaberZOffset;
        LeftSaberHasReference = preset.LeftSaberHasReference;
        LeftSaberReferenceRotation = preset.LeftSaberReferenceRotation;

        RightSaberPivotPosition = preset.RightSaberPivotPosition;
        RightSaberRotation = preset.RightSaberRotation;
        RightSaberZOffset = preset.RightSaberZOffset;
        RightSaberHasReference = preset.RightSaberHasReference;
        RightSaberReferenceRotation = preset.RightSaberReferenceRotation;

        EnableConfigChangeEvent();
        NotifyConfigWasChanged();
    }

    #endregion

    #region GeneratePreset

    public static IConfigPreset GeneratePreset() {
        return new ConfigPresetV2(
            DateTimeOffset.Now.ToUnixTimeSeconds(),
            SelectedControllerType,
            LeftSaberPivotPosition,
            LeftSaberRotationEuler,
            LeftSaberZOffset,
            LeftSaberHasReference,
            LeftSaberReferenceRotation,
            RightSaberPivotPosition,
            RightSaberRotationEuler,
            RightSaberZOffset,
            RightSaberHasReference,
            RightSaberReferenceRotation
        );
    }

    #endregion

    #region SetSaberOffsets

    public static void SetSaberOffsets(
        Vector3 leftPivotPosition,
        Quaternion leftRotation,
        float leftZOffset,
        Vector3 rightPivotPosition,
        Quaternion rightRotation,
        float rightZOffset
    ) {
        DisableConfigChangeEvent();

        LeftSaberPivotPosition = leftPivotPosition;
        LeftSaberRotation = leftRotation;
        LeftSaberZOffset = leftZOffset;

        RightSaberPivotPosition = rightPivotPosition;
        RightSaberRotation = rightRotation;
        RightSaberZOffset = rightZOffset;

        EnableConfigChangeEvent();
        NotifyConfigWasChanged();
    }

    #endregion

    #region ResetReference

    public static void ResetLeftSaberReference() {
        SetLeftSaberReference(ConfigDefaults.LeftSaberHasReference, ConfigDefaults.LeftSaberReferenceRotation);
    }

    public static void ResetRightSaberReference() {
        SetRightSaberReference(ConfigDefaults.RightSaberHasReference, ConfigDefaults.RightSaberReferenceRotation);
    }

    #endregion

    #region SetReference

    public static void SetLeftSaberReference(bool hasReference, Quaternion referenceRotation) {
        LeftSaberHasReference = hasReference;
        LeftSaberReferenceRotation = referenceRotation;
        NotifyConfigWasChanged();
    }

    public static void SetRightSaberReference(bool hasReference, Quaternion referenceRotation) {
        RightSaberHasReference = hasReference;
        RightSaberReferenceRotation = referenceRotation;
        NotifyConfigWasChanged();
    }

    #endregion

    #region AlignReference

    public static void AlignLeftReferenceToCurrent() {
        if (!LeftSaberHasReference) return;
        LeftSaberReferenceRotation = TransformUtils.AlignForwardVectors(
            LeftSaberReferenceRotation,
            LeftSaberRotation
        );
        NotifyConfigWasChanged();
    }

    public static void AlignRightReferenceToCurrent() {
        if (!RightSaberHasReference) return;
        RightSaberReferenceRotation = TransformUtils.AlignForwardVectors(
            RightSaberReferenceRotation,
            RightSaberRotation
        );
        NotifyConfigWasChanged();
    }

    #endregion
}