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
        PreciseLeftButtonsActive = PluginConfig.LeftSaberHasReference;
        PreciseLeftHelpActive = !PluginConfig.LeftSaberHasReference;

        PreciseRightPivotPosition = PluginConfig.RightSaberPivotPosition;
        PreciseRightRotationEuler = PluginConfig.RightSaberRotationEuler;
        PreciseRightZOffset = PluginConfig.RightSaberZOffset;
        PreciseRightRotationReferenceInteractable = PluginConfig.RightSaberHasReference;
        PreciseRightButtonsActive = PluginConfig.RightSaberHasReference;
        PreciseRightHelpActive = !PluginConfig.RightSaberHasReference;

        RecalculateReferenceSpaceRotations();
    }

    #endregion

    #region ApplyPreciseConfig

    private void ApplyPreciseConfig() {
        PluginConfig.SetSaberOffsets(
            PreciseLeftPivotPosition,
            PreciseLeftRotation,
            PreciseLeftZOffset,
            PreciseRightPivotPosition,
            PreciseRightRotation,
            PreciseRightZOffset
        );
    }

    #endregion

    #region OnResetButtonPressed

    private void OnResetButtonPressed(Hand hand) {
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

    #region OnMirrorButtonPressed

    private void OnMirrorButtonPressed(Hand mirrorSource) {
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

    #region RecalculateControllerSpaceRotations

    private void RecalculateControllerSpaceRotations() {
        if (PluginConfig.LeftSaberHasReference) {
            var leftCoordinates = TransformUtils.FromReferenceSpace(
                PreciseLeftRotation,
                PluginConfig.LeftSaberReferenceRotation,
                PreciseLeftRotHor,
                PreciseLeftRotVert
            );

            PreciseLeftRotX = _preciseLeftRotXCurrent = _preciseLeftRotXTarget = leftCoordinates.x;
            PreciseLeftRotY = _preciseLeftRotYCurrent = _preciseLeftRotYTarget = leftCoordinates.y;
            PreciseLeftRotZ = _preciseLeftRotZCurrent = _preciseLeftRotZTarget = leftCoordinates.z;
        }

        if (PluginConfig.RightSaberHasReference) {
            var rightCoordinates = TransformUtils.FromReferenceSpace(
                PreciseRightRotation,
                PluginConfig.RightSaberReferenceRotation,
                PreciseRightRotHor,
                PreciseRightRotVert
            );

            PreciseRightRotX = _preciseRightRotXCurrent = _preciseRightRotXTarget = rightCoordinates.x;
            PreciseRightRotY = _preciseRightRotYCurrent = _preciseRightRotYTarget = rightCoordinates.y;
            PreciseRightRotZ = _preciseRightRotZCurrent = _preciseRightRotZTarget = rightCoordinates.z;
        }
    }

    #endregion

    #region RecalculateReferenceSpaceRotations

    private void RecalculateReferenceSpaceRotations() {
        if (PluginConfig.LeftSaberHasReference) {
            TransformUtils.ToReferenceSpace(
                PreciseLeftRotation,
                PluginConfig.LeftSaberReferenceRotation,
                out var leftHorizontal,
                out var leftVertical
            );
            PreciseLeftRotHor = leftHorizontal;
            PreciseLeftRotVert = leftVertical;
        } else {
            PreciseLeftRotHor = 0.0f;
            PreciseLeftRotVert = 0.0f;
        }

        if (PluginConfig.RightSaberHasReference) {
            TransformUtils.ToReferenceSpace(
                PreciseRightRotation,
                PluginConfig.RightSaberReferenceRotation,
                out var rightHorizontal,
                out var rightVertical
            );
            PreciseRightRotHor = rightHorizontal;
            PreciseRightRotVert = rightVertical;
        } else {
            PreciseRightRotHor = 0.0f;
            PreciseRightRotVert = 0.0f;
        }
    }

    #endregion
}