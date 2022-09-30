using BeatSaberMarkupLanguage.Attributes;
using HMUI;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal class UserGuideModal : ReeUIComponentV2 {
    #region Components

    [UIValue("page-0"), UsedImplicitly]
    private UserGuidePage0 _page0;

    [UIValue("page-1"), UsedImplicitly]
    private UserGuidePage1 _page1;

    [UIValue("page-2"), UsedImplicitly]
    private UserGuidePage2 _page2;

    [UIValue("page-3"), UsedImplicitly]
    private UserGuidePage3 _page3;

    [UIValue("page-4"), UsedImplicitly]
    private UserGuidePage4 _page4;

    [UIValue("page-5"), UsedImplicitly]
    private UserGuidePage5 _page5;

    [UIValue("video-player"), UsedImplicitly]
    private ReeVideoPlayer _videoPlayer;

    private UserGuidePage[] _pages;

    private void Awake() {
        _pages = new UserGuidePage[] {
            _page0 = Instantiate<UserGuidePage0>(transform),
            _page1 = Instantiate<UserGuidePage1>(transform),
            _page2 = Instantiate<UserGuidePage2>(transform),
            _page3 = Instantiate<UserGuidePage3>(transform),
            _page4 = Instantiate<UserGuidePage4>(transform),
            _page5 = Instantiate<UserGuidePage5>(transform),
        };

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
        if (PageData.IsFunny) WatchVideoButtonActive = false;
    }

    #endregion

    #region Content

    private const int FirstPage = 0;
    private int TotalPages => _pages.Length;
    private int LastPage => TotalPages - 1;

    private UserGuidePage PageData => _pages[CurrentPage];

    private int _currentPageIndex;

    private int CurrentPage {
        get => _currentPageIndex;
        set {
            _currentPageIndex = value;
            if (_currentPageIndex < FirstPage) _currentPageIndex = FirstPage;
            if (_currentPageIndex > LastPage) _currentPageIndex = LastPage;

            HasPreviousPage = _currentPageIndex > FirstPage;
            HasNextPage = _currentPageIndex < LastPage;
            ShowVideo = false;

            Title = PageData.Title;
            WatchVideoButtonActive = PageData.ShowVideoPlayer && (!PageData.IsFunny || _anyVideoWasOpened);

            for (var i = 0; i < TotalPages; i++) {
                _pages[i].SetActive(i == _currentPageIndex);
            }
        }
    }

    #endregion

    #region Panels

    [UIComponent("header-panel"), UsedImplicitly]
    private ImageView _headerPanel;

    [UIComponent("content-panel"), UsedImplicitly]
    private ImageView _contentPanel;

    [UIComponent("player-panel"), UsedImplicitly]
    private RectTransform _playerPanel;

    private void InitializePanels() {
        _headerPanel.raycastTarget = true;
        _contentPanel.raycastTarget = true;
        ShowVideo = false;
    }

    private bool _showVideo;
    private bool _anyVideoWasOpened;

    private bool ShowVideo {
        get => _showVideo;
        set {
            _showVideo = value;
            _contentPanel.gameObject.SetActive(!value);
            _playerPanel.gameObject.SetActive(value);
            UpdateButtonText();
            if (value) {
                _anyVideoWasOpened = true;
                _videoPlayer.SetVideo(PageData.VideoKey, PageData.VideoUrl, PageData.IsFunny);
            }
        }
    }

    #endregion

    #region WatchVideo button

    private const string WatchVideoText = "Watch video";
    private const string CloseVideoText = "Back";
    private const string FunnyText = "Get pro player configs";

    private void UpdateButtonText() {
        WatchVideoButtonText = ShowVideo ? CloseVideoText : PageData.IsFunny ? FunnyText : WatchVideoText;
    }

    private string _watchVideoButtonText = WatchVideoText;

    [UIValue("watch-video-button-text"), UsedImplicitly]
    private string WatchVideoButtonText {
        get => _watchVideoButtonText;
        set {
            if (_watchVideoButtonText.Equals(value)) return;
            _watchVideoButtonText = value;
            NotifyPropertyChanged();
        }
    }

    private bool _watchVideoButtonActive;

    [UIValue("watch-video-button-active"), UsedImplicitly]
    private bool WatchVideoButtonActive {
        get => _watchVideoButtonActive;
        set {
            if (_watchVideoButtonActive.Equals(value)) return;
            _watchVideoButtonActive = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("watch-video-button-on-click"), UsedImplicitly]
    private void WatchVideoButtonOnClick() {
        ShowVideo = !ShowVideo;
    }

    #endregion

    #region Modal

    [UIComponent("modal"), UsedImplicitly]
    private ModalView _modal;

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