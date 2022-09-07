using System;
using BeatSaberMarkupLanguage.Attributes;
using HMUI;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal class ReeVideoPlayer : ReeUIComponentV2, IWebRequestHandler<string> {
    #region Components

    [UIComponent("hover-area"), UsedImplicitly]
    private RectTransform _hoverArea;

    [UIComponent("screen"), UsedImplicitly]
    private ImageView _screen;

    [UIValue("download-progress"), UsedImplicitly]
    private DownloadProgress _downloadProgress;

    [UIValue("play-button"), UsedImplicitly]
    private PlayButton _playButton;

    [UIValue("rewind-slider"), UsedImplicitly]
    private RewindSlider _rewindSlider;

    private void Awake() {
        _downloadProgress = Instantiate<DownloadProgress>(transform);
        _playButton = Instantiate<PlayButton>(transform);
        _rewindSlider = Instantiate<RewindSlider>(transform);
    }

    #endregion

    #region OnInitialize / OnDispose

    private VideoRenderer _videoRenderer;
    private HoverController _hoverController;

    protected override void OnInitialize() {
        _videoRenderer = VideoRenderer.FromImageView(_screen);
        _hoverController = _hoverArea.gameObject.AddComponent<HoverController>();
        _hoverController.HoverStateChangedEvent += OnHoverStateChanged;
        _videoRenderer.OnTimeUpdated += OnVideoTimeUpdated;
        _videoRenderer.OnVideoEnded += OnVideoEnded;
        _playButton.OnClickEvent += OnButtonClick;
        _rewindSlider.OnClickEvent += OnRewind;
        UpdateVisuals();
    }

    protected override void OnDispose() {
        _hoverController.HoverStateChangedEvent -= OnHoverStateChanged;
        _videoRenderer.OnTimeUpdated -= OnVideoTimeUpdated;
        _videoRenderer.OnVideoEnded -= OnVideoEnded;
        _playButton.OnClickEvent -= OnButtonClick;
        _rewindSlider.OnClickEvent -= OnRewind;
    }

    #endregion

    #region Interaction

    public void SetVideo(string key, string url) {
        _videoRenderer.Stop();
        StopAllCoroutines();
        SetState(State.Uninitialized);
        StartCoroutine(VideoCache.GetVideoCoroutine(key, url, this));
    }

    #endregion

    #region Events

    private void OnButtonClick() {
        switch (_currentState) {
            case State.Playing:
                _videoRenderer.Pause();
                SetState(State.Paused);
                break;
            case State.Paused:
                _videoRenderer.Play();
                SetState(State.Playing);
                break;
            case State.Finished:
                _videoRenderer.Play();
                SetState(State.Playing);
                break;
            default: return;
        }
    }

    private void OnRewind(float time) {
        _videoRenderer.Seek(time);
    }

    private void OnVideoTimeUpdated(float currentSeconds, float totalSeconds) {
        _rewindSlider.SetTime(currentSeconds, totalSeconds);
    }

    private void OnVideoEnded() {
        SetState(State.Finished);
    }

    private void OnHoverStateChanged(bool isHovered) {
        _isHovered = isHovered;
        UpdateVisuals();
    }

    public void OnRequestStarted() {
        SetState(State.Downloading);
    }

    public void OnRequestFinished(string result) {
        _videoRenderer.Prepare($"file://{result}");
        _videoRenderer.Play();
        SetState(State.Playing);
    }

    public void OnRequestFailed(string reason) {
        SetState(State.Failed);
    }

    public void OnRequestProgress(float uploadProgress, float downloadProgress, float overallProgress) {
        _downloadProgress.Progress = downloadProgress;
    }

    #endregion

    #region State

    private event Action<State> StateChangedEvent;

    private State _currentState = State.Uninitialized;

    private void SetState(State newState) {
        if (_currentState == newState) return;
        _currentState = newState;
        UpdateVisuals();
        StateChangedEvent?.Invoke(newState);
    }

    public void AddStateListener(Action<State> handler) {
        StateChangedEvent += handler;
        handler?.Invoke(_currentState);
    }

    public void RemoveStateListener(Action<State> handler) {
        StateChangedEvent -= handler;
    }

    public enum State {
        Uninitialized,
        Downloading,
        Playing,
        Paused,
        Finished,
        Failed
    }

    #endregion

    #region UpdateVisuals

    private bool _isHovered;

    private void UpdateVisuals() {
        switch (_currentState) {
            case State.Uninitialized:
                _downloadProgress.SetActive(false);
                _playButton.SetActive(false);
                _rewindSlider.SetActive(false);
                break;
            case State.Downloading:
                _downloadProgress.Label = "Loading";
                _downloadProgress.SetActive(true);
                _playButton.SetActive(false);
                _rewindSlider.SetActive(false);
                break;
            case State.Playing:
                _downloadProgress.SetActive(false);
                _playButton.SetState(PlayButton.State.Pause);
                _playButton.SetActive(_isHovered);
                _rewindSlider.SetActive(_isHovered);
                break;
            case State.Paused:
            case State.Finished:
                _downloadProgress.SetActive(false);
                _playButton.SetState(PlayButton.State.Play);
                _playButton.SetActive(_isHovered);
                _rewindSlider.SetActive(_isHovered);
                break;
            case State.Failed:
                _downloadProgress.Label = "Download failed";
                _downloadProgress.SetActive(true);
                _playButton.SetActive(false);
                _rewindSlider.SetActive(false);
                break;
            default: return;
        }
    }

    #endregion
}