using System;
using EasyOffset.Configuration;
using EasyOffset.SyncedWithUnity;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace EasyOffset {
    [UsedImplicitly]
    public class DirectionGridGizmosManager : IInitializable, IDisposable {
        #region Inject

        private readonly MainSettingsModelSO _mainSettingsModel;

        public DirectionGridGizmosManager(
            MainSettingsModelSO mainSettingsModel
        ) {
            _mainSettingsModel = mainSettingsModel;
        }

        #endregion

        #region Init/Dispose

        private DirectionGrid _leftHandDirectionGrid;
        private DirectionGrid _rightHandDirectionGrid;

        public void Initialize() {
            Abomination.TransformsUpdatedEvent += OnTransformsUpdated;
            PluginConfig.AdjustmentModeChangedEvent += OnAdjustmentModeChanged;

            _leftHandDirectionGrid = CreateDirectionGrid();
            _rightHandDirectionGrid = CreateDirectionGrid();

            UpdateVisibility();
        }

        public void Dispose() {
            Abomination.TransformsUpdatedEvent -= OnTransformsUpdated;
            PluginConfig.AdjustmentModeChangedEvent -= OnAdjustmentModeChanged;

            Object.Destroy(_leftHandDirectionGrid);
            Object.Destroy(_rightHandDirectionGrid);
        }

        #endregion

        #region Visibility

        private bool _drawGizmos;

        private void UpdateVisibility() {
            var visible = PluginConfig.AdjustmentMode switch {
                AdjustmentMode.None => false,
                AdjustmentMode.Basic => false,
                AdjustmentMode.PivotOnly => false,
                AdjustmentMode.DirectionOnly => true,
                AdjustmentMode.DirectionAuto => false,
                AdjustmentMode.RoomOffset => false,
                _ => throw new ArgumentOutOfRangeException()
            };
            SetGizmosVisibility(visible);
        }

        private void SetGizmosVisibility(bool value) {
            _drawGizmos = value;
            _leftHandDirectionGrid.SetVisible(value);
            _rightHandDirectionGrid.SetVisible(value);
        }

        #endregion

        #region Interaction

        private Vector3 _leftHandDirection;
        private Vector3 _rightHandDirection;

        public void SetState(Hand hand, bool isActive) {
            switch (hand) {
                case Hand.Left:
                    _leftHandDirectionGrid.SetTargetAlpha(isActive ? 1 : 0);
                    break;
                case Hand.Right:
                    _rightHandDirectionGrid.SetTargetAlpha(isActive ? 1 : 0);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(hand), hand, null);
            }
        }

        public void UpdateGizmos(
            Hand hand,
            Vector3 direction
        ) {
            switch (hand) {
                case Hand.Left:
                    _leftHandDirection = direction;
                    break;
                case Hand.Right:
                    _rightHandDirection = direction;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(hand), hand, null);
            }
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

        private void UpdateLeftGizmos(Vector3 position, Quaternion rotation) {
            var pivotPosition = TransformUtils.LocalToWorldVector(PluginConfig.LeftHandPivotPosition, position, rotation);
            var pointerDirection = rotation * PluginConfig.LeftHandSaberDirection;

            TransformUtils.ApplyRoomOffsetToVector(_mainSettingsModel, ref pivotPosition);

            _leftHandDirectionGrid.SetValues(pivotPosition, _leftHandDirection, pointerDirection);
        }

        private void UpdateRightGizmos(Vector3 position, Quaternion rotation) {
            var pivotPosition = TransformUtils.LocalToWorldVector(PluginConfig.RightHandPivotPosition, position, rotation);
            var pointerDirection = rotation * PluginConfig.RightHandSaberDirection;

            TransformUtils.ApplyRoomOffsetToVector(_mainSettingsModel, ref pivotPosition);

            _rightHandDirectionGrid.SetValues(pivotPosition, _rightHandDirection, pointerDirection);
        }

        #endregion

        #region Utils

        private static DirectionGrid CreateDirectionGrid() {
            var gridObject = Object.Instantiate(BundleLoader.DirectionGridPrefab);
            var directionGrid = gridObject.GetComponent<DirectionGrid>();
            return directionGrid;
        }

        #endregion
    }
}