using System;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal class ModPanelUI : NotifiableSingleton<ModPanelUI> {
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

    #region Constructor

    public ModPanelUI() {
        GoToMainPage();

        GoToMainPageEvent += GoToMainPage;
        GoToBrowserPageEvent += GoToBrowserPage;
    }

    #endregion

    #region Components

    [UIValue("adjustment-mode-selector-modal"), UsedImplicitly]
    private AdjustmentModeSelectorModal _adjustmentModeSelector;

    [UIValue("top-panel"), UsedImplicitly]
    private TopPanel _topPanel = ReeUIComponentV2.InstantiateOnSceneRoot<TopPanel>(false);

    [UIValue("none-panel"), UsedImplicitly]
    private NonePanel _nonePanel = ReeUIComponentV2.InstantiateOnSceneRoot<NonePanel>(false);

    [UIValue("sliders-panel"), UsedImplicitly]
    private SlidersPanel _slidersPanel = ReeUIComponentV2.InstantiateOnSceneRoot<SlidersPanel>(false);

    [UIValue("swing-benchmark-panel"), UsedImplicitly]
    private SwingBenchmarkPanel _swingBenchmarkPanel = ReeUIComponentV2.InstantiateOnSceneRoot<SwingBenchmarkPanel>(false);

    [UIValue("room-offset-panel"), UsedImplicitly]
    private RoomOffsetPanel _roomOffsetPanel = ReeUIComponentV2.InstantiateOnSceneRoot<RoomOffsetPanel>(false);

    [UIValue("bottom-panel"), UsedImplicitly]
    private BottomPanel _bottomPanel = ReeUIComponentV2.InstantiateOnSceneRoot<BottomPanel>(false);

    [UIValue("presets-browser-panel"), UsedImplicitly]
    private PresetsBrowserPanel _presetsBrowserPanel = ReeUIComponentV2.InstantiateOnSceneRoot<PresetsBrowserPanel>(false);

    private void Awake() {
        _adjustmentModeSelector = ReeUIComponentV2.Instantiate<AdjustmentModeSelectorModal>(transform, false);
        _topPanel.SetParent(transform);
        _nonePanel.SetParent(transform);
        _slidersPanel.SetParent(transform);
        _swingBenchmarkPanel.SetParent(transform);
        _roomOffsetPanel.SetParent(transform);
        _bottomPanel.SetParent(transform);
        _presetsBrowserPanel.SetParent(transform);
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