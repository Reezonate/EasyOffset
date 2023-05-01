using System;
using UnityEngine;
using UnityEngine.XR;

namespace EasyOffset;

public static class TransformUtils {
    #region Oculus VR Mode Offsets

    private static readonly Quaternion OculusModeRemoveRotation = Quaternion.Euler(-40f, 0f, 0f);
    private static readonly Vector3 OculusModeRemoveTranslation = new Vector3(0f, 0f, 0.055f);

    public static void RemoveOculusModeOffsets(ref Vector3 controllerPosition, ref Quaternion controllerRotation) {
        controllerRotation *= OculusModeRemoveRotation;
        controllerPosition += controllerRotation * OculusModeRemoveTranslation;
    }

    // ReSharper disable once Unity.InefficientPropertyAccess
    private static void RemoveOculusModeOffsets(Transform transform) {
        transform.rotation *= OculusModeRemoveRotation;
        transform.position += transform.rotation * OculusModeRemoveTranslation;
    }

    #endregion

    #region AdjustControllerTransform

    internal static void AdjustControllerTransformOculus(
        XRNode node,
        Transform transform
    ) {
        switch (node) {
            case XRNode.LeftHand:
                RemoveOculusModeOffsets(transform);
                AdjustLeftControllerTransform(transform);
                break;
            case XRNode.RightHand:
                RemoveOculusModeOffsets(transform);
                AdjustRightControllerTransform(transform);
                break;
            case XRNode.LeftEye: return;
            case XRNode.RightEye: return;
            case XRNode.CenterEye: return;
            case XRNode.Head: return;
            case XRNode.GameController: return;
            case XRNode.TrackingReference: return;
            case XRNode.HardwareTracker: return;
            default: throw new ArgumentOutOfRangeException(nameof(node), node, null);
        }
    }

    internal static void AdjustControllerTransform(
        XRNode node,
        Transform transform
    ) {
        switch (node) {
            case XRNode.LeftHand:
                AdjustLeftControllerTransform(transform);
                break;
            case XRNode.RightHand:
                AdjustRightControllerTransform(transform);
                break;
            case XRNode.LeftEye: return;
            case XRNode.RightEye: return;
            case XRNode.CenterEye: return;
            case XRNode.Head: return;
            case XRNode.GameController: return;
            case XRNode.TrackingReference: return;
            case XRNode.HardwareTracker: return;
            default: throw new ArgumentOutOfRangeException(nameof(node), node, null);
        }
    }

    private static void AdjustLeftControllerTransform(
        Transform transform
    ) {
        if (PluginConfig.SmoothingEnabled) {
            var position = Abomination.LeftControllerTransform.Position;
            var rotation = Abomination.LeftControllerTransform.Rotation;
            ApplyRoomOffset(ref position, ref rotation);
            transform.position = position;
            transform.rotation = rotation;
        }

        transform.Translate(PluginConfig.LeftSaberTranslation);
        transform.rotation *= PluginConfig.LeftSaberRotation;
    }

    private static void AdjustRightControllerTransform(
        Transform transform
    ) {
        if (PluginConfig.SmoothingEnabled) {
            var position = Abomination.RightControllerTransform.Position;
            var rotation = Abomination.RightControllerTransform.Rotation;
            ApplyRoomOffset(ref position, ref rotation);
            transform.position = position;
            transform.rotation = rotation;
        }

        transform.Translate(PluginConfig.RightSaberTranslation);
        transform.rotation *= PluginConfig.RightSaberRotation;
    }

    #endregion

    #region Mirror

    public static Quaternion MirrorRotation(Quaternion rotation) {
        rotation.ToAngleAxis(out var angle, out var axis);
        return Quaternion.AngleAxis(-angle, MirrorVector(axis));
    }

    public static Vector3 MirrorVector(Vector3 vector) {
        return new Vector3(-vector.x, vector.y, vector.z);
    }

    #endregion

    #region Clamp

    public static Vector3 ClampEulerAngles(Vector3 eulerAngles) {
        return new Vector3(
            ClampRotation(eulerAngles.x),
            ClampRotation(eulerAngles.y),
            ClampRotation(eulerAngles.z)
        );
    }

    private static float ClampRotation(float value) {
        return value switch {
            > 180 => -360 + value,
            < -180 => 360 - value,
            _ => value
        };
    }

    #endregion

    #region Rotation <-> Direction

    public static Vector3 DirectionFromRotation(Quaternion rotation) {
        return rotation * Vector3.forward;
    }

    public static Quaternion RotationFromDirection(Vector3 direction) {
        return Quaternion.LookRotation(direction, Vector3.up);
    }

    #endregion

    #region Rotation <-> Euler

    public static Quaternion RotationFromEuler(Vector3 eulerAngles) {
        return Quaternion.Euler(eulerAngles);
    }

