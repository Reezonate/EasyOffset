using System;

namespace EasyOffset;

internal static partial class PluginConfig {
    #region MinimalWarningLevel

    public static event Action<WarningLevel> MinimalWarningLevelChangedEvent;

    private static readonly CachedVariable<WarningLevel> CachedMinimalWarningLevel = new(
        () => WarningLevelUtils.NameToTypeOrDefault(ConfigFileData.Instance.MinimalWarningLevel)
    );

    public static WarningLevel MinimalWarningLevel {
        get => CachedMinimalWarningLevel.Value;
        set {
            if (CachedMinimalWarningLevel.Value == value) return;
            CachedMinimalWarningLevel.Value = value;
            ConfigFileData.Instance.MinimalWarningLevel = WarningLevelUtils.TypeToName(value);
            MinimalWarningLevelChangedEvent?.Invoke(value);
        }
    }

    #endregion

    #region Enabled

    public static event Action<bool> OnEnabledChange;

    private static readonly CachedVariable<bool> CachedEnabled = new(
        () => ConfigFileData.Instance.Enabled
    );

    public static bool Enabled {
        get => CachedEnabled.Value;
        set {
            if (CachedEnabled.Value == value) return;
            CachedEnabled.Value = value;
            ConfigFileData.Instance.Enabled = value;
            OnEnabledChange?.Invoke(value);
        }
    }

    #endregion

    #region UI Lock

    private static readonly CachedVariable<bool> CachedUILock = new(
        () => ConfigFileData.Instance.UILock
    );

    public static bool UILock {
        get => CachedUILock.Value;
        set {
            CachedUILock.Value = value;
            ConfigFileData.Instance.UILock = value;
        }
    }

    #endregion

    #region RoomOffset Axles

    public static bool AllowRoomXChange {
        get => ConfigFileData.Instance.AllowRoomXChange;
        set => ConfigFileData.Instance.AllowRoomXChange = value;
    }

    public static bool AllowRoomYChange {
        get => ConfigFileData.Instance.AllowRoomYChange;
        set => ConfigFileData.Instance.AllowRoomYChange = value;
    }

    public static bool AllowRoomZChange {
        get => ConfigFileData.Instance.AllowRoomZChange;
        set => ConfigFileData.Instance.AllowRoomZChange = value;
    }

    #endregion

    #region HideControllers

    public static event Action<bool> HideControllersChangedEvent;

    private static bool _hideControllers = ConfigFileData.Instance.HideControllers;

    public static bool HideControllers {
        get => _hideControllers;
        set {
            if (_hideControllers == value) return;
            _hideControllers = value;
            ConfigFileData.Instance.HideControllers = value;
            HideControllersChangedEvent?.Invoke(value);
        }
    }

    #endregion

    #region AssignedButton

    private static readonly CachedVariable<ControllerButton> CachedAssignedButton = new(
        () => ControllerButtonUtils.NameToTypeOrDefault(SelectedControllerType, ConfigFileData.Instance.AssignedButton)
    );

    public static ControllerButton AssignedButton {
        get => CachedAssignedButton.Value;
        set {
            CachedAssignedButton.Value = value;
            ConfigFileData.Instance.AssignedButton = ControllerButtonUtils.TypeToName(value);
        }
    }

    #endregion

    #region AdjustmentMode

    public static event Action<AdjustmentMode> AdjustmentModeChangedEvent;

    private static AdjustmentMode _adjustmentMode = AdjustmentMode.None;

    public static AdjustmentMode AdjustmentMode {
        get => _adjustmentMode;
        set {
            _adjustmentMode = value;
            AdjustmentModeChangedEvent?.Invoke(value);
        }
    }

    #endregion

    #region SelectedControllerType

    public static event Action<ControllerType> ControllerTypeChangedEvent;

    private static readonly CachedVariable<ControllerType> CachedSelectedControllerType = new(
        () => ControllerTypeUtils.NameToTypeOrDefault(ConfigFileData.Instance.DisplayControllerType)
    );

    public static ControllerType SelectedControllerType {
        get => CachedSelectedControllerType.Value;
        set {
            CachedSelectedControllerType.Value = value;
            ConfigFileData.Instance.DisplayControllerType = ControllerTypeUtils.TypeToName(value);
            ControllerTypeChangedEvent?.Invoke(value);
        }
    }

    #endregion

    #region UseFreeHand

    private static readonly CachedVariable<bool> CachedUseFreeHand = new(
        () => ConfigFileData.Instance.UseFreeHand
    );

    public static bool UseFreeHand {
        get => CachedUseFreeHand.Value;
        set {
            CachedUseFreeHand.Value = value;
            ConfigFileData.Instance.UseFreeHand = value;
        }
    }

    #endregion
}