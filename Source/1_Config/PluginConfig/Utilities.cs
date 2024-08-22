using System;
using UnityEngine;

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

    #region GameSettings

    public static SettingsManager MainSettingsManager { private get; set; } = SettingsManager.CreateUninitialized();

    public static SettingsApplicatorSO SettingsApplicator { private get; set; }

    public static float BaseGameAudioVolume => MainSettingsManager.settings.audio.volume;

    public static Vector3 BaseGameRoomCenter {
        get => MainSettingsManager.settings.room.center;
        set {
            MainSettingsManager.settings.room.center = value;
            ApplyRoomSettings();
        }
    }

    public static float BaseGameRoomRotation {
        get => MainSettingsManager.settings.room.rotation;
        set {
            MainSettingsManager.settings.room.rotation = value;
            ApplyRoomSettings();
        }
    }

    public static Vector3 BaseGameControllerPosition {
        get => MainSettingsManager.settings.controller.position;
        set {
            MainSettingsManager.settings.controller.position = value;
            ApplyControllerSettings();
        }
    }

    public static Vector3 BaseGameControllerRotation {
        get => MainSettingsManager.settings.controller.rotation;
        set {
            MainSettingsManager.settings.controller.rotation = value;
            ApplyControllerSettings();
        }
    }

    private static void ApplyRoomSettings() {
        SettingsApplicator?.NotifyRoomTransformOffsetWasUpdated();
    }

    private static void ApplyControllerSettings() {
        VRPlatformHelper?.RefreshControllersReference();
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