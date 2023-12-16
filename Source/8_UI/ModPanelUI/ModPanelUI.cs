using System;
using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal class ModPanelUI : PepegaSingletonFix<ModPanelUI> {
    #region static

    private static event Action GoToMainPageEvent;
    private static event Action<bool, bool> GoToBrowserPageEvent;

    public static void OpenMainPage() {
        GoToMainPageEvent?.Invoke();
    }

    public static void OpenBrowserPage(bool allowSave, bool allowLoad) {
        GoToBrowserPageEvent?.Invoke(allowSave, allowLoad);
    }

    #endregion

    #region Components

    [UIValue("adjustment-mode-selector-modal"), UsedImplicitly]
    private AdjustmentModeSelectorModal _adjustmentModeSelector;

    [UIValue("top-panel"), UsedImplicitly]
    private TopPanel _topPanel;

    [UIValue("none-panel"), UsedImplicitly]
    private NonePanel _nonePanel;

    [UIValue("sliders-panel"), UsedImplicitly]
    private SlidersPanel _slidersPanel;

    [UIValue("swing-benchmark-panel"), UsedImplicitly]
    private SwingBenchmarkPanel _swingBenchmarkPanel;

    [UIValue("room-offset-panel"), UsedImplicitly]
    private RoomOffsetPanel _roomOffsetPanel;

    [UIValue("bottom-panel"), UsedImplicitly]
    private BottomPanel _bottomPanel;

    [UIValue("presets-browser-panel"), UsedImplicitly]
    private PresetsBrowserPanel _presetsBrowserPanel;

    private void Awake() {
        _adjustmentModeSelector = ReeUIComponentV2.Instantiate<AdjustmentModeSelectorModal>(transform, false);

        _topPanel = ReeUIComponentV2.Instantiate<TopPanel>(transform,false);
        _nonePanel = ReeUIComponentV2.Instantiate<NonePanel>(transform, false);
        _slidersPanel = ReeUIComponentV2.Instantiate<SlidersPanel>(transform, false);
        _swingBenchmarkPanel = ReeUIComponentV2.Instantiate<SwingBenchmarkPanel>(transform, false);
        _roomOffsetPanel = ReeUIComponentV2.Instantiate<RoomOffsetPanel>(transform, false);
        _bottomPanel = ReeUIComponentV2.Instantiate<BottomPanel>(transform, false);
        _presetsBrowserPanel = ReeUIComponentV2.Instantiate<PresetsBrowserPanel>(transform, false);
        
        GoToMainPage();
        GoToMainPageEvent += GoToMainPage;
        GoToBrowserPageEvent += GoToBrowserPage;
    }

    #endregion

    #region Visibility

    [UIObject("root"), UsedImplicitly]
    private GameObject _root;

    [UIAction("#post-parse"), UsedImplicitly]
    private void OnAfterParse() {
        _root.AddComponent<VisibilityTracker>();
    }

    #endregion

    #region Flow

    private void GoToMainPage() {
        MainPageActive = true;
        _presetsBrowserPanel.Deactivate();
    }

    private void GoToBrowserPage(bool allowSave, bool allowLoad) {
        MainPageActive = false;
        _presetsBrowserPanel.Activate(allowSave, allowLoad);
    }

    #endregion

    #region MainPageActive

    private bool _mainPageActive = true;

    [UIValue("main-page-active")]
    [UsedImplicitly]
    private bool MainPageActive {
        get => _mainPageActive;
        set {
            if (_mainPageActive.Equals(value)) return;
            _mainPageActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion
}