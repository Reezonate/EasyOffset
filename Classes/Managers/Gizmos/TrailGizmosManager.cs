using System;
using EasyOffset.Configuration;
using EasyOffset.SyncedWithUnity;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace EasyOffset {
    [UsedImplicitly]
    public class TrailGizmosManager : IInitializable, IDisposable {
        #region Constants

        private const float CloseOffset = 1.0f;
        private const float FarOffset = 5.0f;

        #endregion

        #region Inject

        private readonly MainSettingsModelSO _mainSettingsModel;

        public TrailGizmosManager(
            MainSettingsModelSO mainSettingsModel
        ) {
            _mainSettingsModel = mainSettingsModel;
        }

        #endregion

        #region Init/Dispose

        public void Initialize() {
            Abomination.TransformsUpdatedEvent += OnTransformsUpdated;
            PluginConfig.AdjustmentModeChangedEvent += OnAdjustmentModeChanged;
            UpdateVisibility();
        }

        public void Dispose() {
            Abomination.TransformsUpdatedEvent -= OnTransformsUpdated;
            PluginConfig.AdjustmentModeChangedEvent -= OnAdjustmentModeChanged;
            DestroyObjects();
        }

        #endregion

        #region Instances

        private bool _instantiated;

        private Trail _leftHandCloseTrail;
        private Trail _leftHandFarTrail;
        private Trail _rightHandCloseTrail;
        private Trail _rightHandFarTrail;

        private void DestroyObjects() {
            if (!_instantiated) return;
            Object.Destroy(_leftHandCloseTrail.gameObject);
            Object.Destroy(_leftHandFarTrail.gameObject);
            Object.Destroy(_rightHandCloseTrail.gameObject);
            Object.Destroy(_rightHandFarTrail.gameObject);
            _instantiated = false;
        }

        private void SpawnObjects() {
            if (_instantiated) return;
            _leftHandCloseTrail = InstantiateTrail();
            _leftHandFarTrail = InstantiateTrail();
            _rightHandCloseTrail = InstantiateTrail();
            _rightHandFarTrail = InstantiateTrail();
            _instantiated = true;
        }

        private static Trail InstantiateTrail() {
            var gameObject = Object.Instantiate(BundleLoader.TrailPrefab);
            Object.DontDestroyOnLoad(gameObject);
            return gameObject.GetComponent<Trail>();
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
                AdjustmentMode.DirectionAuto => true,
                AdjustmentMode.RoomOffset => false,
                _ => throw new ArgumentOutOfRangeException()
            };
            SetGizmosVisibility(visible);
        }

        private void SetGizmosVisibility(bool value) {
            _drawGizmos = value;
            if (value) {
                SpawnObjects();
            } else {
                DestroyObjects();
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
            var normal = rotation * PluginConfig.LeftHandSaberDirection;

            TransformUtils.ApplyRoomOffsetToVector(_mainSettingsModel, ref pivotPosition);

            _leftHandCloseTrail.SetValues(pivotPosition + normal * CloseOffset, normal);
            _leftHandFarTrail.SetValues(pivotPosition + normal * FarOffset, normal);
        }

        private void UpdateRightGizmos(Vector3 position, Quaternion rotation) {
            var pivotPosition = TransformUtils.LocalToWorldVector(PluginConfig.RightHandPivotPosition, position, rotation);
            var normal = rotation * PluginConfig.RightHandSaberDirection;

            TransformUtils.ApplyRoomOffsetToVector(_mainSettingsModel, ref pivotPosition);

            _rightHandCloseTrail.SetValues(pivotPosition + normal * CloseOffset, normal);
            _rightHandFarTrail.SetValues(pivotPosition + normal * FarOffset, normal);
        }

        #endregion
    }
}