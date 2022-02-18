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

    private static Quaternion _leftSaberRotation = TransformUtils.RotationFromEuler(ConfigFileData.Instance.LeftSaberRotationEuler);

    public static Quaternion LeftSaberRotation {
        get => _leftSaberRotation;
        set {
            if (_leftSaberRotation.Equals(value)) return;
            _leftSaberRotation = value;
            ConfigFileData.Instance.LeftSaberRotationEuler = TransformUtils.EulerFromRotation(value);
            UpdateLeftSaberTranslation();
            NotifyConfigWasChanged();
        }
    }

    #endregion

    #region SaberPivotPosition

    private static Vector3 _leftSaberPivotPosition = ConfigFileData.Instance.GetLeftSaberPivotPositionInMeters();

    public static Vector3 LeftSaberPivotPosition {
        get => _leftSaberPivotPosition;
        set {
            if (_leftSaberPivotPosition.Equals(value)) return;
            _leftSaberPivotPosition = value;
            ConfigFileData.Instance.SetLeftSaberPivotPositionInMeters(value);
            UpdateLeftSaberTranslation();
            NotifyConfigWasChanged();
        }
    }

    #endregion

    #region SaberZOffset

    private static float _leftSaberZOffset = ConfigFileData.Instance.GetLeftSaberZOffsetInMeters();

    public static float LeftSaberZOffset {
        get => _leftSaberZOffset;
        set {
            if (_leftSaberZOffset.Equals(value)) return;
            _leftSaberZOffset = value;
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

    #region HasReference

    public static bool LeftSaberHasReference {
        get => ConfigFileData.Instance.LeftSaberHasReference;
        private set => ConfigFileData.Instance.LeftSaberHasReference = value;
    }

    #endregion

    #region ReferenceRotation

    private static Quaternion _leftSaberReferenceRotation = ConfigFileData.Instance.LeftSaberReference.ToUnityQuaternion();

    public static Quaternion LeftSaberReferenceRotation {
        get => _leftSaberReferenceRotation;
        private set {
            _leftSaberReferenceRotation = value;
            ConfigFileData.Instance.LeftSaberReference = ConfigFileQuaternion.FromUnityQuaternion(value);
        }
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

    private static Quaternion _rightSaberRotation = TransformUtils.RotationFromEuler(ConfigFileData.Instance.RightSaberRotationEuler);

    public static Quaternion RightSaberRotation {
        get => _rightSaberRotation;
        set {
            if (_rightSaberRotation.Equals(value)) return;
            _rightSaberRotation = value;
            ConfigFileData.Instance.RightSaberRotationEuler = TransformUtils.EulerFromRotation(value);
            UpdateRightSaberTranslation();
            NotifyConfigWasChanged();
        }
    }

    #endregion

    #region SaberPivotPosition

    private static Vector3 _rightSaberPivotPosition = ConfigFileData.Instance.GetRightSaberPivotPositionInMeters();

    public static Vector3 RightSaberPivotPosition {
        get => _rightSaberPivotPosition;
        set {
            if (_rightSaberPivotPosition.Equals(value)) return;
            _rightSaberPivotPosition = value;
            ConfigFileData.Instance.SetRightSaberPivotPositionInMeters(value);
            UpdateRightSaberTranslation();
            NotifyConfigWasChanged();
        }
    }

    #endregion

    #region SaberZOffset

    private static float _rightSaberZOffset = ConfigFileData.Instance.GetRightSaberZOffsetInMeters();

    public static float RightSaberZOffset {
        get => _rightSaberZOffset;
        set {
            if (_rightSaberZOffset.Equals(value)) return;
            _rightSaberZOffset = value;
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

    #region HasReference

    public static bool RightSaberHasReference {
        get => ConfigFileData.Instance.RightSaberHasReference;
        private set => ConfigFileData.Instance.RightSaberHasReference = value;
    }

    #endregion

    #region ReferenceRotation

    private static Quaternion _rightSaberReferenceRotation = ConfigFileData.Instance.RightSaberReference.ToUnityQuaternion();

    public static Quaternion RightSaberReferenceRotation {
        get => _rightSaberReferenceRotation;
        private set {
            _rightSaberReferenceRotation = value;
            ConfigFileData.Instance.RightSaberReference = ConfigFileQuaternion.FromUnityQuaternion(value);
        }
    }

    #endregion

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

    #region Save

    public static void SaveConfigFile() {
        ConfigFileData.Instance.Changed();
    }

    #endregion

    #region Preset

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

    #region ApplyPreciseModeValues

    public static void ApplyPreciseModeValues(
        Vector3 leftPivotPosition,
        Vector3 leftRotationEuler,
        float leftZOffset,
        Vector3 rightPivotPosition,
        Vector3 rightRotationEuler,
        float rightZOffset
    ) {
        DisableConfigChangeEvent();

        LeftSaberPivotPosition = leftPivotPosition;
        LeftSaberRotationEuler = leftRotationEuler;
        LeftSaberZOffset = leftZOffset;

        RightSaberPivotPosition = rightPivotPosition;
        RightSaberRotationEuler = rightRotationEuler;
        RightSaberZOffset = rightZOffset;

        EnableConfigChangeEvent();
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