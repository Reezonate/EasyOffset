using UnityEngine;

namespace EasyOffset;

internal static class ConfigConversions {
    #region Built-in offsets

    #region DefaultControllerOffsets

    private static readonly Vector3 DefaultPositionOffset = new Vector3(0.0f, -0.008f, 0.0f);
    private static readonly Vector3 DefaultRotationOffset = new Vector3(-4.3f, 0.0f, 0.0f);

    private static readonly ReeTransform DefaultXRNodeOffset = new ReeTransform(
        Vector3.zero,
        Quaternion.identity
    );

    #endregion

    #region ValveControllerOffsets

    private static readonly Vector3 ValvePositionOffset = new Vector3(0.0f, 0.022f, -0.01f);
    private static readonly Vector3 ValveRotationOffset = new Vector3(-16.3f, 0.0f, 0.0f);

    private static readonly ReeTransform ValveXRNodeOffset = new ReeTransform(
        Vector3.zero,
        Quaternion.identity
    );

    #endregion

    #region OculusOffsets

    private static readonly Vector3 OculusPositionOffset = new Vector3(0.0f, 0.0f, 0.055f);
    private static readonly Vector3 OculusRotationOffset = new Vector3(-40f, 0.0f, 0.0f);

    private static readonly ReeTransform OculusXRNodeOffset = new ReeTransform(
        OculusPositionOffset,
        Quaternion.Euler(OculusRotationOffset)
    );

    #endregion

    #region GetBuiltInOffsets

    private static void GetBuiltInOffsets(
        bool isValveController, bool isVRModeOculus,
        out Vector3 positionOffset,
        out Vector3 eulerOffset,
        out ReeTransform xrNodeOffset
    ) {
        if (isVRModeOculus) {
            positionOffset = OculusPositionOffset;
            eulerOffset = OculusRotationOffset;
            xrNodeOffset = OculusXRNodeOffset;
            return;
        }

        if (isValveController) {
            positionOffset = ValvePositionOffset;
            eulerOffset = ValveRotationOffset;
            xrNodeOffset = ValveXRNodeOffset;
            return;
        }

        positionOffset = DefaultPositionOffset;
        eulerOffset = DefaultRotationOffset;
        xrNodeOffset = DefaultXRNodeOffset;
    }

    #endregion

    #endregion

    #region OneHandConversion

    private static void OneHandConversion(
        bool isValveController,
        bool isVRModeOculus,
        float zOffset,
        Vector3 gripPosition,
        Vector3 gripRotation,
        out Vector3 pivotPosition,
        out Quaternion saberRotation
    ) {
        GetBuiltInOffsets(isValveController, isVRModeOculus, out var positionOffset, out var eulerOffset, out var xrNodeOffset);

        gripRotation += eulerOffset;
        saberRotation = Quaternion.Euler(gripRotation);
        saberRotation = xrNodeOffset.WorldToLocalRotation(saberRotation);

        gripPosition += positionOffset;
        pivotPosition = saberRotation * gripPosition;

        var offset = saberRotation * new Vector3(0, 0, zOffset);
        pivotPosition -= offset + xrNodeOffset.Position;
    }

    private static void OneHandInverseConversion(
        bool isValveController,
        bool isVRModeOculus,
        Vector3 saberTranslation,
        Quaternion saberRotation,
        out Vector3 gripPosition,
        out Vector3 gripRotation
    ) {
        GetBuiltInOffsets(isValveController, isVRModeOculus, out var positionOffset, out var eulerOffset, out var xrNodeOffset);

        saberTranslation = xrNodeOffset.LocalToWorldDirection(saberTranslation + xrNodeOffset.Position);

        saberRotation = xrNodeOffset.LocalToWorldRotation(saberRotation);
        var direction = TransformUtils.DirectionFromRotation(saberRotation);
        var zeroZRotation = TransformUtils.RotationFromDirection(direction);
        gripRotation = TransformUtils.EulerFromRotation(zeroZRotation) - eulerOffset;

        gripPosition = Quaternion.Inverse(zeroZRotation) * saberTranslation;
        gripPosition -= positionOffset;
    }

    #endregion

    #region Universal

    public static void Universal(
        bool isVRModeOculus,
        Vector3 controllerWorldPosition,
        Quaternion controllerWorldRotation,
        Vector3 saberWorldPosition,
        Quaternion saberWorldRotation,
        float zOffset,
        out Vector3 pivotPosition,
        out Quaternion saberRotation
    ) {
        if (isVRModeOculus) TransformUtils.RemoveOculusModeOffsets(ref controllerWorldPosition, ref controllerWorldRotation);

        var controllerTransform = new ReeTransform(controllerWorldPosition, controllerWorldRotation);
        saberRotation = controllerTransform.WorldToLocalRotation(saberWorldRotation);
        pivotPosition = controllerTransform.WorldToLocalPosition(saberWorldPosition);
        pivotPosition += saberRotation * new Vector3(0, 0, -zOffset);
    }

    #endregion

    #region FromBaseGame

    public static void FromBaseGame(
        bool isValveController,
        bool isVRModeOculus,
        float zOffset,
        Vector3 position,
        Vector3 rotation,
        out Vector3 leftPivotPosition,
        out Vector3 rightPivotPosition,
        out Quaternion leftSaberRotation,
        out Quaternion rightSaberRotation
    ) {
        OneHandConversion(
            isValveController,
            isVRModeOculus,
            zOffset,
            position,
            rotation,
            out rightPivotPosition,
            out rightSaberRotation
        );

        leftPivotPosition = TransformUtils.MirrorVector(rightPivotPosition);
        leftSaberRotation = TransformUtils.MirrorRotation(rightSaberRotation);
    }

    #endregion

    #region ToBaseGame

    public static void ToBaseGame(
        bool isValveController,
        bool isVRModeOculus,
        Vector3 saberTranslation,
        Quaternion saberRotation,
        out Vector3 position,
        out Vector3 rotation
    ) {
        OneHandInverseConversion(
            isValveController,
            isVRModeOculus,
            saberTranslation,
            saberRotation,
            out position,
            out rotation
        );
    }

    #endregion
    
    #region ToTailor

    public static void ToTailor(
        bool isValveController,
        bool isVRModeOculus,
        Vector3 leftSaberTranslation,
        Quaternion leftSaberRotation,
        Vector3 rightSaberTranslation,
        Quaternion rightSaberRotation,
        out Vector3 gripLeftPosition,
        out Vector3 gripRightPosition,
        out Vector3 gripLeftRotation,
        out Vector3 gripRightRotation
    ) {
        OneHandInverseConversion(
            isValveController,
            isVRModeOculus,
            leftSaberTranslation,
            leftSaberRotation,
            out gripLeftPosition,
            out gripLeftRotation
        );

        OneHandInverseConversion(
            isValveController,
            isVRModeOculus,
            rightSaberTranslation,
            rightSaberRotation,
            out gripRightPosition,
            out gripRightRotation
        );
    }

    #endregion
}