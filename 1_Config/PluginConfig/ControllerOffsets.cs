using System;
using UnityEngine;

namespace EasyOffset;

internal static partial class PluginConfig {
    #region ConfigWasChangedEvent

    public static event Action ConfigWasChangedEvent;

    private static bool _notifyOnChange = true;

    private static void NotifyConfigWasChanged() {
        if (!_notifyOnChange) return;
        ConfigWasChangedEvent?.Invoke();
    }

    private static void DisableChangeEvent() {
        _notifyOnChange = false;
    }

    private static void EnableChangeEvent() {
        _notifyOnChange = true;
    }

    #endregion

    #region LeftHand

    #region SaberTranslation

    private static readonly CachedVariable<Vector3> CachedLeftSaberTranslation = new(GetLeftSaberTranslationValue);

    public static Vector3 LeftSaberTranslation => CachedLeftSaberTranslation.Value;

    private static void UpdateLeftSaberTranslation() {
        CachedLeftSaberTranslation.Value = GetLeftSaberTranslationValue();
    }

    private static Vector3 GetLeftSaberTranslationValue() {
        return TransformUtils.GetSaberTranslation(LeftSaberPivotPosition, LeftSaberRotation, LeftSaberZOffset);
    }

    #endregion

    #region SaberRotation

    private static readonly CachedVariable<Quaternion> CachedLeftSaberRotation =
        new(() => TransformUtils.RotationFromEuler(ConfigFileData.Instance.LeftSaberRotationEuler));

    public static Quaternion LeftSaberRotation {
        get => CachedLeftSaberRotation.Value;
        set {
            CachedLeftSaberRotation.Value = value;
            ConfigFileData.Instance.LeftSaberRotationEuler = TransformUtils.EulerFromRotation(value);
            UpdateLeftSaberTranslation();
            NotifyConfigWasChanged();
        }
    }

    #endregion

    #region SaberPivotPosition

    public static Vector3 LeftSaberPivotPosition {
        get => ConfigFileData.Instance.GetLeftSaberPivotPositionInMeters();
        set {
            ConfigFileData.Instance.SetLeftSaberPivotPositionInMeters(value);
            UpdateLeftSaberTranslation();
            NotifyConfigWasChanged();
        }
    }

    #endregion

    #region SaberZOffset

    private static readonly CachedVariable<float> CachedLeftSaberZOffset = new(
        () => ConfigFileData.Instance.GetLeftSaberZOffsetInMeters()
    );

    public static float LeftSaberZOffset {
        get => CachedLeftSaberZOffset.Value;
        set {
            CachedLeftSaberZOffset.Value = value;
            ConfigFileData.Instance.SetLeftSaberZOffsetInMeters(value);
            UpdateLeftSaberTranslation();
            NotifyConfigWasChanged();
        }
    }

    #endregion

    #region SaberRotationEuler

    public static Vector3 LeftSaberRotationEuler {
        get => TransformUtils.EulerFromRotation(LeftSaberRotation);
        private set => LeftSaberRotation = TransformUtils.RotationFromEuler(value);
    }

    #endregion

    #region ReferenceRotation

    public static bool LeftSaberHasReference {
        get => ConfigFileData.Instance.LeftSaberHasReference;
        private set => ConfigFileData.Instance.LeftSaberHasReference = value;
    }

    public static Quaternion LeftSaberReferenceRotation {
        get => ConfigFileData.Instance.LeftSaberReferenceRotation;
        private set => ConfigFileData.Instance.LeftSaberReferenceRotation = value;
    }

    #endregion

    #endregion

    #region RightHand

    #region SaberTranslation

    private static readonly CachedVariable<Vector3> CachedRightSaberTranslation = new(GetRightSaberTranslationValue);

    public static Vector3 RightSaberTranslation => CachedRightSaberTranslation.Value;

    private static void UpdateRightSaberTranslation() {
        CachedRightSaberTranslation.Value = GetRightSaberTranslationValue();
    }

    private static Vector3 GetRightSaberTranslationValue() {
        return TransformUtils.GetSaberTranslation(RightSaberPivotPosition, RightSaberRotation, RightSaberZOffset);
    }

    #endregion

    #region SaberRotation

    private static readonly CachedVariable<Quaternion> CachedRightSaberRotation =
        new(() => TransformUtils.RotationFromEuler(ConfigFileData.Instance.RightSaberRotationEuler));

    public static Quaternion RightSaberRotation {
        get => CachedRightSaberRotation.Value;
        set {
            CachedRightSaberRotation.Value = value;
            ConfigFileData.Instance.RightSaberRotationEuler = TransformUtils.EulerFromRotation(value);
            UpdateRightSaberTranslation();
            NotifyConfigWasChanged();
        }
    }

    #endregion

    #region SaberPivotPosition

    public static Vector3 RightSaberPivotPosition {
        get => ConfigFileData.Instance.GetRightSaberPivotPositionInMeters();
        set {
            ConfigFileData.Instance.SetRightSaberPivotPositionInMeters(value);
            UpdateRightSaberTranslation();
            NotifyConfigWasChanged();
        }
    }

    #endregion

    #region SaberZOffset

    private static readonly CachedVariable<float> CachedRightSaberZOffset = new(
        () => ConfigFileData.Instance.GetRightSaberZOffsetInMeters()
    );

