using System;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal partial class ModPanelUI : NotifiableSingleton<ModPanelUI> {
    #region Constructor

    public ModPanelUI() {
        SubscribeToAssignedButtonEvents();
        SubscribeToBenchmarkEvents();
        SubscribeToRoomOffsetEvents();
        SubscribeToWarningEvents();
        SubscribeToPreciseModeEvents();
        SubscribeToSmoothingEvents();
    }

    #endregion

    #region Update

    private void Update() {
        UpdatePanelVisibility();
        SmoothingUpdate();
    }

    #endregion

    #region Visibility

    [UIObject("root")] [UsedImplicitly] private GameObject _root;

    private void UpdatePanelVisibility() {
        var isVisible = (_root != null) && _root.activeInHierarchy;
        PluginConfig.IsModPanelVisible = isVisible;
    }

    #endregion

    #region Flow

    private void UpdateBenchmarkPanel(bool hasResults) {
        BenchmarkGuideActive = !hasResults;
        BenchmarkResultsActive = hasResults;
    }

    private void GoToMainPage() {
        MainPageActive = true;
        PresetsBrowserActive = false;

        switch (PluginConfig.AdjustmentMode) {
            case AdjustmentMode.None:
            case AdjustmentMode.Basic:
            case AdjustmentMode.Position:
            case AdjustmentMode.Rotation:
            case AdjustmentMode.RotationAuto:
                UseFreeHandActive = true;
                HandsPanelActive = true;
                BenchmarkPanelActive = false;
                PrecisePanelActive = false;
                RoomOffsetPanelActive = false;
                break;
            case AdjustmentMode.SwingBenchmark:
                UseFreeHandActive = true;
                HandsPanelActive = false;
                BenchmarkPanelActive = true;
                PrecisePanelActive = false;
                RoomOffsetPanelActive = false;
                break;
            case AdjustmentMode.Precise:
                UseFreeHandActive = false;
                HandsPanelActive = false;
                BenchmarkPanelActive = false;
                PrecisePanelActive = true;
                RoomOffsetPanelActive = false;
                break;
            case AdjustmentMode.RoomOffset:
                UseFreeHandActive = true;
                HandsPanelActive = false;
                BenchmarkPanelActive = false;
                PrecisePanelActive = false;
                RoomOffsetPanelActive = true;
                break;
            default: throw new ArgumentOutOfRangeException();
        }
    }

    private void GoToBrowserPage(bool allowSave, bool allowLoad) {
        MainPageActive = false;
        PresetsBrowserActive = true;
        PresetsBrowserSaveActive = allowSave;
        PresetsBrowserLoadActive = allowLoad;
    }

    #endregion
}