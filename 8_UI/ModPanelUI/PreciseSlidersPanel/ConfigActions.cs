using System;

namespace EasyOffset;

internal partial class ModPanelUI {
    #region Synchronization

    private int _syncCount;

    private void NotifySynchronizationRequired() {
        _syncCount = 2; //ISSUE: Slider formatter requires 2 frames after activation (1.18.3)
    }

    private void SynchronizationUpdate() {
        if (_syncCount <= 0) return;
        SyncConfigValues();
        _syncCount -= 1;
    }

    private void SyncConfigValues() {
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

    #region Reset

    private void PreciseReset(Hand hand) {
        switch (_precisePanelState) {
            case PrecisePanelState.Hidden: return;
            case PrecisePanelState.PositionOnly:
                PluginConfig.ResetOffsets(hand, true, false, false);
                break;
            case PrecisePanelState.RotationOnly:
                PluginConfig.ResetOffsets(hand, false, true, true);
                break;
            case PrecisePanelState.ZOffsetOnly:
            case PrecisePanelState.Full:
                PluginConfig.ResetOffsets(hand, true, true, false);
                break;
            default: throw new ArgumentOutOfRangeException();
        }
    }

    #endregion

    #region Mirror

    private void PreciseMirror(Hand mirrorSource) {
        switch (_precisePanelState) {
            case PrecisePanelState.Hidden: return;
            case PrecisePanelState.PositionOnly:
                PluginConfig.Mirror(mirrorSource, true, false);
                break;
            case PrecisePanelState.RotationOnly:
                PluginConfig.Mirror(mirrorSource, false, true);
                break;
            case PrecisePanelState.ZOffsetOnly:
            case PrecisePanelState.Full:
                PluginConfig.Mirror(mirrorSource, true, true);
                break;
            default: throw new ArgumentOutOfRangeException();
        }
    }

    #endregion

    #region RecalculateRotations

    private void CalculateControllerSpaceRotations() {
        if (PluginConfig.LeftSaberHasReference) {
            var leftCoordinates = FromReferenceSpace(
                PreciseLeftRotationEuler,
                PluginConfig.LeftSaberReferenceRotation,
                PreciseLeftRotHor,
                PreciseLeftRotVert
            );

            PreciseLeftRotX = _preciseLeftRotXCurrent = _preciseLeftRotXTarget = leftCoordinates.x;
            PreciseLeftRotY = _preciseLeftRotYCurrent = _preciseLeftRotYTarget = leftCoordinates.y;
            PreciseLeftRotZ = _preciseLeftRotZCurrent = _preciseLeftRotZTarget = leftCoordinates.z;
        }
        
        if (PluginConfig.RightSaberHasReference) {
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
    }

    private void CalculateReferenceSpaceRotations() {
        if (PluginConfig.LeftSaberHasReference) {
            ToReferenceSpace(PreciseLeftRotationEuler, PluginConfig.LeftSaberReferenceRotation, out var leftHorizontal, out var leftVertical);
            PreciseLeftRotHor = leftHorizontal;
            PreciseLeftRotVert = leftVertical;
        } else {
            PreciseLeftRotHor = 0.0f;
            PreciseLeftRotVert = 0.0f;
        }

        if (PluginConfig.RightSaberHasReference) {
            ToReferenceSpace(PreciseRightRotationEuler, PluginConfig.RightSaberReferenceRotation, out var rightHorizontal, out var rightVertical);
            PreciseRightRotHor = rightHorizontal;
            PreciseRightRotVert = rightVertical;
        } else {
            PreciseRightRotHor = 0.0f;
            PreciseRightRotVert = 0.0f;
        }
    }

    #endregion
}