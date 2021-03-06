using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal partial class ModPanelUI {
    #region Events

    private Vector3SO _roomCenterSO;

    private void SubscribeToRoomOffsetEvents() {
        PluginConfig.MainSettingsModelChangedEvent += OnMainSettingsModelChanged;
        RoomOffsetAdjustmentModeManager.OnHmdPositionChangedEvent += OnHmdPositionChange;
    }

    private void OnMainSettingsModelChanged(MainSettingsModelSO mainSettingsModel) {
        if (_roomCenterSO != null) {
            _roomCenterSO.didChangeEvent -= OnRoomCenterChanged;
        }

        _roomCenterSO = mainSettingsModel.roomCenter;
        _roomCenterSO.didChangeEvent += OnRoomCenterChanged;
        OnRoomCenterChanged();
    }

    private void OnRoomCenterChanged() {
        var tmp = _roomCenterSO.value;
        RoomXText = BuildRoomOffsetValueString("Room X", tmp.x, 40);
        RoomYText = BuildRoomOffsetValueString("Room Y", tmp.y, 40);
        RoomZText = BuildRoomOffsetValueString("Room Z", tmp.z, 40);
    }

    private void OnHmdPositionChange(Vector3 position) {
        HmdXText = BuildRoomOffsetValueString("HMD X", position.x, 35);
        HmdYText = BuildRoomOffsetValueString("HMD Y", position.y, 35);
        HmdZText = BuildRoomOffsetValueString("HMD Z", position.z, 35);
    }

    private static string BuildRoomOffsetValueString(string prefix, float value, int offsetPercent) {
        return $"{prefix} <pos={offsetPercent}%><mspace=0.43em>{((value >= 0) ? " " : "")}{(value * 100):F1}</mspace> cm";
    }

    #endregion

    #region Active

    private bool _roomOffsetPanelActive;

    [UIValue("room-offset-panel-active")]
    [UsedImplicitly]
    private bool RoomOffsetPanelActive {
        get => _roomOffsetPanelActive;
        set {
            _roomOffsetPanelActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Head

    private string _hmdXText = "";

    [UIValue("hmd-x-text")]
    [UsedImplicitly]
    private string HmdXText {
        get => _hmdXText;
        set {
            _hmdXText = value;
            NotifyPropertyChanged();
        }
    }

    private string _hmdYText = "";

    [UIValue("hmd-y-text")]
    [UsedImplicitly]
    private string HmdYText {
        get => _hmdYText;
        set {
            _hmdYText = value;
            NotifyPropertyChanged();
        }
    }

    private string _hmdZText = "";

    [UIValue("hmd-z-text")]
    [UsedImplicitly]
    private string HmdZText {
        get => _hmdZText;
        set {
            _hmdZText = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Room X

    private string _roomXText = "";

    [UIValue("room-x-text")]
    [UsedImplicitly]
    private string RoomXText {
        get => _roomXText;
        set {
            _roomXText = value;
            NotifyPropertyChanged();
        }
    }

    [UIValue("room-x-toggle-value")] [UsedImplicitly]
    private bool _roomXToggleValue = PluginConfig.AllowRoomXChange;

    [UIAction("room-x-toggle-on-change")]
    [UsedImplicitly]
    private void RoomXToggleOnChange(bool value) {
        PluginConfig.AllowRoomXChange = value;
    }

    [UIAction("room-x-reset-on-click")]
    [UsedImplicitly]
    private void RoomXResetOnClick() {
        var tmp = PluginConfig.MainSettingsModel.roomCenter.value;
        PluginConfig.MainSettingsModel.roomCenter.value = new Vector3(0, tmp.y, tmp.z);
    }

    #endregion

    #region Room Y

    private string _roomYText = "";

    [UIValue("room-y-text")]
    [UsedImplicitly]
    private string RoomYText {
        get => _roomYText;
        set {
            _roomYText = value;
            NotifyPropertyChanged();
        }
    }

    [UIValue("room-y-toggle-value")] [UsedImplicitly]
    private bool _roomYToggleValue = PluginConfig.AllowRoomYChange;

    [UIAction("room-y-toggle-on-change")]
    [UsedImplicitly]
    private void RoomYToggleOnChange(bool value) {
        PluginConfig.AllowRoomYChange = value;
    }

    [UIAction("room-y-reset-on-click")]
    [UsedImplicitly]
    private void RoomYResetOnClick() {
        var tmp = PluginConfig.MainSettingsModel.roomCenter.value;
        PluginConfig.MainSettingsModel.roomCenter.value = new Vector3(tmp.x, 0, tmp.z);
    }

    #endregion

    #region Room Z

    private string _roomZText = "";

    [UIValue("room-z-text")]
    [UsedImplicitly]
    private string RoomZText {
        get => _roomZText;
        set {
            _roomZText = value;
            NotifyPropertyChanged();
        }
    }

    [UIValue("room-z-toggle-value")] [UsedImplicitly]
    private bool _roomZToggleValue = PluginConfig.AllowRoomZChange;

    [UIAction("room-z-toggle-on-change")]
    [UsedImplicitly]
    private void RoomZToggleOnChange(bool value) {
        PluginConfig.AllowRoomZChange = value;
    }

    [UIAction("room-z-reset-on-click")]
    [UsedImplicitly]
    private void RoomZResetOnClick() {
        var tmp = PluginConfig.MainSettingsModel.roomCenter.value;
        PluginConfig.MainSettingsModel.roomCenter.value = new Vector3(tmp.x, tmp.y, 0);
    }

    #endregion
}