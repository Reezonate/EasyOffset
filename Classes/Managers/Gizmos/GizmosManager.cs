using System;
using EasyOffset.Configuration;
using EasyOffset.SyncedWithUnity;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace EasyOffset {
    [UsedImplicitly]
    public class GizmosManager : IInitializable, IDisposable, ITickable {
        #region Inject

        private MainSettingsModelSO _mainSettingsModel;

        public GizmosManager(MainSettingsModelSO mainSettingsModel) {
            _mainSettingsModel = mainSettingsModel;
        }

        #endregion

        #region Init/Dispose

        public GizmosController LeftHandGizmosController;
        public GizmosController RightHandGizmosController;

        public void Initialize() {
            LeftHandGizmosController = InstantiateGizmosController();
            RightHandGizmosController = InstantiateGizmosController();

            PluginConfig.AdjustmentModeChangedEvent += OnAdjustmentModeChanged;
            PluginConfig.ControllerTypeChangedEvent += OnControllerTypeChanged;
            Abomination.TransformsUpdatedEvent += OnControllerTransformsChanged;

            UpdateVisibility();
            OnControllerTypeChanged(PluginConfig.DisplayControllerType);
        }

        public void Dispose() {
            Object.Destroy(LeftHandGizmosController.gameObject);
            Object.Destroy(RightHandGizmosController.gameObject);

            PluginConfig.AdjustmentModeChangedEvent -= OnAdjustmentModeChanged;
            PluginConfig.ControllerTypeChangedEvent -= OnControllerTypeChanged;
            Abomination.TransformsUpdatedEvent -= OnControllerTransformsChanged;
        }

        #endregion

        #region Tick

        public void Tick() {
            LeftHandGizmosController.SetPivotPosition(PluginConfig.LeftHandPivotPosition);
            LeftHandGizmosController.SetSaberDirection(PluginConfig.LeftHandSaberDirection);

            RightHandGizmosController.SetPivotPosition(PluginConfig.RightHandPivotPosition);
            RightHandGizmosController.SetSaberDirection(PluginConfig.RightHandSaberDirection);

            var mainCamera = Camera.main;
            if (mainCamera == null) return;
            var cameraWorldPosition = mainCamera.transform.position;
            RightHandGizmosController.SetCameraPosition(cameraWorldPosition);
            LeftHandGizmosController.SetCameraPosition(cameraWorldPosition);
        }

        #endregion

        #region Events

        private void OnControllerTransformsChanged(Vector3 leftPos, Quaternion leftRot, Vector3 rightPos, Quaternion rightRot) {
            TransformUtils.ApplyRoomOffset(_mainSettingsModel, ref leftPos, ref leftRot);
            TransformUtils.ApplyRoomOffset(_mainSettingsModel, ref rightPos, ref rightRot);
            LeftHandGizmosController.SetControllerTransform(leftPos, leftRot);
            RightHandGizmosController.SetControllerTransform(rightPos, rightRot);
        }

        private void OnAdjustmentModeChanged(AdjustmentMode adjustmentMode) {
            UpdateVisibility();
        }

        private void OnControllerTypeChanged(ControllerType controllerType) {
            LeftHandGizmosController.SetControllerType(controllerType, Hand.Left);
            RightHandGizmosController.SetControllerType(controllerType, Hand.Right);
        }

        #endregion

        #region Visibility

        private void UpdateVisibility() {
            GetVisibilityValues(
                PluginConfig.AdjustmentMode,
                out var isPivotVisible,
                out var isSphericalBasisVisible,
                out var isOrthonormalBasisVisible,
                out var isControllerModelVisible,
                out var isSwingPreviewVisible
            );

            LeftHandGizmosController.SetVisibility(
                isPivotVisible,
                isSphericalBasisVisible,
                isOrthonormalBasisVisible,
                isControllerModelVisible,
                isSwingPreviewVisible
            );

            RightHandGizmosController.SetVisibility(
                isPivotVisible,
                isSphericalBasisVisible,
                isOrthonormalBasisVisible,
                isControllerModelVisible,
                isSwingPreviewVisible
            );
        }

        private static void GetVisibilityValues(
            AdjustmentMode adjustmentMode,
            out bool isPivotVisible,
            out bool isSphericalBasisVisible,
            out bool isOrthonormalBasisVisible,
            out bool isControllerModelVisible,
            out bool isSwingPreviewVisible
        ) {
            switch (adjustmentMode) {
                case AdjustmentMode.None:
                    isPivotVisible = false;
                    isOrthonormalBasisVisible = false;
                    isSphericalBasisVisible = false;
                    isControllerModelVisible = true;
                    isSwingPreviewVisible = false;
                    break;
                case AdjustmentMode.Basic:
                    isPivotVisible = true;
                    isOrthonormalBasisVisible = false;
                    isSphericalBasisVisible = false;
                    isControllerModelVisible = true;
                    isSwingPreviewVisible = true;
                    break;
                case AdjustmentMode.PivotOnly:
                    isPivotVisible = true;
                    isOrthonormalBasisVisible = true;
                    isSphericalBasisVisible = false;
                    isControllerModelVisible = true;
                    isSwingPreviewVisible = false;
                    break;
                case AdjustmentMode.DirectionOnly:
                    isPivotVisible = true;
                    isOrthonormalBasisVisible = false;
                    isSphericalBasisVisible = true;
                    isControllerModelVisible = true;
                    isSwingPreviewVisible = true;
                    break;
                case AdjustmentMode.DirectionAuto:
                    isPivotVisible = true;
                    isOrthonormalBasisVisible = false;
                    isSphericalBasisVisible = true;
                    isControllerModelVisible = true;
                    isSwingPreviewVisible = true;
                    break;
                case AdjustmentMode.RoomOffset:
                    isPivotVisible = false;
                    isOrthonormalBasisVisible = false;
                    isSphericalBasisVisible = false;
                    isControllerModelVisible = true;
                    isSwingPreviewVisible = false;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(adjustmentMode), adjustmentMode, null);
            }
        }

        #endregion

        #region Utils

        private static GizmosController InstantiateGizmosController() {
            var gameObject = Object.Instantiate(BundleLoader.GizmosController);
            Object.DontDestroyOnLoad(gameObject);
            return gameObject.GetComponent<GizmosController>();
        }

        #endregion
    }
}