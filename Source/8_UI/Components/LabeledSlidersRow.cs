using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;
using TMPro;

namespace EasyOffset;

internal class LabeledSlidersRow : ReeUIComponentV2 {
    #region Components

    [UIValue("left-slider"), UsedImplicitly]
    public SmoothSlider leftSlider;

    [UIValue("right-slider"), UsedImplicitly]
    public SmoothSlider rightSlider;

    private void Awake() {
        leftSlider = Instantiate<SmoothSlider>(transform);
        rightSlider = Instantiate<SmoothSlider>(transform);
    }

    #endregion

    #region Initialize

    private const float VerticalScale = 0.9f;

    protected override void OnInitialize() {
        leftSlider.SetVerticalScale(VerticalScale);
        rightSlider.SetVerticalScale(VerticalScale);
    }

    #endregion

    #region Setup

    public void Setup(
        string label,
        SmoothSlider.RangeDescriptor rangeDescriptor,
        SmoothSlider.AppearanceDescriptor appearanceDescriptor,
        DirectModeVariable leftSliderValue,
        DirectModeVariable rightSliderValue
    ) {
        _labelComponent.text = label;
        _labelComponent.faceColor = appearanceDescriptor.Color;
        leftSlider.Setup(rangeDescriptor, appearanceDescriptor, leftSliderValue);
        rightSlider.Setup(rangeDescriptor, appearanceDescriptor, rightSliderValue);
    }

    public void Setup(
        string label,
        SmoothSlider.RangeDescriptor rangeDescriptor,
        SmoothSlider.AppearanceDescriptor leftAppearanceDescriptor,
        SmoothSlider.AppearanceDescriptor rightAppearanceDescriptor,
        DirectModeVariable leftSliderValue,
        DirectModeVariable rightSliderValue
    ) {
        _labelComponent.text = label;
        _labelComponent.faceColor = leftAppearanceDescriptor.Color;
        leftSlider.Setup(rangeDescriptor, leftAppearanceDescriptor, leftSliderValue);
        rightSlider.Setup(rangeDescriptor, rightAppearanceDescriptor, rightSliderValue);
    }

    #endregion

    #region LabelComponent

    [UIComponent("label-component"), UsedImplicitly]
    private TextMeshProUGUI _labelComponent;

    #endregion
}