    public static float RightSaberZOffset {
        get => CachedRightSaberZOffset.Value;
        set {
            CachedRightSaberZOffset.Value = value;
            ConfigFileData.Instance.SetRightSaberZOffsetInMeters(value);
            UpdateRightSaberTranslation();
            NotifyConfigWasChanged();
        }
    }

    #endregion

    #region SaberRotationEuler

    public static Vector3 RightSaberRotationEuler {
        get => TransformUtils.EulerFromRotation(RightSaberRotation);
        private set => RightSaberRotation = TransformUtils.RotationFromEuler(value);
    }

    #endregion

    #region ReferenceRotation

    public static bool RightSaberHasReference {
        get => ConfigFileData.Instance.RightSaberHasReference;
        set {
            ConfigFileData.Instance.RightSaberHasReference = value;
            NotifyConfigWasChanged();
        }
    }

    public static Quaternion RightSaberReferenceRotation {
        get => ConfigFileData.Instance.RightSaberReferenceRotation;
        set {
            ConfigFileData.Instance.RightSaberReferenceRotation = value;
            NotifyConfigWasChanged();
        }
    }

    #endregion

    #endregion

    #region Mirror

    public static void MirrorAll(Hand mirrorSource) {
        DisableChangeEvent();

        MirrorPivot(mirrorSource);
        MirrorSaberRotation(mirrorSource);
        MirrorZOffset(mirrorSource);

        EnableChangeEvent();
        NotifyConfigWasChanged();
    }

    public static void MirrorPivot(Hand mirrorSource) {
        switch (mirrorSource) {
            case Hand.Left:
                RightSaberPivotPosition = TransformUtils.MirrorVector(LeftSaberPivotPosition);
                break;
            case Hand.Right:
                LeftSaberPivotPosition = TransformUtils.MirrorVector(RightSaberPivotPosition);
                break;
            default: throw new ArgumentOutOfRangeException(nameof(mirrorSource), mirrorSource, null);
        }
    }

    public static void MirrorSaberRotation(Hand mirrorSource) {
        switch (mirrorSource) {
            case Hand.Left:
                RightSaberRotation = TransformUtils.MirrorRotation(LeftSaberRotation);
                break;
            case Hand.Right:
                LeftSaberRotation = TransformUtils.MirrorRotation(RightSaberRotation);
                break;
            default: throw new ArgumentOutOfRangeException(nameof(mirrorSource), mirrorSource, null);
        }
    }

    public static void MirrorZOffset(Hand mirrorSource) {
        switch (mirrorSource) {
            case Hand.Left:
                RightSaberZOffset = LeftSaberZOffset;
                break;
            case Hand.Right:
                LeftSaberZOffset = RightSaberZOffset;
                break;
            default: throw new ArgumentOutOfRangeException(nameof(mirrorSource), mirrorSource, null);
        }
    }

    #endregion

    #region Reset

    public static void ResetOffsets(Hand hand) {
        DisableChangeEvent();

        switch (hand) {
            case Hand.Left:
                LeftSaberPivotPosition = ConfigDefaults.LeftSaberPivotPosition;
                LeftSaberRotation = ConfigDefaults.LeftSaberRotation;
                LeftSaberZOffset = ConfigDefaults.ZOffset;
                break;
            case Hand.Right:
                RightSaberPivotPosition = ConfigDefaults.RightSaberPivotPosition;
                RightSaberRotation = ConfigDefaults.RightSaberRotation;
                RightSaberZOffset = ConfigDefaults.ZOffset;
                break;
            default: throw new ArgumentOutOfRangeException(nameof(hand), hand, null);
        }

        EnableChangeEvent();
        NotifyConfigWasChanged();
    }

    #endregion

    #region Save

    public static void SaveConfigFile() {
        ConfigFileData.Instance.Changed();
    }

    #endregion

    #region Preset

    public static void ApplyPreset(IConfigPreset preset) {
        DisableChangeEvent();

        LeftSaberPivotPosition = preset.LeftSaberPivotPosition;
        LeftSaberRotation = preset.LeftSaberRotation;
        LeftSaberZOffset = preset.LeftSaberZOffset;
        RightSaberPivotPosition = preset.RightSaberPivotPosition;
        RightSaberRotation = preset.RightSaberRotation;
        RightSaberZOffset = preset.RightSaberZOffset;

        EnableChangeEvent();
        NotifyConfigWasChanged();
    }

    public static IConfigPreset GeneratePreset() {
        return new ConfigPresetV2(
            DateTimeOffset.Now.ToUnixTimeSeconds(),
            SelectedControllerType,
            LeftSaberPivotPosition,
            LeftSaberRotationEuler,
            LeftSaberZOffset,
            RightSaberPivotPosition,
            RightSaberRotationEuler,
            RightSaberZOffset
        );
    }

    #endregion

    #region ApplyPreciseModeValues

    public static void ApplyPreciseModeValues(
        Vector3 leftPivotPosition,
        Vector3 leftRotationEuler,
        float leftZOffset,
        Vector3 rightPivotPosition,
        Vector3 rightRotationEuler,
        float rightZOffset
    ) {
        DisableChangeEvent();

        LeftSaberPivotPosition = leftPivotPosition;
        LeftSaberRotationEuler = leftRotationEuler;
        LeftSaberZOffset = leftZOffset;

        RightSaberPivotPosition = rightPivotPosition;
        RightSaberRotationEuler = rightRotationEuler;
        RightSaberZOffset = rightZOffset;

        EnableChangeEvent();
        NotifyConfigWasChanged();
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
}