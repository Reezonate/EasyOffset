using System;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

[ViewDefinition("EasyOffset._9_Resources.BSML.UserGuide.UserGuideViewController.bsml")]
public class UserGuideViewController : BSMLAutomaticViewController {
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
            _page0 = ReeUIComponentV2.Instantiate<UserGuidePage0>(transform),
            _page1 = ReeUIComponentV2.Instantiate<UserGuidePage1>(transform),
            _page2 = ReeUIComponentV2.Instantiate<UserGuidePage2>(transform),
            _page3 = ReeUIComponentV2.Instantiate<UserGuidePage3>(transform),
            _page4 = ReeUIComponentV2.Instantiate<UserGuidePage4>(transform),
            _page5 = ReeUIComponentV2.Instantiate<UserGuidePage5>(transform),
        };

        _videoPlayer = ReeUIComponentV2.Instantiate<ReeVideoPlayer>(transform, false);
    }

    #endregion

    #region Start / OnDestroy

    private void Start() {
        InitializePanels();
        _videoPlayer.AddStateListener(OnPlayerStateChanged);
        CurrentPage = FirstPage;
    }

    protected override void OnDestroy() {
        _videoPlayer.RemoveStateListener(OnPlayerStateChanged);
        base.OnDestroy();
    }

    #endregion

    #region Events

    [UIAction("close-on-click"), UsedImplicitly]
    private void CloseOnClick() {
        UIEvents.NotifyUserGuideButtonWasPressed();
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

            var commitFunny = RemoteConfig.UserGuideConfig.FunnyType switch {
                0 => !_anyVideoWasOpened,
                1 => _anyVideoWasOpened,
                2 => true,
                _ => false
            };
            
            WatchVideoButtonActive = PageData.ShowVideoPlayer && (!PageData.IsFunny || commitFunny);

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
                if (!PageData.IsFunny) _anyVideoWasOpened = true;
                _videoPlayer.SetVideo(PageData.VideoKey, PageData.VideoUrl, PageData.IsFunny);
                UIEvents.NotifyUserGuideVideoStarted();
            }
        }
    }

    #endregion

    #region WatchVideo button

    private void UpdateButtonText() {
        WatchVideoButtonText = ShowVideo ? PageData.CloseButtonText : PageData.WatchButtonText;
    }

    private string _watchVideoButtonText = "";

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