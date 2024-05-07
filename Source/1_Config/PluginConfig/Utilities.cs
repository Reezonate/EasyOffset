using System;
using BeatSaber.GameSettings;

namespace EasyOffset;

public static partial class PluginConfig {
    #region VRPlatformHelper

    public static event Action<IVRPlatformHelper> VRPlatformHelperChangedEvent;

    private static IVRPlatformHelper _vrPlatformHelper;

    public static bool IsDeviceless { get; private set; } = false;

    public static IVRPlatformHelper VRPlatformHelper {
        get => _vrPlatformHelper;
        set {
            _vrPlatformHelper = value;
            IsDeviceless = value.vrPlatformSDK switch {
                VRPlatformSDK.OpenXR => false,
                VRPlatformSDK.Oculus => false,
                _ => true
            };
            VRPlatformHelperChangedEvent?.Invoke(value);
        }
    }

    #endregion

    #region MainSettingsModel

    public static event Action<MainSettingsHandler> MainSettingsHandlerChangedEvent;

    private static MainSettingsHandler _mainSettingsHandler;

    public static MainSettingsHandler MainSettingsHandler {
        get => _mainSettingsHandler;
        set {
            _mainSettingsHandler = value;
            MainSettingsHandlerChangedEvent?.Invoke(value);
        }
    }

    #endregion

    #region IsInMainMenu

    public static event Action<bool> IsInMainMenuChangedEvent;

    private static bool _isInMainMenu;

    public static bool IsInMainMenu {
        get => _isInMainMenu;
        set {
            if (_isInMainMenu == value) return;
            _isInMainMenu = value;
            IsInMainMenuChangedEvent?.Invoke(value);
        }
    }

    #endregion

    #region IsModPanelVisible

    public static event Action<bool> IsModPanelVisibleChangedEvent;

    private static bool _isModPanelVisible;

    public static bool IsModPanelVisible {
        get => _isModPanelVisible;
        set {
            if (_isModPanelVisible == value) return;
            _isModPanelVisible = value;
            IsModPanelVisibleChangedEvent?.Invoke(value);
        }
    }

    #endregion

    #region Smoothing

    public static bool SmoothingEnabled { get; set; }
    public static float PositionalSmoothing { get; set; }
    public static float RotationalSmoothing { get; set; }

    #endregion
}