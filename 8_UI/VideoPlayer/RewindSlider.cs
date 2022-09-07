using System;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components.Settings;
using HMUI;
using IPA.Utilities;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal class RewindSlider : ReeUIComponentV2 {
    #region Initialize

    protected override void OnInitialize() {
        InitializeSlider();
        UpdateSliderText();
    }

    #endregion

    #region Interaction

    public event Action<float> OnClickEvent;

    private float _totalSeconds;
    private float _currentSeconds;

    public void SetTime(float currentSeconds, float totalSeconds) {
        _sliderComponent.slider.maxValue = totalSeconds;
        _sliderComponent.Value = currentSeconds;
        
        _currentSeconds = currentSeconds;
        _totalSeconds = totalSeconds;
        
        UpdateSliderText();
    }

    public void SetActive(bool value) {
        _sliderBackground.gameObject.SetActive(value);
    }

    #endregion

    #region Events

    private void OnSliderValueDidChange(RangeValuesTextSlider _, float value) {
        OnClickEvent?.Invoke(value);
    }

    private void UpdateSliderText() {
        SplitTimeUnits(_currentSeconds, out var currentMinutes, out var currentSeconds);
        SplitTimeUnits(_totalSeconds, out var totalMinutes, out var totalSeconds);
        SliderText = $"{currentMinutes}:{currentSeconds:0#}/{totalMinutes}:{totalSeconds:0#}";
    }

    private static void SplitTimeUnits(float totalSeconds, out int minutes, out int seconds) {
        minutes = (int)(totalSeconds / 60);
        seconds = (int)(totalSeconds % 60);
    }

    #endregion

    #region Slider

    [UIComponent("slider-background"), UsedImplicitly]
    private ImageView _sliderBackground;

    [UIComponent("slider-component"), UsedImplicitly]
    private SliderSetting _sliderComponent;

    [UIAction("slider-formatter"), UsedImplicitly]
    private string SliderFormatter(float value) => string.Empty;

    private string _sliderText = string.Empty;

    [UIValue("slider-text"), UsedImplicitly]
    private string SliderText {
        get => _sliderText;
        set {
            if (_sliderText.Equals(value)) return;
            _sliderText = value;
            NotifyPropertyChanged();
        }
    }

    private static Color BarHighlightColor => new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private static Color BarPressedColor => new Color(0.6f, 0.6f, 0.6f, 1.0f);
    private static Color BarNormalColor => new Color(0.6f, 0.6f, 0.6f, 0.5f);

    private void InitializeSlider() {
        _sliderComponent.slider.valueDidChangeEvent += OnSliderValueDidChange;
        TryModifySlider();

        var colors = _sliderComponent.slider.colors;
        colors.normalColor = BarNormalColor;
        colors.pressedColor = BarPressedColor;
        colors.highlightedColor = BarHighlightColor;
        _sliderComponent.slider.colors = colors;
    }

    private void TryModifySlider() {
        try {
            var images = _sliderComponent.GetComponentsInChildren<ImageView>();
            images[0].SetField("_skew", 0.0f);
            images[0].SetVerticesDirty();
            images[0].transform.localScale = new Vector3(0.88f, 0.2f, 1.0f);
            images[1].transform.localScale = new Vector3(1.4f, 3.0f, 1.0f);
        } catch (Exception) {
            // Suppress
        }
    }

    #endregion
}