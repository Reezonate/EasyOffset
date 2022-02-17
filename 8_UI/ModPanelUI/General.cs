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
        SubscribeToSmoothingEvents();
        SubscribeToPrecisePanelEvents();
        SubscribeToUndoRedoEvents();
        GoToMainPage();
    }

    #endregion

    #region Updates

    private void Update() {
        UpdatePanelVisibility();
        SmoothingUpdate();
    }

    private void LateUpdate() {
        SynchronizationUpdate();
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
                UseFreeHandActive = false;
                NonePanelActive = true;
                BenchmarkPanelActive = false;
                RoomOffsetPanelActive = false;
                UndoRedoButtonsActive = false;
                SetPrecisePanelState(PrecisePanelState.Hidden);
                break;
            case AdjustmentMode.Basic:
                UseFreeHandActive = true;
                NonePanelActive = false;
                BenchmarkPanelActive = false;
                RoomOffsetPanelActive = false;
                UndoRedoButtonsActive = true;
                SetPrecisePanelState(PrecisePanelState.ZOffsetOnly);
                break;
            case AdjustmentMode.Position:
                UseFreeHandActive = true;
                NonePanelActive = false;
                BenchmarkPanelActive = false;
                RoomOffsetPanelActive = false;
                UndoRedoButtonsActive = true;
                SetPrecisePanelState(PrecisePanelState.PositionOnly);
                break;
            case AdjustmentMode.Rotation:
            case AdjustmentMode.RotationAuto:
                UseFreeHandActive = true;
                NonePanelActive = false;
                BenchmarkPanelActive = false;
                RoomOffsetPanelActive = false;
                UndoRedoButtonsActive = true;
                SetPrecisePanelState(PrecisePanelState.RotationOnly);
                break;
            case AdjustmentMode.SwingBenchmark:
                UseFreeHandActive = true;
                NonePanelActive = false;
                BenchmarkPanelActive = true;
                RoomOffsetPanelActive = false;
                UndoRedoButtonsActive = true;
                SetPrecisePanelState(PrecisePanelState.Hidden);
                break;
            case AdjustmentMode.Precise:
                UseFreeHandActive = false;
                NonePanelActive = false;
                BenchmarkPanelActive = false;
                RoomOffsetPanelActive = false;
                UndoRedoButtonsActive = true;
                SetPrecisePanelState(PrecisePanelState.Full);
                break;
            case AdjustmentMode.RoomOffset:
                UseFreeHandActive = true;
                NonePanelActive = false;
                BenchmarkPanelActive = false;
                RoomOffsetPanelActive = true;
                UndoRedoButtonsActive = false;
                SetPrecisePanelState(PrecisePanelState.Hidden);
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