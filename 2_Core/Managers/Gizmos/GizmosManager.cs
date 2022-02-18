using System;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace EasyOffset {
    [UsedImplicitly]
    public class GizmosManager : IInitializable, IDisposable, ILateTickable {
        #region Init/Dispose

        public GizmosController LeftHandGizmosController;
        public GizmosController RightHandGizmosController;

        public void Initialize() {
            LeftHandGizmosController = Object.Instantiate(BundleLoader.GizmosController).GetComponent<GizmosController>();
            RightHandGizmosController = Object.Instantiate(BundleLoader.GizmosController).GetComponent<GizmosController>();

            PluginConfig.IsInMainMenuChangedEvent += OnIsInMainMenuChanged;
            PluginConfig.HideControllersChangedEvent += OnHideControllersChanged;
            PluginConfig.AdjustmentModeChangedEvent += OnAdjustmentModeChanged;
            PluginConfig.ControllerTypeChangedEvent += OnControllerTypeChanged;
            Abomination.TransformsUpdatedEvent += OnControllerTransformsChanged;
            PluginConfig.ConfigWasChangedEvent += OnConfigWasChanged;
            ModPanelUI.PreciseChangeStartedEvent += OnPreciseChangeStarted;
            ModPanelUI.PreciseChangeFinishedEvent += OnPreciseChangeFinished;

            OnControllerTypeChanged(PluginConfig.SelectedControllerType);
            OnConfigWasChanged();
        }

        public void Dispose() {
            PluginConfig.IsInMainMenuChangedEvent -= OnIsInMainMenuChanged;
            PluginConfig.HideControllersChangedEvent -= OnHideControllersChanged;
            PluginConfig.AdjustmentModeChangedEvent -= OnAdjustmentModeChanged;
            PluginConfig.ControllerTypeChangedEvent -= OnControllerTypeChanged;
            PluginConfig.ConfigWasChangedEvent -= OnConfigWasChanged;
            PluginConfig.IsModPanelVisibleChangedEvent -= OnModPanelVisibleChanged;

            ModPanelUI.PreciseChangeStartedEvent -= OnPreciseChangeStarted;
            ModPanelUI.PreciseChangeFinishedEvent -= OnPreciseChangeFinished;

            Abomination.TransformsUpdatedEvent -= OnControllerTransformsChanged;
        }

        #endregion

        #region LateTick

        private bool _configUpdateRequired;

        public void LateTick() {
            if (_configUpdateRequired) {
                UpdateConfigValues();
                _configUpdateRequired = false;
            }

            var mainCamera = Camera.main;
            if (mainCamera == null) return;
            var cameraWorldPosition = mainCamera.transform.position;
            RightHandGizmosController.SetCameraPosition(cameraWorldPosition);
            LeftHandGizmosController.SetCameraPosition(cameraWorldPosition);
        }

        #endregion

        #region Events

        private void OnPreciseChangeStarted(Hand? hand) {
            switch (hand) {
                case Hand.Left:
                    LeftHandGizmosController.SetOrthonormalBasisFocus(true);
                    LeftHandGizmosController.SetSphericalBasisFocus(true);
                    LeftHandGizmosController.SetPreviousRotation(PluginConfig.LeftSaberRotation, true, PluginConfig.LeftSaberReferenceRotation);
                    break;
                case Hand.Right:
                    RightHandGizmosController.SetOrthonormalBasisFocus(true);
                    RightHandGizmosController.SetSphericalBasisFocus(true);
                    RightHandGizmosController.SetPreviousRotation(PluginConfig.RightSaberRotation, true, PluginConfig.RightSaberReferenceRotation);
                    break;
                case null: return;
                default: throw new ArgumentOutOfRangeException(nameof(hand), hand, null);
            }
        }

        private void OnPreciseChangeFinished(Hand? hand) {
            LeftHandGizmosController.SetOrthonormalBasisFocus(false);
            LeftHandGizmosController.SetSphericalBasisFocus(false);
            LeftHandGizmosController.SetPreviousRotation(Quaternion.identity, false);

            RightHandGizmosController.SetOrthonormalBasisFocus(false);
            RightHandGizmosController.SetSphericalBasisFocus(false);
            RightHandGizmosController.SetPreviousRotation(Quaternion.identity, false);
        }

        private void OnConfigWasChanged() {
            _configUpdateRequired = true;
        }

        private void UpdateConfigValues() {
            LeftHandGizmosController.SetConfigValues(
                PluginConfig.LeftSaberPivotPosition,
                PluginConfig.LeftSaberRotation,
                PluginConfig.LeftSaberZOffset,
                PluginConfig.LeftSaberHasReference,
                PluginConfig.LeftSaberReferenceRotation
            );
            RightHandGizmosController.SetConfigValues(
                PluginConfig.RightSaberPivotPosition,
                PluginConfig.RightSaberRotation,
                PluginConfig.RightSaberZOffset,
                PluginConfig.RightSaberHasReference,
                PluginConfig.RightSaberReferenceRotation
            );
        }

        private void OnControllerTransformsChanged(ReeTransform leftHandTransform, ReeTransform rightHandTransform) {
            var leftPos = leftHandTransform.Position;
            var leftRot = leftHandTransform.Rotation;
            var rightPos = rightHandTransform.Position;
            var rightRot = rightHandTransform.Rotation;
            TransformUtils.ApplyRoomOffset(ref leftPos, ref leftRot);
            TransformUtils.ApplyRoomOffset(ref rightPos, ref rightRot);
            LeftHandGizmosController.SetControllerTransform(leftPos, leftRot);
            RightHandGizmosController.SetControllerTransform(rightPos, rightRot);
        }

        private void OnModPanelVisibleChanged(bool value) {
            if (!value) return;
            UpdateVisibility();
        }

        private void OnIsInMainMenuChanged(bool value) {
            UpdateVisibility();
        }

        private void OnHideControllersChanged(bool value) {
            UpdateVisibility();
        }

        private void OnAdjustmentModeChanged(AdjustmentMode adjustmentMode) {
            UpdateVisibility();
        }

        private void OnControllerTypeChanged(ControllerType controllerType) {
            LeftHandGizmosController.SetControllerType(controllerType, Hand.Left);
            RightHandGizmosController.SetControllerType(controllerType, Hand.Right);
            UpdateVisibility();
        }

        #endregion

        #region Visibility

        private void UpdateVisibility() {
            GetVisibilityValues(
                out var isPivotVisible,
                out var isSphericalBasisVisible,
                out var isOrthonormalBasisVisible,
                out var isOrthonormalBasisPointerVisible,
                out var isControllerModelVisible,
                out var isSwingPreviewVisible,
                out var isLegacyGimbalVisible,
                out var isReferenceRotationVisible
            );

            LeftHandGizmosController.SetVisibility(
                isPivotVisible,
                isSphericalBasisVisible,
                isOrthonormalBasisVisible,
                isOrthonormalBasisPointerVisible,
                isControllerModelVisible,
                isSwingPreviewVisible,
                isLegacyGimbalVisible,
                isReferenceRotationVisible
            );

            RightHandGizmosController.SetVisibility(
                isPivotVisible,
                isSphericalBasisVisible,
                isOrthonormalBasisVisible,
                isOrthonormalBasisPointerVisible,
                isControllerModelVisible,
                isSwingPreviewVisible,
                isLegacyGimbalVisible,
                isReferenceRotationVisible
            );
        }

        private static void GetVisibilityValues(
            out bool isPivotVisible,
            out bool isSphericalBasisVisible,
            out bool isOrthonormalBasisVisible,
            out bool isOrthonormalBasisPointerVisible,
            out bool isControllerModelVisible,
            out bool isSwingPreviewVisible,
            out bool isLegacyGimbalVisible,
            out bool isReferenceRotationVisible
        ) {
            isControllerModelVisible = PluginConfig.HideControllers ? PluginConfig.IsModPanelVisible : PluginConfig.IsInMainMenu;

            switch (PluginConfig.AdjustmentMode) {
                case AdjustmentMode.None:
                    isPivotVisible = false;
                    isOrthonormalBasisVisible = false;
                    isOrthonormalBasisPointerVisible = false;
                    isSphericalBasisVisible = false;
                    isSwingPreviewVisible = false;
                    isLegacyGimbalVisible = false;
                    isReferenceRotationVisible = false;
                    break;
                case AdjustmentMode.Basic:
                    isPivotVisible = true;
                    isOrthonormalBasisVisible = PluginConfig.SelectedControllerType == ControllerType.None;
                    isOrthonormalBasisPointerVisible = false;
                    isSphericalBasisVisible = false;
                    isSwingPreviewVisible = true;
                    isLegacyGimbalVisible = false;
                    isReferenceRotationVisible = false;
                    break;
                case AdjustmentMode.Position:
                    isPivotVisible = true;
                    isOrthonormalBasisVisible = true;
                    isOrthonormalBasisPointerVisible = true;
                    isSphericalBasisVisible = false;
                    isSwingPreviewVisible = true;
                    isLegacyGimbalVisible = false;
                    isReferenceRotationVisible = false;
                    break;
                case AdjustmentMode.Rotation:
                    isPivotVisible = true;
                    isOrthonormalBasisVisible = false;
                    isOrthonormalBasisPointerVisible = false;
                    isSphericalBasisVisible = true;
                    isSwingPreviewVisible = true;
                    isLegacyGimbalVisible = false;
                    isReferenceRotationVisible = true;
                    break;
                case AdjustmentMode.Precise:
                    isPivotVisible = false;
                    isOrthonormalBasisVisible = PluginConfig.SelectedControllerType == ControllerType.None;
                    isOrthonormalBasisPointerVisible = false;
                    isSphericalBasisVisible = false;
                    isSwingPreviewVisible = true;
                    isLegacyGimbalVisible = true;
                    isReferenceRotationVisible = false;
                    break;
                case AdjustmentMode.SwingBenchmark:
                    isPivotVisible = true;
                    isOrthonormalBasisVisible = false;
                    isOrthonormalBasisPointerVisible = false;
                    isSphericalBasisVisible = false;
                    isSwingPreviewVisible = false;
                    isLegacyGimbalVisible = false;
                    isReferenceRotationVisible = true;
                    break;
                case AdjustmentMode.RotationAuto:
                    isPivotVisible = true;
                    isOrthonormalBasisVisible = false;
                    isOrthonormalBasisPointerVisible = false;
                    isSphericalBasisVisible = true;
                    isSwingPreviewVisible = true;
                    isLegacyGimbalVisible = false;
                    isReferenceRotationVisible = false;
                    break;
                case AdjustmentMode.RoomOffset:
                    isPivotVisible = false;
                    isOrthonormalBasisVisible = false;
                    isOrthonormalBasisPointerVisible = false;
                    isSphericalBasisVisible = false;
                    isSwingPreviewVisible = false;
                    isLegacyGimbalVisible = false;
                    isReferenceRotationVisible = false;
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}