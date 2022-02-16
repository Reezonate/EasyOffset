using System;

namespace EasyOffset;

internal partial class ModPanelUI {
    #region Synchronization

    private int _synchronizationsRequired;

    private void NotifySynchronizationRequired() {
        _synchronizationsRequired = 2; //ISSUE: Slider formatter requires 2 frames after activation (1.18.3)
    }

    private void SynchronizationUpdate() {
        if (_synchronizationsRequired <= 0) return;
        SyncPreciseUIValuesWithConfig();
        _synchronizationsRequired -= 1;
    }

    private void SyncPreciseUIValuesWithConfig() {
        PreciseLeftPivotPosition = PluginConfig.LeftSaberPivotPosition;
        PreciseLeftRotationEuler = PluginConfig.LeftSaberRotationEuler;
        PreciseLeftZOffset = PluginConfig.LeftSaberZOffset;
        PreciseLeftRotationReferenceInteractable = PluginConfig.LeftSaberHasReference;

        PreciseRightPivotPosition = PluginConfig.RightSaberPivotPosition;
        PreciseRightRotationEuler = PluginConfig.RightSaberRotationEuler;
        PreciseRightZOffset = PluginConfig.RightSaberZOffset;
        PreciseRightRotationReferenceInteractable = PluginConfig.RightSaberHasReference;

        CalculateReferenceSpaceRotations();
    }

    #endregion

    #region Apply

    private void ApplyPreciseConfig() {
        PluginConfig.ApplyPreciseModeValues(
            PreciseLeftPivotPosition,
            PreciseLeftRotationEuler,
            PreciseLeftZOffset,
            PreciseRightPivotPosition,
            PreciseRightRotationEuler,
            PreciseRightZOffset
        );
    }

    #endregion

    #region Mirror

    private void PreciseMirrorFromLeft() {
        switch (_precisePanelState) {
            case PrecisePanelState.Hidden: return;
            case PrecisePanelState.PositionOnly:
                PreciseRightZOffset = PreciseLeftZOffset;
                PreciseRightPosX = -PreciseLeftPosX;
                PreciseRightPosY = PreciseLeftPosY;
                PreciseRightPosZ = PreciseLeftPosZ;
                break;
            case PrecisePanelState.RotationOnly:
                PreciseRightRotX = PreciseLeftRotX;
                PreciseRightRotY = -PreciseLeftRotY;
                PreciseRightRotZ = -PreciseLeftRotZ;
                break;
            case PrecisePanelState.ZOffsetOnly:
            case PrecisePanelState.Full:
                PreciseRightZOffset = PreciseLeftZOffset;
                PreciseRightPosX = -PreciseLeftPosX;
                PreciseRightPosY = PreciseLeftPosY;
                PreciseRightPosZ = PreciseLeftPosZ;
                PreciseRightRotX = PreciseLeftRotX;
                PreciseRightRotY = -PreciseLeftRotY;
                PreciseRightRotZ = -PreciseLeftRotZ;
                break;
            default: throw new ArgumentOutOfRangeException();
        }

        CalculateReferenceSpaceRotations();
        ApplyPreciseConfig();
    }

    private void PreciseMirrorFromRight() {
        switch (_precisePanelState) {
            case PrecisePanelState.Hidden: return;
            case PrecisePanelState.PositionOnly:
                PreciseLeftZOffset = PreciseRightZOffset;
                PreciseLeftPosX = -PreciseRightPosX;
                PreciseLeftPosY = PreciseRightPosY;
                PreciseLeftPosZ = PreciseRightPosZ;
                break;
            case PrecisePanelState.RotationOnly:
                PreciseLeftRotX = PreciseRightRotX;
                PreciseLeftRotY = -PreciseRightRotY;
                PreciseLeftRotZ = -PreciseRightRotZ;
                break;
            case PrecisePanelState.ZOffsetOnly:
            case PrecisePanelState.Full:
                PreciseLeftZOffset = PreciseRightZOffset;
                PreciseLeftPosX = -PreciseRightPosX;
                PreciseLeftPosY = PreciseRightPosY;
                PreciseLeftPosZ = PreciseRightPosZ;
                PreciseLeftRotX = PreciseRightRotX;
                PreciseLeftRotY = -PreciseRightRotY;
                PreciseLeftRotZ = -PreciseRightRotZ;
                break;
            default: throw new ArgumentOutOfRangeException();
        }

        ApplyPreciseConfig();
    }

    #endregion

    #region RecalculateRotations

    private void CalculateControllerSpaceRotations() {
        var leftCoordinates = FromReferenceSpace(
            PreciseLeftRotationEuler,
            PluginConfig.LeftSaberReferenceRotation,
            PreciseLeftRotHor,
            PreciseLeftRotVert
        );

        PreciseLeftRotX = _preciseLeftRotXCurrent = _preciseLeftRotXTarget = leftCoordinates.x;
        PreciseLeftRotY = _preciseLeftRotYCurrent = _preciseLeftRotYTarget = leftCoordinates.y;
        PreciseLeftRotZ = _preciseLeftRotZCurrent = _preciseLeftRotZTarget = leftCoordinates.z;

        var rightCoordinates = FromReferenceSpace(
            PreciseRightRotationEuler,
            PluginConfig.RightSaberReferenceRotation,
            PreciseRightRotHor,
            PreciseRightRotVert
        );

        PreciseRightRotX = _preciseRightRotXCurrent = _preciseRightRotXTarget = rightCoordinates.x;
        PreciseRightRotY = _preciseRightRotYCurrent = _preciseRightRotYTarget = rightCoordinates.y;
        PreciseRightRotZ = _preciseRightRotZCurrent = _preciseRightRotZTarget = rightCoordinates.z;
    }

    private void CalculateReferenceSpaceRotations() {
        ToReferenceSpace(PreciseLeftRotationEuler, PluginConfig.LeftSaberReferenceRotation, out var leftHorizontal, out var leftVertical);
        PreciseLeftRotHor = _preciseLeftRotHorCurrent = _preciseLeftRotHorTarget = leftHorizontal;
        PreciseLeftRotVert = _preciseLeftRotVertCurrent = _preciseLeftRotVertTarget = leftVertical;

        ToReferenceSpace(PreciseRightRotationEuler, PluginConfig.RightSaberReferenceRotation, out var rightHorizontal, out var rightVertical);
        PreciseRightRotHor = _preciseRightRotHorCurrent = _preciseRightRotHorTarget = rightHorizontal;
        PreciseRightRotVert = _preciseRightRotVertCurrent = _preciseRightRotVertTarget = rightVertical;
    }

    #endregion
}