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

    #region RightHand

    #region Translation

    private static readonly CachedVariable<Vector3> CachedRightHandTranslation = new(GetRightHandTranslationValue);

    public static Vector3 RightHandTranslation => CachedRightHandTranslation.Value;

    private static void UpdateRightHandTranslation() {
        CachedRightHandTranslation.Value = GetRightHandTranslationValue();
    }

    private static Vector3 GetRightHandTranslationValue() {
        return TransformUtils.GetSaberLocalPosition(RightHandPivotPosition, RightHandSaberDirection, RightHandZOffset);
    }

    #endregion

    #region Rotation

    private static readonly CachedVariable<Quaternion> CachedRightHandRotation = new(GetRightHandRotationValue);

    public static Quaternion RightHandRotation => CachedRightHandRotation.Value;

    private static void UpdateRightHandRotation() {
        CachedRightHandRotation.Value = GetRightHandRotationValue();
    }

    private static Quaternion GetRightHandRotationValue() {
        return TransformUtils.GetSaberLocalRotation(RightHandSaberDirection);
    }

    #endregion

    #region PivotPosition

    private static readonly CachedVariable<Vector3> CachedRightHandPivotPosition = new(() => new Vector3(
        ConfigFileData.Instance.RightHandPivotX,
        ConfigFileData.Instance.RightHandPivotY,
        ConfigFileData.Instance.RightHandPivotZ
    ));

    public static Vector3 RightHandPivotPosition {
        get => CachedRightHandPivotPosition.Value;
        set {
            CachedRightHandPivotPosition.Value = value;
            ConfigFileData.Instance.RightHandPivotX = value.x;
            ConfigFileData.Instance.RightHandPivotY = value.y;
            ConfigFileData.Instance.RightHandPivotZ = value.z;
            UpdateRightHandTranslation();
            NotifyConfigWasChanged();
        }
    }

    #endregion

    #region SaberDirection

    private static readonly CachedVariable<Vector3> CachedRightHandSaberDirection = new(() => new Vector3(
        ConfigFileData.Instance.RightHandSaberDirectionX,
        ConfigFileData.Instance.RightHandSaberDirectionY,
        ConfigFileData.Instance.RightHandSaberDirectionZ
    ).normalized);

    public static Vector3 RightHandSaberDirection {
        get => CachedRightHandSaberDirection.Value;
        set {
            var normalized = value.normalized;
            CachedRightHandSaberDirection.Value = normalized;
            ConfigFileData.Instance.RightHandSaberDirectionX = normalized.x;
            ConfigFileData.Instance.RightHandSaberDirectionY = normalized.y;
            ConfigFileData.Instance.RightHandSaberDirectionZ = normalized.z;
            UpdateRightHandTranslation();
            UpdateRightHandRotation();
            NotifyConfigWasChanged();
        }
    }

    #endregion

    #region ZOffset

    private static readonly CachedVariable<float> CachedRightHandZOffset = new(
        () => ConfigFileData.Instance.RightHandZOffset
    );

    public static float RightHandZOffset {
        get => CachedRightHandZOffset.Value;
        set {
            CachedRightHandZOffset.Value = value;
            ConfigFileData.Instance.RightHandZOffset = value;
            UpdateRightHandTranslation();
            NotifyConfigWasChanged();
        }
    }

    #endregion

    #endregion

    #region LeftHand

    #region Translation

    private static readonly CachedVariable<Vector3> CachedLeftHandTranslation = new(GetLeftHandTranslationValue);

    public static Vector3 LeftHandTranslation => CachedLeftHandTranslation.Value;

    private static void UpdateLeftHandTranslation() {
        CachedLeftHandTranslation.Value = GetLeftHandTranslationValue();
    }

    private static Vector3 GetLeftHandTranslationValue() {
        return TransformUtils.GetSaberLocalPosition(LeftHandPivotPosition, LeftHandSaberDirection, LeftHandZOffset);
    }

    #endregion

    #region Rotation

    private static readonly CachedVariable<Quaternion> CachedLeftHandRotation = new(GetLeftHandRotationValue);

    public static Quaternion LeftHandRotation => CachedLeftHandRotation.Value;

    private static void UpdateLeftHandRotation() {
        CachedLeftHandRotation.Value = GetLeftHandRotationValue();
    }

    private static Quaternion GetLeftHandRotationValue() {
        return TransformUtils.GetSaberLocalRotation(LeftHandSaberDirection);
    }

    #endregion

    #region PivotPosition

    private static readonly CachedVariable<Vector3> CachedLeftHandPivotPosition = new(() => new Vector3(
        ConfigFileData.Instance.LeftHandPivotX,
        ConfigFileData.Instance.LeftHandPivotY,
        ConfigFileData.Instance.LeftHandPivotZ
    ));

    public static Vector3 LeftHandPivotPosition {
        get => CachedLeftHandPivotPosition.Value;
        set {
            CachedLeftHandPivotPosition.Value = value;
            ConfigFileData.Instance.LeftHandPivotX = value.x;
            ConfigFileData.Instance.LeftHandPivotY = value.y;
            ConfigFileData.Instance.LeftHandPivotZ = value.z;
            UpdateLeftHandTranslation();
            NotifyConfigWasChanged();
        }
    }

    #endregion

    #region SaberDirection

    private static readonly CachedVariable<Vector3> CachedLeftHandSaberDirection = new(() => new Vector3(
        ConfigFileData.Instance.LeftHandSaberDirectionX,
        ConfigFileData.Instance.LeftHandSaberDirectionY,
        ConfigFileData.Instance.LeftHandSaberDirectionZ
    ).normalized);

    public static Vector3 LeftHandSaberDirection {
        get => CachedLeftHandSaberDirection.Value;
        set {
            var normalized = value.normalized;
            CachedLeftHandSaberDirection.Value = normalized;
            ConfigFileData.Instance.LeftHandSaberDirectionX = normalized.x;
            ConfigFileData.Instance.LeftHandSaberDirectionY = normalized.y;
            ConfigFileData.Instance.LeftHandSaberDirectionZ = normalized.z;
            UpdateLeftHandTranslation();
            UpdateLeftHandRotation();
            NotifyConfigWasChanged();
        }
    }

    #endregion

    #region ZOffset

    private static readonly CachedVariable<float> CachedLeftHandZOffset = new(
        () => ConfigFileData.Instance.LeftHandZOffset
    );

    public static float LeftHandZOffset {
        get => CachedLeftHandZOffset.Value;
        set {
            CachedLeftHandZOffset.Value = value;
            ConfigFileData.Instance.LeftHandZOffset = value;
            UpdateLeftHandTranslation();
            NotifyConfigWasChanged();
        }
    }

    #endregion

    #endregion

    #region Mirror

    public static void MirrorAll(Hand mirrorSource) {
        DisableChangeEvent();

        MirrorPivot(mirrorSource);
        MirrorSaberDirection(mirrorSource);
        MirrorZOffset(mirrorSource);

        EnableChangeEvent();
        NotifyConfigWasChanged();
    }

    public static void MirrorPivot(Hand mirrorSource) {
        Vector3 from;
        switch (mirrorSource) {
            case Hand.Left:
                from = LeftHandPivotPosition;
                RightHandPivotPosition = new Vector3(-from.x, from.y, from.z);
                break;
            case Hand.Right:
                from = RightHandPivotPosition;
                LeftHandPivotPosition = new Vector3(-from.x, from.y, from.z);
                break;
            default: throw new ArgumentOutOfRangeException(nameof(mirrorSource), mirrorSource, null);
        }
    }

    public static void MirrorSaberDirection(Hand mirrorSource) {
        Vector3 from;
        switch (mirrorSource) {
            case Hand.Left:
                from = LeftHandSaberDirection;
                RightHandSaberDirection = new Vector3(-from.x, from.y, from.z);
                break;
            case Hand.Right:
                from = RightHandSaberDirection;
                LeftHandSaberDirection = new Vector3(-from.x, from.y, from.z);
                break;
            default: throw new ArgumentOutOfRangeException(nameof(mirrorSource), mirrorSource, null);
        }
    }

    public static void MirrorZOffset(Hand mirrorSource) {
        switch (mirrorSource) {
            case Hand.Left:
                RightHandZOffset = LeftHandZOffset;
                break;
            case Hand.Right:
                LeftHandZOffset = RightHandZOffset;
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
                LeftHandPivotPosition = new Vector3(-Defaults.PivotX, Defaults.PivotY, Defaults.PivotZ);
                LeftHandSaberDirection = new Vector3(-Defaults.SaberDirectionX, Defaults.SaberDirectionY, Defaults.SaberDirectionZ);
                LeftHandZOffset = Defaults.ZOffset;
                break;
            case Hand.Right:
                RightHandPivotPosition = new Vector3(Defaults.PivotX, Defaults.PivotY, Defaults.PivotZ);
                RightHandSaberDirection = new Vector3(Defaults.SaberDirectionX, Defaults.SaberDirectionY, Defaults.SaberDirectionZ);
                RightHandZOffset = Defaults.ZOffset;
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

        RightHandPivotPosition = preset.RightHandPivotPosition;
        RightHandSaberDirection = preset.RightHandSaberDirection;
        RightHandZOffset = preset.RightHandZOffset;
        LeftHandPivotPosition = preset.LeftHandPivotPosition;
        LeftHandSaberDirection = preset.LeftHandSaberDirection;
        LeftHandZOffset = preset.LeftHandZOffset;

        EnableChangeEvent();
        NotifyConfigWasChanged();
    }

    public static IConfigPreset GeneratePreset() {
        return new ConfigPresetV1(
            DateTimeOffset.Now.ToUnixTimeSeconds(),
            SelectedControllerType,
            RightHandPivotPosition,
            RightHandSaberDirection,
            RightHandZOffset,
            LeftHandPivotPosition,
            LeftHandSaberDirection,
            LeftHandZOffset
        );
    }

    #endregion

    #region Precise Mode

    public static void ApplyPreciseModeConfig(
        Vector3 leftPosition,
        Vector3 leftRotation,
        float leftZOffset,
        Vector3 rightPosition,
        Vector3 rightRotation,
        float rightZOffset
    ) {
        DisableChangeEvent();

        LeftHandPivotPosition = leftPosition;
        LeftHandSaberDirection = Quaternion.Euler(leftRotation) * Vector3.forward;
        LeftHandZOffset = leftZOffset;

        RightHandPivotPosition = rightPosition;
        RightHandSaberDirection = Quaternion.Euler(rightRotation) * Vector3.forward;
        RightHandZOffset = rightZOffset;

        EnableChangeEvent();
        NotifyConfigWasChanged();
    }

    public static void GetPreciseModeConfig(
        out Vector3 leftPosition,
        out Vector3 leftRotation,
        out float leftZOffset,
        out Vector3 rightPosition,
        out Vector3 rightRotation,
        out float rightZOffset
    ) {
        var leftSpherical = TransformUtils.OrthoToSphericalDirection(LeftHandSaberDirection);
        var rightSpherical = TransformUtils.OrthoToSphericalDirection(RightHandSaberDirection);
        
        leftPosition = LeftHandPivotPosition;
        leftRotation = new Vector3(leftSpherical.x * Mathf.Rad2Deg, leftSpherical.y * Mathf.Rad2Deg, 0);
        leftZOffset = LeftHandZOffset;

        rightPosition = RightHandPivotPosition;
        rightRotation = new Vector3(rightSpherical.x * Mathf.Rad2Deg, rightSpherical.y * Mathf.Rad2Deg, 0);
        rightZOffset = RightHandZOffset;
    }

    #endregion
}