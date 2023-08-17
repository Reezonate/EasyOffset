using UnityEngine;

namespace EasyOffset;

public partial class PluginConfig {
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

    private static Vector3 _leftSaberPivotPosition = Vector3.ClampMagnitude(
        ConfigFileData.Instance.GetLeftSaberPivotPositionInMeters(),
        ConfigDefaults.MaximalPositionOffset
    );

    public static Vector3 LeftSaberPivotPosition {
        get => _leftSaberPivotPosition;
        set {
            var clamped = Vector3.ClampMagnitude(value, ConfigDefaults.MaximalPositionOffset);
            if (_leftSaberPivotPosition.Equals(clamped)) return;
            _leftSaberPivotPosition = clamped;
            ConfigFileData.Instance.SetLeftSaberPivotPositionInMeters(clamped);
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

    public static Vector3 LeftSaberRotationEuler => TransformUtils.EulerFromRotation(LeftSaberRotation);

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
}