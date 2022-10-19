using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal class SlidersRowAnnotations : ReeUIComponentV2 {
    #region Components

    [UIValue("left-slider-annotations"), UsedImplicitly]
    public SliderAnnotations leftSliderAnnotations;

    [UIValue("right-slider-annotations"), UsedImplicitly]
    public SliderAnnotations rightSliderAnnotations;

    private void Awake() {
        leftSliderAnnotations = Instantiate<SliderAnnotations>(transform);
        rightSliderAnnotations = Instantiate<SliderAnnotations>(transform);
    }

    #endregion

    #region Setup

    public void Setup(Color color, string decText, string incText, bool mirror) {
        leftSliderAnnotations.Setup(color, decText, incText);
        rightSliderAnnotations.Setup(color, mirror ? incText : decText, mirror ? decText : incText);
    }

    #endregion
}