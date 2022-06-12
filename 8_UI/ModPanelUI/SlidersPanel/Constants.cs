using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;

namespace EasyOffset;

internal partial class SlidersPanel {
    #region ZOffsetSliderSettings

    [UIValue("z-offset-slider-min")] [UsedImplicitly]
    private float _directZOffsetSliderMin = -0.2f;

    [UIValue("z-offset-slider-max")] [UsedImplicitly]
    private float _directZOffsetSliderMax = 0.25f;

    [UIValue("z-offset-slider-increment")] [UsedImplicitly]
    private float _directZOffsetSliderIncrement = 0.001f;

    [UIAction("z-offset-slider-formatter")]
    [UsedImplicitly]
    private string DirectZOffsetSliderFormatter(float value) => $"{value * 100f:F1} cm";

    #endregion

    #region Position slider settings

    [UIValue("pos-slider-min")] [UsedImplicitly]
    private float _posSliderMin = -0.15f;

    [UIValue("pos-slider-max")] [UsedImplicitly]
    private float _posSliderMax = 0.15f;

    [UIValue("pos-slider-increment")] [UsedImplicitly]
    private float _posSliderIncrement = 0.001f;

    [UIAction("pos-slider-formatter")]
    [UsedImplicitly]
    private string PosSliderFormatter(float value) => $"{value * 100f:F1} cm";

    #endregion

    #region Rotation slider settings

    [UIValue("rot-90-slider-min")] [UsedImplicitly]
    private float _rot90SliderMin = -89.5f;

    [UIValue("rot-90-slider-max")] [UsedImplicitly]
    private float _rot90SliderMax = 89.5f;

    [UIValue("rot-180-slider-min")] [UsedImplicitly]
    private float _rot180SliderMin = -180.0f;

    [UIValue("rot-180-slider-max")] [UsedImplicitly]
    private float _rot180SliderMax = 180.0f;

    [UIValue("rot-slider-increment")] [UsedImplicitly]
    private float _rotSliderIncrement = 0.1f;

    [UIAction("rot-slider-formatter")]
    [UsedImplicitly]
    private string RotSliderFormatter(float value) => $"{value:F1}°";

    [UIAction("rot-curve-left-slider-formatter")]
    [UsedImplicitly]
    private string RotCurveLeftSliderFormatter(float value) => $"{-value:F1}°";

    [UIAction("rot-curve-right-slider-formatter")]
    [UsedImplicitly]
    private string RotCurveRightSliderFormatter(float value) => $"{value:F1}°";

    #endregion

    #region ButtonsSettings

    private const float ButtonPromptDelaySeconds = 2.0f;
    private const string ButtonPromptText = "<color=#ff5555>Sure?</color>";

    private const string ResetButtonIdleText = "Reset";
    private const string LeftMirrorButtonIdleText = "Mirror to Right ►";
    private const string RightMirrorButtonIdleText = "<pos=-0.4em>◄ Mirror to Left";

    #endregion
}