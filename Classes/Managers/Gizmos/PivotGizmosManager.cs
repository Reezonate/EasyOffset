using System;
using EasyOffset.Configuration;
using EasyOffset.SyncedWithUnity;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace EasyOffset {
    [UsedImplicitly]
    public class PivotGizmosManager : IInitializable, IDisposable {
        #region Inject

        private readonly MainSettingsModelSO _mainSettingsModel;

        public PivotGizmosManager(
            MainSettingsModelSO mainSettingsModel
        ) {
            _mainSettingsModel = mainSettingsModel;
        }

        #endregion

        #region Init/Dispose

        private Pivot _leftHandPivot;
        private Pivot _rightHandPivot;

        public void Initialize() {
            Abomination.TransformsUpdatedEvent += OnTransformsUpdated;
            PluginConfig.AdjustmentModeChangedEvent += OnAdjustmentModeChanged;

            _leftHandPivot = CreatePivot();
            _rightHandPivot = CreatePivot();

            UpdateVisibility();
        }

        public void Dispose() {
            Abomination.TransformsUpdatedEvent -= OnTransformsUpdated;
            PluginConfig.AdjustmentModeChangedEvent -= OnAdjustmentModeChanged;

            Object.Destroy(_leftHandPivot.gameObject);
            Object.Destroy(_rightHandPivot.gameObject);
        }

        #endregion

        #region Visibility

        private bool _drawGizmos;

        private void UpdateVisibility() {
            var visible = PluginConfig.AdjustmentMode switch {
                AdjustmentMode.None => false,
                AdjustmentMode.Basic => true,
                AdjustmentMode.PivotOnly => true,
                AdjustmentMode.DirectionOnly => true,
                AdjustmentMode.DirectionAuto => false,
                AdjustmentMode.RoomOffset => false,
                _ => throw new ArgumentOutOfRangeException()
            };
            SetGizmosVisibility(visible);
        }

        private void SetGizmosVisibility(bool value) {
            _drawGizmos = value;
            _leftHandPivot.SetVisible(value);
            _rightHandPivot.SetVisible(value);
        }

        #endregion

        #region Events

        private void OnAdjustmentModeChanged(AdjustmentMode value) {
            UpdateVisibility();
        }

        private void OnTransformsUpdated(Vector3 leftPos, Quaternion leftRot, Vector3 rightPos, Quaternion rightRot) {
            if (!_drawGizmos) return;
            UpdateLeftGizmos(leftPos, leftRot);
            UpdateRightGizmos(rightPos, rightRot);
        }

        private void UpdateLeftGizmos(Vector3 rawControllerPosition, Quaternion rawControllerRotation) {
            if (!_drawGizmos) return;
            var pivotPosition = rawControllerPosition + rawControllerRotation * PluginConfig.LeftHandPivotPosition;
            TransformUtils.ApplyRoomOffsetToVector(_mainSettingsModel, ref pivotPosition);
            _leftHandPivot.SetValues(pivotPosition);
        }

        private void UpdateRightGizmos(Vector3 rawControllerPosition, Quaternion rawControllerRotation) {
            if (!_drawGizmos) return;
            var pivotPosition = rawControllerPosition + rawControllerRotation * PluginConfig.RightHandPivotPosition;
            TransformUtils.ApplyRoomOffsetToVector(_mainSettingsModel, ref pivotPosition);
            _rightHandPivot.SetValues(pivotPosition);
        }

        #endregion

        #region Utils

        private static Pivot CreatePivot() {
            var pivotObject = Object.Instantiate(BundleLoader.PivotPrefab);
            return pivotObject.GetComponent<Pivot>();
        }

        #endregion
    }
}