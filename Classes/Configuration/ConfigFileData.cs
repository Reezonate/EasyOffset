using System.Runtime.CompilerServices;
using IPA.Config.Stores;
using JetBrains.Annotations;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]

namespace EasyOffset.Configuration {
    [UsedImplicitly]
    public class ConfigFileData {
        private const string CurrentConfigVersion = "1.0";
        public static ConfigFileData Instance { get; set; }

        #region ConfigVersion

        public string ConfigVersion = CurrentConfigVersion;

        #endregion

        #region UI Lock

        public bool UILock = Defaults.UILock;

        #endregion

        #region AssignedButton

        public string AssignedButton = Defaults.AssignedButton;

        #endregion

        #region DisplayControllerType

        public string DisplayControllerType = Defaults.DisplayControllerType;

        #endregion

        #region UseFreeHand

        public bool UseFreeHand = Defaults.UseFreeHand;

        #endregion

        #region RightHand

        public float RightHandPivotX = Defaults.PivotX;
        public float RightHandPivotY = Defaults.PivotY;
        public float RightHandPivotZ = Defaults.PivotZ;

        public float RightHandSaberDirectionX = Defaults.SaberDirectionX;
        public float RightHandSaberDirectionY = Defaults.SaberDirectionY;
        public float RightHandSaberDirectionZ = Defaults.SaberDirectionZ;

        public float RightHandZOffset = Defaults.ZOffset;

        #endregion

        #region LeftHand

        public float LeftHandPivotX = Defaults.PivotX;
        public float LeftHandPivotY = Defaults.PivotY;
        public float LeftHandPivotZ = Defaults.PivotZ;

        public float LeftHandSaberDirectionX = Defaults.SaberDirectionX;
        public float LeftHandSaberDirectionY = Defaults.SaberDirectionY;
        public float LeftHandSaberDirectionZ = Defaults.SaberDirectionZ;

        public float LeftHandZOffset = Defaults.ZOffset;

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