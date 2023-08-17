using System;

namespace EasyOffset;

public static partial class PluginConfig {
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

    #region EnabledForDeviceless

    public static bool EnabledForDeviceless {
        get => ConfigFileData.Instance.EnabledForDeviceless;
        set => ConfigFileData.Instance.EnabledForDeviceless = value;
    }

    #endregion

    #region UI Lock

    public static event Action<bool> UILockChangedEvent;

    public static bool UILock {
        get => ConfigFileData.Instance.UILock;
        set {
            if (ConfigFileData.Instance.UILock.Equals(value)) return;
            ConfigFileData.Instance.UILock = value;
            UILockChangedEvent?.Invoke(value);
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

    private static ControllerType _selectedControllerType = ControllerTypeUtils.NameToTypeOrDefault(ConfigFileData.Instance.ControllerType);

    public static ControllerType SelectedControllerType {
        get => _selectedControllerType;
        set {
            if (_selectedControllerType.Equals(value)) return;
            _selectedControllerType = value;
            ConfigFileData.Instance.ControllerType = ControllerTypeUtils.TypeToName(value);
            ChangeButtonIfNeeded();
            ControllerTypeChangedEvent?.Invoke(value);
        }
    }

    private static void ChangeButtonIfNeeded() {
        var options = ControllerButtonUtils.GetAvailableOptions(SelectedControllerType);
        if (options.ContainsKey(AssignedButton)) return;
        AssignedButton = ControllerButtonUtils.GetDefaultButton(SelectedControllerType);
    }

    #endregion

    #region AssignedButton

    public static event Action<ControllerButton> AssignedButtonChangedEvent;

    private static ControllerButton _assignedButton = GetInitialButtonValue();

    public static ControllerButton AssignedButton {
        get => _assignedButton;
        set {
            if (_assignedButton.Equals(value)) return;
            _assignedButton = value;
            ConfigFileData.Instance.AssignedButton = ControllerButtonUtils.TypeToName(value);
            AssignedButtonChangedEvent?.Invoke(value);
        }
    }

    private static ControllerButton GetInitialButtonValue() {
        var stored = ControllerButtonUtils.NameToTypeOrDefault(SelectedControllerType, ConfigFileData.Instance.AssignedButton);
        var options = ControllerButtonUtils.GetAvailableOptions(SelectedControllerType);
        return options.ContainsKey(stored) ? stored : ControllerButtonUtils.GetDefaultButton(SelectedControllerType);
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