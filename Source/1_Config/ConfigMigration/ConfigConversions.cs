using UnityEngine;
using UnityEngine.XR;

namespace EasyOffset;

internal static class ConfigConversions {
    #region GetBuiltInOffset

    private static ReeTransform GetBuiltInOffset(IVRPlatformHelper vrPlatformHelper, XRNode xrNode) {
        vrPlatformHelper.TryGetPoseOffsetForNode(xrNode, out var pose);
        return new ReeTransform(pose.position, pose.rotation);
    }

    #endregion

    #region OneHandConversion

    private static void OneHandConversion(
        IVRPlatformHelper vrPlatformHelper,
        XRNode xrNode,
        float zOffset,
        Vector3 gripPosition,
        Vector3 gripRotation,
        out Vector3 pivotPosition,
        out Quaternion saberRotation
    ) {
        var xrNodeOffset = GetBuiltInOffset(vrPlatformHelper, xrNode);

        saberRotation = Quaternion.Euler(gripRotation);
        saberRotation = xrNodeOffset.WorldToLocalRotation(saberRotation);

        pivotPosition = saberRotation * gripPosition;

        var offset = saberRotation * new Vector3(0, 0, zOffset);
        pivotPosition -= offset + xrNodeOffset.Position;
    }

    private static void OneHandInverseConversion(
        IVRPlatformHelper vrPlatformHelper,
        XRNode xrNode,
        Vector3 saberTranslation,
        Quaternion saberRotation,
        out Vector3 gripPosition,
        out Vector3 gripRotation
    ) {
        var xrNodeOffset = GetBuiltInOffset(vrPlatformHelper, xrNode);

        saberTranslation = xrNodeOffset.LocalToWorldDirection(saberTranslation + xrNodeOffset.Position);

        saberRotation = xrNodeOffset.LocalToWorldRotation(saberRotation);
        var direction = TransformUtils.DirectionFromRotation(saberRotation);
        var zeroZRotation = TransformUtils.RotationFromDirection(direction);
        gripRotation = TransformUtils.EulerFromRotation(zeroZRotation);

        gripPosition = Quaternion.Inverse(zeroZRotation) * saberTranslation;
    }

    #endregion

    #region Universal

    public static void Universal(
        Vector3 controllerWorldPosition,
        Quaternion controllerWorldRotation,
        Vector3 saberWorldPosition,
        Quaternion saberWorldRotation,
        float zOffset,
        out Vector3 pivotPosition,
        out Quaternion saberRotation
    ) {
        var controllerTransform = new ReeTransform(controllerWorldPosition, controllerWorldRotation);
        saberRotation = controllerTransform.WorldToLocalRotation(saberWorldRotation);
        pivotPosition = controllerTransform.WorldToLocalPosition(saberWorldPosition);
        pivotPosition += saberRotation * new Vector3(0, 0, -zOffset);
    }

    #endregion

    #region FromBaseGame

    public static void FromBaseGame(
        IVRPlatformHelper vrPlatformHelper,
        float zOffset,
        Vector3 position,
        Vector3 rotation,
        out Vector3 leftPivotPosition,
        out Vector3 rightPivotPosition,
        out Quaternion leftSaberRotation,
        out Quaternion rightSaberRotation
    ) {
        OneHandConversion(
            vrPlatformHelper,
            XRNode.RightHand,
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
        IVRPlatformHelper vrPlatformHelper,
        XRNode xrNode,
        Vector3 saberTranslation,
        Quaternion saberRotation,
        out Vector3 position,
        out Vector3 rotation
    ) {
        OneHandInverseConversion(
            vrPlatformHelper,
            xrNode,
            saberTranslation,
            saberRotation,
            out position,
            out rotation
        );
    }

    #endregion
}