    public static Vector3 EulerFromRotation(Quaternion rotation) {
        return ClampEulerAngles(rotation.eulerAngles);
    }

    #endregion

    #region Direction <-> Euler

    public static Vector3 DirectionFromEuler(Vector3 eulerAngles) {
        var rotation = RotationFromEuler(eulerAngles);
        return DirectionFromRotation(rotation);
    }

    public static Vector3 EulerFromDirection(Vector3 direction) {
        var rotation = RotationFromDirection(direction);
        return EulerFromRotation(rotation);
    }

    #endregion

    #region GetSaberTranslation

    public static Vector3 GetSaberTranslation(
        Vector3 pivotPosition,
        Quaternion saberRotation,
        float zOffset
    ) {
        var rotationCausedOffset = saberRotation * new Vector3(0.0f, 0.0f, zOffset);
        return pivotPosition + rotationCausedOffset;
    }

    #endregion

    #region ApplyRoomOffsets

    public static void ApplyRoomOffset(ref Vector3 position, ref Quaternion rotation) {
        var roomRotation = Quaternion.Euler(0, PluginConfig.MainSettingsModel.roomRotation, 0);
        position = PluginConfig.MainSettingsModel.roomCenter + roomRotation * position;
        rotation = roomRotation * rotation;
    }

    public static void ApplyRoomOffsetToVector(ref Vector3 position) {
        var roomRotation = Quaternion.Euler(0, PluginConfig.MainSettingsModel.roomRotation, 0);
        position = PluginConfig.MainSettingsModel.roomCenter + roomRotation * position;
    }

    public static void ApplyRoomOffsetToDirection(ref Vector3 direction) {
        var roomRotation = Quaternion.Euler(0, PluginConfig.MainSettingsModel.roomRotation, 0);
        direction = roomRotation * direction;
    }

    #endregion

    #region RemoveRoomOffset

    public static void RemoveRoomOffsetFromVector(ref Vector3 position) {
        var roomRotation = Quaternion.Euler(0, PluginConfig.MainSettingsModel.roomRotation, 0);
        position = Quaternion.Inverse(roomRotation) * (position - PluginConfig.MainSettingsModel.roomCenter);
    }

    public static void RemoveRoomOffsetFromDirection(ref Vector3 direction) {
        var roomRotation = Quaternion.Euler(0, PluginConfig.MainSettingsModel.roomRotation, 0);
        direction = Quaternion.Inverse(roomRotation) * direction;
    }

    #endregion

    #region Spherical

    public static Vector3 SphericalToOrthoDirection(Vector2 sphericalDirectionRadians) {
        var xzScale = Mathf.Cos(sphericalDirectionRadians.x);
        var x = Mathf.Sin(sphericalDirectionRadians.y) * xzScale;
        var z = Mathf.Cos(sphericalDirectionRadians.y) * xzScale;
        var y = -Mathf.Sin(sphericalDirectionRadians.x);
        return new Vector3(x, y, z);
    }

    public static Vector2 OrthoToSphericalDirection(Vector3 orthoDirection) {
        var yDir = new Vector2(orthoDirection.z, orthoDirection.x);
        var yAngleRad = Mathf.Atan2(yDir.y, yDir.x);
        var xAngleRad = -Mathf.Atan2(orthoDirection.y, yDir.magnitude);
        return new Vector2(xAngleRad, yAngleRad);
    }

    #endregion

    #region Controller <-> Reference

    public static void ToReferenceSpace(
        Quaternion originalRotation,
        Quaternion rotationReference,
        out float curve,
        out float balance
    ) {
        var rotationDifference = Quaternion.Inverse(rotationReference) * originalRotation;
        var differenceDirection = DirectionFromRotation(rotationDifference);
        var resultEulerRad = OrthoToSphericalDirection(differenceDirection);

        curve = -resultEulerRad.y * Mathf.Rad2Deg;
        balance = resultEulerRad.x * Mathf.Rad2Deg;
    }

    public static Vector3 FromReferenceSpace(
        Quaternion originalRotation,
        Quaternion rotationReference,
        float curve,
        float balance
    ) {
        var additionalRotation = Quaternion.Euler(balance, -curve, 0.0f);
        var resultRotation = rotationReference * additionalRotation;
        var alignedRotation = AlignForwardVectors(originalRotation, resultRotation);
        return EulerFromRotation(alignedRotation);
    }

    #endregion

    #region AlignForwardVectors

    public static Quaternion AlignForwardVectors(Quaternion sourceRotation, Quaternion targetRotation) {
        var sourceDirection = DirectionFromRotation(sourceRotation);
        var targetDirection = DirectionFromRotation(targetRotation);
        var relativeRotation = Quaternion.FromToRotation(sourceDirection, targetDirection);
        return relativeRotation * sourceRotation;
    }

    #endregion
}