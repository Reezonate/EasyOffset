using System;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace EasyOffset {
    [UsedImplicitly]
    internal class DirectAdjustmentModeManager : IInitializable, IDisposable {
        #region Events Subscription

        public void Initialize() {
            PluginConfig.ConfigWasChangedEvent += OnMainConfigWasChanged;
            OnMainConfigWasChanged();

            Subscribe(LeftZOffset);
            Subscribe(LeftPosX);
            Subscribe(LeftPosY);
            Subscribe(LeftPosZ);
            Subscribe(LeftRotX);
            Subscribe(LeftRotY);
            Subscribe(LeftRotZ);
            Subscribe(LeftCurve);
            Subscribe(LeftBalance);

            Subscribe(RightZOffset);
            Subscribe(RightPosX);
            Subscribe(RightPosY);
            Subscribe(RightPosZ);
            Subscribe(RightRotX);
            Subscribe(RightRotY);
            Subscribe(RightRotZ);
            Subscribe(RightCurve);
            Subscribe(RightBalance);
        }

        public void Dispose() {
            PluginConfig.ConfigWasChangedEvent -= OnMainConfigWasChanged;

            Unsubscribe(LeftZOffset);
            Unsubscribe(LeftPosX);
            Unsubscribe(LeftPosY);
            Unsubscribe(LeftPosZ);
            Unsubscribe(LeftRotX);
            Unsubscribe(LeftRotY);
            Unsubscribe(LeftRotZ);
            Unsubscribe(LeftCurve);
            Unsubscribe(LeftBalance);

            Unsubscribe(RightZOffset);
            Unsubscribe(RightPosX);
            Unsubscribe(RightPosY);
            Unsubscribe(RightPosZ);
            Unsubscribe(RightRotX);
            Unsubscribe(RightRotY);
            Unsubscribe(RightRotZ);
            Unsubscribe(RightCurve);
            Unsubscribe(RightBalance);
        }

        private static void Subscribe(DirectModeVariable variable) {
            variable.SmoothChangeStartedEvent += OnSmoothChangeChangeStarted;
            variable.SmoothChangeFinishedEvent += OnSmoothChangeFinished;
            variable.ChangedFromUIEvent += OnValueChangedFromUI;
        }

        private static void Unsubscribe(DirectModeVariable variable) {
            variable.SmoothChangeStartedEvent -= OnSmoothChangeChangeStarted;
            variable.SmoothChangeFinishedEvent -= OnSmoothChangeFinished;
            variable.ChangedFromUIEvent -= OnValueChangedFromUI;
        }

        #endregion

        #region Events

        public static event Action<Hand?> DirectChangeStartedEvent;
        public static event Action<Hand?> DirectChangeFinishedEvent;

        private static void OnSmoothChangeChangeStarted(SliderValueType valueType, Hand hand) {
            PluginConfig.CreateUndoStep($"Change {hand} {valueType}");
            DirectChangeStartedEvent?.Invoke(hand);
        }

        private static void OnSmoothChangeFinished(SliderValueType valueType, Hand hand) {
            DirectChangeFinishedEvent?.Invoke(hand);
        }

        private static void OnValueChangedFromUI(SliderValueType valueType, float value) {
            switch (valueType) {
                case SliderValueType.ZOffset:
                case SliderValueType.PositionX:
                case SliderValueType.PositionY:
                case SliderValueType.PositionZ:
                    break;
                case SliderValueType.RotationX:
                case SliderValueType.RotationY:
                case SliderValueType.RotationZ:
                    RecalculateReferenceSpaceRotations();
                    break;
                case SliderValueType.Curve:
                case SliderValueType.Balance:
                    RecalculateControllerSpaceRotations();
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(valueType), valueType, null);
            }

            ApplyDirectConfig();
        }

        private static void OnMainConfigWasChanged() {
            ApplyMainConfig();
        }

        #endregion

        #region Apply Config

        private static void ApplyDirectConfig() {
            PluginConfig.SetSaberOffsets(
                LeftPivotPosition,
                LeftRotation,
                LeftZOffset.GetValue(),
                RightPivotPosition,
                RightRotation,
                RightZOffset.GetValue()
            );
        }

        private static void ApplyMainConfig() {
            LeftPivotPosition = PluginConfig.LeftSaberPivotPosition;
            LeftRotationEuler = PluginConfig.LeftSaberRotationEuler;
            LeftZOffset.SetValueFromCode(PluginConfig.LeftSaberZOffset);

            RightPivotPosition = PluginConfig.RightSaberPivotPosition;
            RightRotationEuler = PluginConfig.RightSaberRotationEuler;
            RightZOffset.SetValueFromCode(PluginConfig.RightSaberZOffset);

            RecalculateReferenceSpaceRotations();
        }

        #endregion

        #region RecalculateControllerSpaceRotations

        private static void RecalculateControllerSpaceRotations() {
            if (PluginConfig.LeftSaberHasReference) {
                LeftRotationEuler = TransformUtils.FromReferenceSpace(
                    LeftRotation,
                    PluginConfig.LeftSaberReferenceRotation,
                    LeftCurve.GetValue(),
                    LeftBalance.GetValue()
                );
            }

            if (PluginConfig.RightSaberHasReference) {
                RightRotationEuler = TransformUtils.FromReferenceSpace(
                    RightRotation,
                    PluginConfig.RightSaberReferenceRotation,
                    RightCurve.GetValue(),
                    RightBalance.GetValue()
                );
            }
        }

        #endregion

        #region RecalculateReferenceSpaceRotations

        private static void RecalculateReferenceSpaceRotations() {
            if (PluginConfig.LeftSaberHasReference) {
                TransformUtils.ToReferenceSpace(
                    LeftRotation,
                    PluginConfig.LeftSaberReferenceRotation,
                    out var leftCurve,
                    out var leftBalance
                );
                LeftCurve.SetValueFromCode(leftCurve);
                LeftBalance.SetValueFromCode(leftBalance);
            } else {
                LeftCurve.SetValueFromCode(0.0f);
                LeftBalance.SetValueFromCode(0.0f);
            }

            if (PluginConfig.RightSaberHasReference) {
                TransformUtils.ToReferenceSpace(
                    RightRotation,
                    PluginConfig.RightSaberReferenceRotation,
                    out var rightCurve,
                    out var rightBalance
                );
                RightCurve.SetValueFromCode(rightCurve);
                RightBalance.SetValueFromCode(rightBalance);
            } else {
                RightCurve.SetValueFromCode(0.0f);
                RightBalance.SetValueFromCode(0.0f);
            }
        }

        #endregion

        #region LeftHand

        #region Combined Position

        private static Vector3 LeftPivotPosition {
            get => new(LeftPosX.GetValue(), LeftPosY.GetValue(), LeftPosZ.GetValue());
            set {
                LeftPosX.SetValueFromCode(value.x);
                LeftPosY.SetValueFromCode(value.y);
                LeftPosZ.SetValueFromCode(value.z);
            }
        }

        #endregion

        #region Combined Rotation

        private static Quaternion LeftRotation => TransformUtils.RotationFromEuler(LeftRotationEuler);

        private static Vector3 LeftRotationEuler {
            get => new(LeftRotX.GetValue(), LeftRotY.GetValue(), LeftRotZ.GetValue());
            set {
                LeftRotX.SetValueFromCode(value.x);
                LeftRotY.SetValueFromCode(value.y);
                LeftRotZ.SetValueFromCode(value.z);
            }
        }

        #endregion

        #region UIControlledVariables

        public static readonly DirectModeVariable LeftZOffset = new(SliderValueType.ZOffset, Hand.Left);

        public static readonly DirectModeVariable LeftPosX = new(SliderValueType.PositionX, Hand.Left);
        public static readonly DirectModeVariable LeftPosY = new(SliderValueType.PositionY, Hand.Left);
        public static readonly DirectModeVariable LeftPosZ = new(SliderValueType.PositionZ, Hand.Left);

        public static readonly DirectModeVariable LeftRotX = new(SliderValueType.RotationX, Hand.Left);
        public static readonly DirectModeVariable LeftRotY = new(SliderValueType.RotationY, Hand.Left);
        public static readonly DirectModeVariable LeftRotZ = new(SliderValueType.RotationZ, Hand.Left);

        public static readonly DirectModeVariable LeftCurve = new(SliderValueType.Curve, Hand.Left);
        public static readonly DirectModeVariable LeftBalance = new(SliderValueType.Balance, Hand.Left);

        #endregion

        #endregion

        #region RightHand

        #region Combined Position

        private static Vector3 RightPivotPosition {
            get => new(RightPosX.GetValue(), RightPosY.GetValue(), RightPosZ.GetValue());
            set {
                RightPosX.SetValueFromCode(value.x);
                RightPosY.SetValueFromCode(value.y);
                RightPosZ.SetValueFromCode(value.z);
            }
        }

        #endregion

        #region Combined Rotation

        private static Quaternion RightRotation => TransformUtils.RotationFromEuler(RightRotationEuler);

        private static Vector3 RightRotationEuler {
            get => new(RightRotX.GetValue(), RightRotY.GetValue(), RightRotZ.GetValue());
            set {
                RightRotX.SetValueFromCode(value.x);
                RightRotY.SetValueFromCode(value.y);
                RightRotZ.SetValueFromCode(value.z);
            }
        }

        #endregion

        #region UIControlledVariables

        public static readonly DirectModeVariable RightZOffset = new(SliderValueType.ZOffset, Hand.Right);

        public static readonly DirectModeVariable RightPosX = new(SliderValueType.PositionX, Hand.Right);
        public static readonly DirectModeVariable RightPosY = new(SliderValueType.PositionY, Hand.Right);
        public static readonly DirectModeVariable RightPosZ = new(SliderValueType.PositionZ, Hand.Right);

        public static readonly DirectModeVariable RightRotX = new(SliderValueType.RotationX, Hand.Right);
        public static readonly DirectModeVariable RightRotY = new(SliderValueType.RotationY, Hand.Right);
        public static readonly DirectModeVariable RightRotZ = new(SliderValueType.RotationZ, Hand.Right);

        public static readonly DirectModeVariable RightCurve = new(SliderValueType.Curve, Hand.Right);
        public static readonly DirectModeVariable RightBalance = new(SliderValueType.Balance, Hand.Right);

        #endregion

        #endregion
    }
}