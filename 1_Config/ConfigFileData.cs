using System.Runtime.CompilerServices;
using IPA.Config.Stores;
using JetBrains.Annotations;
using UnityEngine;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]

namespace EasyOffset {
    [UsedImplicitly]
    internal class ConfigFileData {
        public const string CurrentConfigVersion = "2.0";
        public static ConfigFileData Instance { get; set; }

        #region ConfigVersion

        public string ConfigVersion = CurrentConfigVersion;

        #endregion

        #region Enabled

        public bool Enabled = ConfigDefaults.Enabled;

        #endregion

        #region HideControllers

        public bool HideControllers = ConfigDefaults.HideControllers;

        #endregion

        #region MinimalWarningLevel

        public string MinimalWarningLevel = ConfigDefaults.MinimalWarningLevel;

        #endregion

        #region RoomOffset

        public bool AllowRoomXChange = ConfigDefaults.AllowRoomXChange;
        public bool AllowRoomYChange = ConfigDefaults.AllowRoomYChange;
        public bool AllowRoomZChange = ConfigDefaults.AllowRoomZChange;

        #endregion

        #region UI Lock

        public bool UILock = ConfigDefaults.UILock;

        #endregion

        #region AssignedButton

        public string AssignedButton = ConfigDefaults.AssignedButton;

        #endregion

        #region DisplayControllerType

        public string DisplayControllerType = ConfigDefaults.DisplayControllerType;

        #endregion

        #region UseFreeHand

        public bool UseFreeHand = ConfigDefaults.UseFreeHand;

        #endregion

        #region ScaleConstants

        private const float PositionUnitScale = 0.01f;

        #endregion

        #region LeftSaberPivotPosition

        [UsedImplicitly] public Vector3 LeftSaberPivotPosition = ConfigDefaults.LeftSaberPivotPosition / PositionUnitScale;

        public Vector3 GetLeftSaberPivotPositionInMeters() {
            return LeftSaberPivotPosition * PositionUnitScale;
        }

        public void SetLeftSaberPivotPositionInMeters(Vector3 value) {
            LeftSaberPivotPosition = value / PositionUnitScale;
        }

        #endregion

        #region LeftSaberRotationEuler

        public Vector3 LeftSaberRotationEuler = TransformUtils.EulerFromRotation(ConfigDefaults.LeftSaberRotation);

        #endregion

        #region LeftSaberZOffset

        [UsedImplicitly] public float LeftSaberZOffset = ConfigDefaults.ZOffset / PositionUnitScale;

        public float GetLeftSaberZOffsetInMeters() {
            return LeftSaberZOffset * PositionUnitScale;
        }

        public void SetLeftSaberZOffsetInMeters(float value) {
            LeftSaberZOffset = value / PositionUnitScale;
        }

        #endregion

        #region LeftSaberReferenceRotation

        public bool LeftSaberHasReference = ConfigDefaults.LeftSaberHasReference;
        public Quaternion LeftSaberReferenceRotation = ConfigDefaults.LeftSaberReferenceRotation;

        #endregion

        #region RightSaberPivotPosition

        [UsedImplicitly] public Vector3 RightSaberPivotPosition = ConfigDefaults.RightSaberPivotPosition / PositionUnitScale;

        public Vector3 GetRightSaberPivotPositionInMeters() {
            return RightSaberPivotPosition * PositionUnitScale;
        }

        public void SetRightSaberPivotPositionInMeters(Vector3 value) {
            RightSaberPivotPosition = value / PositionUnitScale;
        }

        #endregion

        #region RightSaberRotationEuler

        public Vector3 RightSaberRotationEuler = TransformUtils.EulerFromRotation(ConfigDefaults.RightSaberRotation);

        #endregion

        #region RightSaberZOffset

        [UsedImplicitly] public float RightSaberZOffset = ConfigDefaults.ZOffset / PositionUnitScale;

        public float GetRightSaberZOffsetInMeters() {
            return RightSaberZOffset * PositionUnitScale;
        }

        public void SetRightSaberZOffsetInMeters(float value) {
            RightSaberZOffset = value / PositionUnitScale;
        }

        #endregion

        #region RightSaberReferenceRotation

        public bool RightSaberHasReference = ConfigDefaults.RightSaberHasReference;
        public Quaternion RightSaberReferenceRotation = ConfigDefaults.RightSaberReferenceRotation;

        #endregion

        #region Generic IPA stuff

        /// <summary>
        /// This is called whenever BSIPA reads the config from disk (including when file changes are detected).
        /// </summary>
        public virtual void OnReload() {
            // Do stuff after config is read from disk.
        }

        /// <summary>
        /// Call this to force BSIPA to update the config file. This is also called by BSIPA if it detects the file was modified.
        /// </summary>
        public virtual void Changed() {
            // Do stuff when the config is changed.
        }

        /// <summary>
        /// Call this to have BSIPA copy the values from <paramref name="other"/> into this config.
        /// </summary>
        public virtual void CopyFrom(ConfigFileData other) {
            // This instance's members populated from other
        }

        #endregion
    }
}