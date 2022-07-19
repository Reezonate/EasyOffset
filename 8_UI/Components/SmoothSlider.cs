using System;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components.Settings;
using HMUI;
using IPA.Utilities;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EasyOffset;

internal class SmoothSlider : ReeUIComponentV2 {
    #region Initialize

    private Material _leftArrowMaterial;
    private Material _rightArrowMaterial;
    private Transform _textTransform;

    protected override void OnInitialize() {
        InitializeSlider();
        InitializeButtons();
    }

    protected override void OnDispose() {
        UnsetValue();
    }

    private void InitializeSlider() {
        var pointerEventsHandler = _sliderComponent.slider.gameObject.AddComponent<PointerEventsHandler>();
        pointerEventsHandler.smoothSlider = this;

        var textComponent = _sliderComponent.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1];
        textComponent.richText = true;
        _textTransform = textComponent.transform;
    }

    private void InitializeButtons() {
        var incButton = _sliderComponent.slider.GetField<Button, RangeValuesTextSlider>("_incButton");
        var decButton = _sliderComponent.slider.GetField<Button, RangeValuesTextSlider>("_decButton");
        InitializeButton(incButton, out _rightArrowMaterial);
        InitializeButton(decButton, out _leftArrowMaterial);
    }

    private void InitializeButton(Button button, out Material arrowMaterial) {
        button.onClick.AddListener(OnButtonClick);
        var arrowImage = button.GetComponentsInChildren<ImageView>()[1];
        arrowMaterial = arrowImage.material = Instantiate(arrowImage.material);
    }

    #endregion

    #region Setup

    private float _smoothFactor;
    private Func<float, string> _formatter;

    public void SetVerticalScale(float verticalScale) {
        _sliderComponent.transform.localScale = new Vector3(1.0f, verticalScale, 1.0f);
        _textTransform.localScale = new Vector3(1.0f, 1.0f / verticalScale, 1.0f);
    }

    public void Setup(
        RangeDescriptor rangeDescriptor,
        AppearanceDescriptor appearanceDescriptor,
        DirectModeVariable value
    ) {
        _sliderComponent.slider.minValue = rangeDescriptor.MinimalValue;
        _sliderComponent.slider.maxValue = rangeDescriptor.MaximalValue;
        _sliderComponent.slider.numberOfSteps = rangeDescriptor.NumberOfSteps;
        _smoothFactor = rangeDescriptor.SmoothFactor;
        _formatter = appearanceDescriptor.Formatter;
        ApplyColor(appearanceDescriptor.Color);
        SetValue(value);
    }

    private void ApplyColor(Color color) {
        var colors = _sliderComponent.slider.colors;
        colors.highlightedColor = color.ColorWithAlpha(0.3f);
        colors.pressedColor = color.ColorWithAlpha(0.4f);
        _sliderComponent.slider.colors = colors;

        _sliderComponent.slider.handleColor = color;
        _rightArrowMaterial.color = color;
        _leftArrowMaterial.color = color;
    }

    #endregion

    #region Value

    [CanBeNull] private DirectModeVariable _value;

    private void SetValue([NotNull] DirectModeVariable value) {
        UnsetValue();
        _value = value;
        _value.ChangedFromCodeEvent += OnExternalValueChange;
        OnExternalValueChange(value.GetValue());
    }

    private void UnsetValue() {
        if (_value == null) return;
        _value.ChangedFromCodeEvent -= OnExternalValueChange;
        _value = null;
    }

    public void OnExternalValueChange(float newValue) {
        _sliderComponent.ReceiveValue();
    }

    #endregion

    #region Smoothing

    private static readonly Range MultiplierTimeRange = new(0.0f, 2.0f);

    private bool _pressed;
    private float _pressedTime;
    private float _targetValue;
    private float _currentValue;

    private void LateUpdate() {
        if (!_pressed) return;
        var t = _smoothFactor * Time.deltaTime * MultiplierTimeRange.GetRatioClamped(Time.time - _pressedTime);
        _currentValue = Mathf.Lerp(_currentValue, _targetValue, t);
        _value?.SetValueFromUI(_currentValue);
        _sliderComponent.ReceiveValue();
    }

    #endregion

    #region Events

    private void OnButtonClick() {
        _value?.SetValueFromUI(_targetValue);
    }

    private void OnPointerDown() {
        _currentValue = _targetValue = SliderValue;
        _pressed = true;
        _pressedTime = Time.time;
        _value?.NotifyChangeStarted();
    }

    private void OnPointerUp() {
        _currentValue = _targetValue = SliderValue;
        _pressed = false;
        _value?.NotifyChangeFinished();
    }

    #endregion

    #region SliderComponent

    [UIComponent("slider-component"), UsedImplicitly]
    private SliderSetting _sliderComponent;

    [UIAction("slider-formatter"), UsedImplicitly]
    private string SliderFormatter(float value) {
        return _formatter?.Invoke(value) ?? $"{value:F2}";
    }

    [UIValue("slider-value"), UsedImplicitly]
    private float SliderValue {
        get => _value?.GetValue() ?? 0.0f;
        set => _targetValue = value;
    }

    private bool _interactable = true;

    [UIValue("interactable"), UsedImplicitly]
    public bool Interactable {
        get => _interactable;
        set {
            if (_interactable == value) return;
            _interactable = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region PointerEventsHandler

    private class PointerEventsHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
        public SmoothSlider smoothSlider;

        public void OnPointerDown(PointerEventData eventData) => smoothSlider.OnPointerDown();

        public void OnPointerUp(PointerEventData eventData) => smoothSlider.OnPointerUp();
    }

    #endregion

    #region Descriptors

    public readonly struct RangeDescriptor {
        public readonly float MinimalValue;
        public readonly float MaximalValue;
        public readonly int NumberOfSteps;
        public readonly float SmoothFactor;

        public RangeDescriptor(float minimalValue, float maximalValue, float increment, float smoothFactor) {
            MinimalValue = minimalValue;
            MaximalValue = maximalValue;
            NumberOfSteps = (int) Mathf.Round((MaximalValue - MinimalValue) / increment) + 1;
            SmoothFactor = smoothFactor;
        }
    }

    public readonly struct AppearanceDescriptor {
        public readonly Color Color;
        public readonly Func<float, string> Formatter;

        public AppearanceDescriptor(Color color, Func<float, string> formatter) {
            Color = color;
            Formatter = formatter;
        }
    }

    #endregion
}