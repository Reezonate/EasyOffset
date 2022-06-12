using UnityEngine;

namespace EasyOffset;

internal partial class SlidersPanel {
    #region Step Up/Down

    private static float StepUp(float currentValue, float stepSize) {
        var currentStep = Mathf.RoundToInt(currentValue / stepSize);
        return stepSize * (currentStep + 1);
    }

    private static float StepDown(float currentValue, float stepSize) {
        var currentStep = Mathf.RoundToInt(currentValue / stepSize);
        return stepSize * (currentStep - 1);
    }

    #endregion

    #region SlidersClamping

    private float ClampZOffsetSliderValue(float value) {
        return Mathf.Clamp(value, _directZOffsetSliderMin, _directZOffsetSliderMax);
    }

    private float ClampPosSliderValue(float value) {
        return Mathf.Clamp(value, _posSliderMin, _posSliderMax);
    }

    private float ClampRot90SliderValue(float value) {
        return Mathf.Clamp(value, _rot90SliderMin, _rot90SliderMax);
    }

    private static float ClampRot180SliderValue(float value) {
        return value switch {
            >= 180 => -360 + value,
            <= -180 => 360 - value,
            _ => value
        };
    }

    #endregion
}