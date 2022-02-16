using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal partial class ModPanelUI {
    #region ZOffsetSliderSettings

    [UIValue("z-offset-slider-min")] [UsedImplicitly]
    private float _preciseZOffsetSliderMin = -0.2f;

    [UIValue("z-offset-slider-max")] [UsedImplicitly]
    private float _preciseZOffsetSliderMax = 0.25f;

    [UIValue("z-offset-slider-increment")] [UsedImplicitly]
    private float _preciseZOffsetSliderIncrement = 0.001f;

    [UIAction("z-offset-slider-formatter")]
    [UsedImplicitly]
    private string PreciseZOffsetSliderFormatter(float value) => $"{value * 100f:F1} cm";

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

    #endregion

    #region ButtonsSettings

    private const float ButtonPromptDelaySeconds = 2.0f;

    private const string ResetButtonIdleText = "Reset";
    private const string ResetButtonPromptText = "<color=#ff5555>Sure?</color>";

    private const string LeftMirrorButtonIdleText = "- Mirror to Right >";
    private const string LeftMirrorButtonPromptText = "<color=#ff5555>-<mspace=1.9em> </mspace>Sure?<mspace=1.9em> </mspace>></color>";

    private const string RightMirrorButtonIdleText = "< Mirror to Left -";
    private const string RightMirrorButtonPromptText = "<color=#ff5555><<mspace=1.67em> </mspace>Sure?<mspace=1.67em> </mspace>-</color>";

    #endregion
}