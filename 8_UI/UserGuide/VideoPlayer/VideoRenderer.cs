using System;
using HMUI;
using UnityEngine;
using UnityEngine.Video;

namespace EasyOffset;

internal class VideoRenderer : MonoBehaviour {
    #region Construct

    private Material _materialInstance;
    private VideoPlayer _videoPlayer;

    public static VideoRenderer FromImageView(ImageView imageView) {
        var component = imageView.gameObject.AddComponent<VideoRenderer>();
        component.Construct(imageView);
        return component;
    }

    private void Construct(ImageView screen) {
        _materialInstance = Instantiate(BundleLoader.UIVideoPlayerMaterial);
        screen.material = _materialInstance;
        screen.raycastTarget = true;

        _videoPlayer = screen.gameObject.AddComponent<VideoPlayer>();
        _videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        _videoPlayer.aspectRatio = VideoAspectRatio.Stretch;
        _videoPlayer.source = VideoSource.Url;
        _videoPlayer.playOnAwake = false;
        _videoPlayer.SetDirectAudioVolume(0, PluginConfig.MainSettingsModel.volume);

        _videoPlayer.loopPointReached += _ => OnVideoEnded?.Invoke();

        CreateCleanTexture();
    }

    private void OnDestroy() {
        DisposeTexture();
    }

    #endregion

    #region OutputTexture

    private static readonly int VideoTexturePropertyID = Shader.PropertyToID("_VideoTexture");
    private RenderTexture _outputTexture;

    private void CreateCleanTexture() {
        DisposeTexture();

        _outputTexture = new RenderTexture(910, 512, 0, RenderTextureFormat.Default);
        _outputTexture.Create();

        _materialInstance.SetTexture(VideoTexturePropertyID, _outputTexture);
        _videoPlayer.targetTexture = _outputTexture;
    }

    private void DisposeTexture() {
        if (_outputTexture == null) return;
        _outputTexture.Release();
        _outputTexture = null;
    }

    #endregion

    #region Time

    private void Update() {
        if (!_videoPlayer.isPlaying) return;
        OnTimeUpdated?.Invoke((float)_videoPlayer.time, (float)_videoPlayer.length);
    }

    #endregion

    #region Interaction

    public event Action<float, float> OnTimeUpdated;
    public event Action OnVideoEnded;

    public void Prepare(string url) {
        _videoPlayer.url = url;
        _videoPlayer.Prepare();
    }

    public void Play() {
        _videoPlayer.Play();
    }

    public void Stop() {
        _videoPlayer.Stop();
        CreateCleanTexture();
    }

    public void Pause() {
        _videoPlayer.Pause();
    }

    public void Seek(float time) {
        _videoPlayer.time = time;
        OnTimeUpdated?.Invoke(time, (float)_videoPlayer.length);
    }

    #endregion
}