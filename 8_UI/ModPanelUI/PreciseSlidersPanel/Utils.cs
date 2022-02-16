using UnityEngine;

namespace EasyOffset;

internal partial class ModPanelUI {
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
        return Mathf.Clamp(value, _preciseZOffsetSliderMin, _preciseZOffsetSliderMax);
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

    #region RotationConversions

    private static void ToReferenceSpace(
        Vector3 originalRotationEuler,
        Quaternion rotationReference,
        out float horizontal,
        out float vertical
    ) {
        var originalRotation = TransformUtils.RotationFromEuler(originalRotationEuler);
        var rotationDifference = Quaternion.Inverse(rotationReference) * originalRotation;
        var differenceDirection = TransformUtils.DirectionFromRotation(rotationDifference);
        var resultEulerRad = TransformUtils.OrthoToSphericalDirection(differenceDirection);

        horizontal = resultEulerRad.y * Mathf.Rad2Deg;
        vertical = resultEulerRad.x * Mathf.Rad2Deg;
    }

    private static Vector3 FromReferenceSpace(
        Vector3 originalRotationEuler,
        Quaternion rotationReference,
        float horizontal,
        float vertical
    ) {
        var originalRotation = TransformUtils.RotationFromEuler(originalRotationEuler);
        var originalDirection = TransformUtils.DirectionFromEuler(originalRotationEuler);
        var additionalRotation = Quaternion.Euler(vertical, horizontal, 0.0f);
        var resultRotation = rotationReference * additionalRotation;
        var resultDirection = TransformUtils.DirectionFromRotation(resultRotation);
        var rotationDifference = Quaternion.FromToRotation(originalDirection, resultDirection);
        var finalRotation = rotationDifference * originalRotation;
        return TransformUtils.EulerFromRotation(finalRotation);
    }

    #endregion
}