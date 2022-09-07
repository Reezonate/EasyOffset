using BeatSaberMarkupLanguage.Attributes;
using HMUI;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal class UserGuideModal : ReeUIComponentV2 {
    #region Components

    [UIValue("video-player"), UsedImplicitly] private ReeVideoPlayer _videoPlayer;

    private void Awake() {
        _videoPlayer = Instantiate<ReeVideoPlayer>(transform, false);
    }

    #endregion

    #region Init / Dispose

    protected override void OnInitialize() {
        InitializeModal();
        InitializePanels();

        UIEvents.UserGuideButtonWasPressedEvent += ShowModal;
        PluginConfig.IsModPanelVisibleChangedEvent += OnIsModPanelVisibleChanged;
        _videoPlayer.AddStateListener(OnPlayerStateChanged);

        CurrentPage = FirstPage;
    }

    protected override void OnDispose() {
        UIEvents.UserGuideButtonWasPressedEvent -= ShowModal;
        PluginConfig.IsModPanelVisibleChangedEvent -= OnIsModPanelVisibleChanged;
        _videoPlayer.RemoveStateListener(OnPlayerStateChanged);
    }

    #endregion

    #region Events

    private void OnIsModPanelVisibleChanged(bool isVisible) {
        if (!isVisible) HideModal(false);
    }

    private void OnPlayerStateChanged(ReeVideoPlayer.State state) {
        if (state != ReeVideoPlayer.State.Playing) return;
        if (CurrentPage == 5) PreviewButtonText = ";D";
    }

    [UIAction("rotation-auto-on-click"), UsedImplicitly]
    private void RotationAutoOnClick() => PluginConfig.AdjustmentMode = AdjustmentMode.RotationAuto;

    [UIAction("position-auto-on-click"), UsedImplicitly]
    private void PositionAutoOnClick() => PluginConfig.AdjustmentMode = AdjustmentMode.PositionAuto;

    [UIAction("swing-benchmark-on-click"), UsedImplicitly]
    private void SwingBenchmarkOnClick() => PluginConfig.AdjustmentMode = AdjustmentMode.SwingBenchmark;

    [UIAction("rotation-on-click"), UsedImplicitly]
    private void RotationOnClick() => PluginConfig.AdjustmentMode = AdjustmentMode.Rotation;

    [UIAction("youtube-on-click"), UsedImplicitly]
    private void YoutubeOnClick() { }

    [UIAction("github-on-click"), UsedImplicitly]
    private void GuthubOnClick() { }

    [UIAction("discord-on-click"), UsedImplicitly]
    private void DiscordOnClick() { }

    #endregion

    #region Pages

    private readonly Page[] _pages = {
        new() {
            Title = "How to use"
        },
        new() {
            Title = "Step 1 ‒ Positions",
            ShowVideoPlayer = true,
            VideoKey = "PositionAuto",
            VideoUrl = "https://github.com/Reezonate/EasyOffset/raw/experimental/media/PositionAuto.mp4"
        },
        new() {
            Title = "Step 2 ‒ Rotations",
            ShowVideoPlayer = true,
            VideoKey = "RotationAuto",
            VideoUrl = "https://github.com/Reezonate/EasyOffset/raw/experimental/media/RotationAuto.mp4"
        },
        new() {
            Title = "Step 3 ‒ Reference",
            ShowVideoPlayer = true,
            VideoKey = "SwingBenchmark",
            VideoUrl = "https://github.com/Reezonate/EasyOffset/raw/experimental/media/SwingBenchmark.mp4"
        },
        new() {
            Title = "Step 4 ‒ Fine-tuning"
        },
        new() {
            Title = "More info",
            ShowVideoPlayer = true,
            VideoKey = "MoreInfo",
            VideoUrl = "https://github.com/Reezonate/EasyOffset/raw/experimental/media/MoreInfo.mp4"
        }
    };

    private struct Page {
        public string Title;
        public bool ShowVideoPlayer;
        public string VideoKey;
        public string VideoUrl;
    }

    #endregion

    #region Content

    [UIComponent("content-panel"), UsedImplicitly] private RectTransform _contentRoot;

    private int _currentPage;

    private const int FirstPage = 0;
    private int TotalPages => _pages.Length;
    private int LastPage => TotalPages - 1;

    private int CurrentPage {
        get => _currentPage;
        set {
            _currentPage = value;
            if (_currentPage < FirstPage) _currentPage = FirstPage;
            if (_currentPage > LastPage) _currentPage = LastPage;

            HasPreviousPage = _currentPage > FirstPage;
            HasNextPage = _currentPage < LastPage;
            ShowVideo = false;

            var pageData = _pages[_currentPage];

            Title = pageData.Title;
            PreviewButtonInteractable = pageData.ShowVideoPlayer;

            for (var i = 0; i < TotalPages; i++) {
                _contentRoot.GetChild(i).gameObject.SetActive(i == _currentPage);
            }
        }
    }

    #endregion

    #region Panels

    [UIComponent("header-panel"), UsedImplicitly] private ImageView _headerPanel;

    [UIComponent("content-panel"), UsedImplicitly] private ImageView _contentPanel;

    [UIComponent("player-panel"), UsedImplicitly] private RectTransform _playerPanel;

    private void InitializePanels() {
        _headerPanel.raycastTarget = true;
        _contentPanel.raycastTarget = true;
        ShowVideo = false;
    }

    private bool _showVideo;

    private bool ShowVideo {
        get => _showVideo;
        set {
            _showVideo = value;
            _contentPanel.gameObject.SetActive(!value);
            _playerPanel.gameObject.SetActive(value);
            if (value) {
                var pageData = _pages[_currentPage];
                _videoPlayer.SetVideo(pageData.VideoKey, pageData.VideoUrl);
            }
            PreviewButtonText = value ? CloseVideoText : WatchVideoText;
        }
    }

    #endregion

    #region PreviewButton

    private const string WatchVideoText = "Watch video";
    private const string CloseVideoText = "Close video";

    private string _previewButtonText = WatchVideoText;

    [UIValue("preview-button-text"), UsedImplicitly]
    private string PreviewButtonText {
        get => _previewButtonText;
        set {
            if (_previewButtonText.Equals(value)) return;
            _previewButtonText = value;
            NotifyPropertyChanged();
        }
    }

    private bool _previewButtonActive;

    [UIValue("preview-button-active"), UsedImplicitly]
    private bool PreviewButtonInteractable {
        get => _previewButtonActive;
        set {
            if (_previewButtonActive.Equals(value)) return;
            _previewButtonActive = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("preview-button-on-click"), UsedImplicitly]
    private void PreviewButtonOnClick() {
        ShowVideo = !ShowVideo;
    }

    #endregion

    #region Modal

    [UIComponent("modal"), UsedImplicitly] private ModalView _modal;

    private void InitializeModal() {
        var background = _modal.GetComponentInChildren<ImageView>();
        if (background != null) background.enabled = false;
        var touchable = _modal.GetComponentInChildren<Touchable>();
        if (touchable != null) touchable.enabled = false;
    }

    private void ShowModal() {
        ShowVideo = false;
        if (_modal == null) return;
        _modal.Show(true, true);
    }

    private void HideModal(bool animated) {
        if (_modal == null) return;
        _modal.Hide(animated);
    }

    #endregion

    #region Navigation

    #region Title

    private string _title = "";

    [UIValue("title"), UsedImplicitly]
    private string Title {
        get => _title;
        set {
            if (_title.Equals(value)) return;
            _title = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region PreviousPageButton

    private bool _hasPreviousPage;

    [UIValue("has-previous-page"), UsedImplicitly]
    private bool HasPreviousPage {
        get => _hasPreviousPage;
        set {
            if (_hasPreviousPage.Equals(value)) return;
            _hasPreviousPage = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("previous-page-on-click"), UsedImplicitly]
    private void PreviousPageOnClick() => CurrentPage -= 1;

    #endregion

    #region NextPageButton

    private bool _hasNextPage;

    [UIValue("has-next-page"), UsedImplicitly]
    private bool HasNextPage {
        get => _hasNextPage;
        set {
            if (_hasNextPage.Equals(value)) return;
            _hasNextPage = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("next-page-on-click"), UsedImplicitly]
    private void NextPageOnClick() => CurrentPage += 1;

    #endregion

    #endregion
}