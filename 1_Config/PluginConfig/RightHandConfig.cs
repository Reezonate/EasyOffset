using UnityEngine;

namespace EasyOffset;

internal partial class PluginConfig {
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

    private static Vector3 _rightSaberPivotPosition = Vector3.ClampMagnitude(
        ConfigFileData.Instance.GetRightSaberPivotPositionInMeters(),
        ConfigDefaults.MaximalPositionOffset
    );

    public static Vector3 RightSaberPivotPosition {
        get => _rightSaberPivotPosition;
        set {
            var clamped = Vector3.ClampMagnitude(value, ConfigDefaults.MaximalPositionOffset);
            if (_rightSaberPivotPosition.Equals(clamped)) return;
            _rightSaberPivotPosition = clamped;
            ConfigFileData.Instance.SetRightSaberPivotPositionInMeters(clamped);
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

    public static Vector3 RightSaberRotationEuler => TransformUtils.EulerFromRotation(RightSaberRotation);

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
}