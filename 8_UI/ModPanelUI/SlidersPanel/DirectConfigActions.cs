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
        DirectLeftPivotPosition = PluginConfig.LeftSaberPivotPosition;
        DirectLeftRotationEuler = PluginConfig.LeftSaberRotationEuler;
        DirectLeftZOffset = PluginConfig.LeftSaberZOffset;
        DirectLeftRotationReferenceInteractable = PluginConfig.LeftSaberHasReference;
        DirectLeftButtonsActive = PluginConfig.LeftSaberHasReference;
        DirectLeftHelpActive = !PluginConfig.LeftSaberHasReference;

        DirectRightPivotPosition = PluginConfig.RightSaberPivotPosition;
        DirectRightRotationEuler = PluginConfig.RightSaberRotationEuler;
        DirectRightZOffset = PluginConfig.RightSaberZOffset;
        DirectRightRotationReferenceInteractable = PluginConfig.RightSaberHasReference;
        DirectRightButtonsActive = PluginConfig.RightSaberHasReference;
        DirectRightHelpActive = !PluginConfig.RightSaberHasReference;

        RecalculateReferenceSpaceRotations();
    }

    #endregion

    #region ApplyDirectConfig

    private void ApplyDirectConfig() {
        PluginConfig.SetSaberOffsets(
            DirectLeftPivotPosition,
            DirectLeftRotation,
            DirectLeftZOffset,
            DirectRightPivotPosition,
            DirectRightRotation,
            DirectRightZOffset
        );
    }

    #endregion

    #region OnResetButtonPressed

    private void OnResetButtonPressed(Hand hand) {
        switch (_directPanelState) {
            case DirectPanelState.Hidden: return;
            case DirectPanelState.PositionOnly:
                PluginConfig.ResetOffsets(hand, true, false, false);
                break;
            case DirectPanelState.RotationOnly:
                PluginConfig.ResetOffsets(hand, false, true, true);
                break;
            case DirectPanelState.ZOffsetOnly:
            case DirectPanelState.Full:
                PluginConfig.ResetOffsets(hand, true, true, false);
                break;
            default: throw new ArgumentOutOfRangeException();
        }
    }

    #endregion

    #region OnMirrorButtonPressed

    private void OnMirrorButtonPressed(Hand mirrorSource) {
        switch (_directPanelState) {
            case DirectPanelState.Hidden: return;
            case DirectPanelState.PositionOnly:
                PluginConfig.Mirror(mirrorSource, true, false);
                break;
            case DirectPanelState.RotationOnly:
                PluginConfig.Mirror(mirrorSource, false, true);
                break;
            case DirectPanelState.ZOffsetOnly:
            case DirectPanelState.Full:
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
                DirectLeftRotation,
                PluginConfig.LeftSaberReferenceRotation,
                DirectLeftRotHor,
                DirectLeftRotVert
            );

            DirectLeftRotX = _directLeftRotXCurrent = _directLeftRotXTarget = leftCoordinates.x;
            DirectLeftRotY = _directLeftRotYCurrent = _directLeftRotYTarget = leftCoordinates.y;
            DirectLeftRotZ = _directLeftRotZCurrent = _directLeftRotZTarget = leftCoordinates.z;
        }

        if (PluginConfig.RightSaberHasReference) {
            var rightCoordinates = TransformUtils.FromReferenceSpace(
                DirectRightRotation,
                PluginConfig.RightSaberReferenceRotation,
                DirectRightRotHor,
                DirectRightRotVert
            );

            DirectRightRotX = _directRightRotXCurrent = _directRightRotXTarget = rightCoordinates.x;
            DirectRightRotY = _directRightRotYCurrent = _directRightRotYTarget = rightCoordinates.y;
            DirectRightRotZ = _directRightRotZCurrent = _directRightRotZTarget = rightCoordinates.z;
        }
    }

    #endregion

    #region RecalculateReferenceSpaceRotations

    private void RecalculateReferenceSpaceRotations() {
        if (PluginConfig.LeftSaberHasReference) {
            TransformUtils.ToReferenceSpace(
                DirectLeftRotation,
                PluginConfig.LeftSaberReferenceRotation,
                out var leftHorizontal,
                out var leftVertical
            );
            DirectLeftRotHor = leftHorizontal;
            DirectLeftRotVert = leftVertical;
        } else {
            DirectLeftRotHor = 0.0f;
            DirectLeftRotVert = 0.0f;
        }

        if (PluginConfig.RightSaberHasReference) {
            TransformUtils.ToReferenceSpace(
                DirectRightRotation,
                PluginConfig.RightSaberReferenceRotation,
                out var rightHorizontal,
                out var rightVertical
            );
            DirectRightRotHor = rightHorizontal;
            DirectRightRotVert = rightVertical;
        } else {
            DirectRightRotHor = 0.0f;
            DirectRightRotVert = 0.0f;
        }
    }

    #endregion
}