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
            PluginConfig.GripButtonActionChangedEvent += OnGripButtonActionChanged;

            _leftHandPivot = CreatePivot();
            _rightHandPivot = CreatePivot();

            UpdateVisibility();
        }

        public void Dispose() {
            Abomination.TransformsUpdatedEvent -= OnTransformsUpdated;
            PluginConfig.GripButtonActionChangedEvent -= OnGripButtonActionChanged;

            Object.Destroy(_leftHandPivot.gameObject);
            Object.Destroy(_rightHandPivot.gameObject);
        }

        #endregion

        #region Visibility

        private bool _drawGizmos;

        private void UpdateVisibility() {
            switch (PluginConfig.GripButtonAction) {
                case GripButtonAction.None:
                    SetGizmosVisibility(false);
                    break;
                case GripButtonAction.FullGrip:
                    SetGizmosVisibility(true);
                    break;
                case GripButtonAction.PivotOnly:
                    SetGizmosVisibility(true);
                    break;
                case GripButtonAction.DirectionOnly:
                    SetGizmosVisibility(true);
                    break;
                case GripButtonAction.RoomOffset:
                    SetGizmosVisibility(false);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private void SetGizmosVisibility(bool value) {
            _drawGizmos = value;
            _leftHandPivot.SetVisible(value);
            _rightHandPivot.SetVisible(value);
        }

        #endregion

        #region Events

        private void OnGripButtonActionChanged(GripButtonAction value) {
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