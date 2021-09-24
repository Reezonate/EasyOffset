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

        public virtual string ConfigVersion { get; set; } = CurrentConfigVersion;

        #endregion

        #region UI Lock
        
        public virtual bool UILock { get; set; } = Defaults.UILock;

        #endregion

        #region AssignedButton
        
        public virtual string AssignedButton { get; set; } = Defaults.AssignedButton;

        #endregion

        #region DisplayControllerType

        public virtual string DisplayControllerType { get; set; } = Defaults.DisplayControllerType;

        #endregion

        #region UseFreeHand

        public virtual bool UseFreeHand { get; set; } = Defaults.UseFreeHand;

        #endregion

        #region RightHand

        public virtual float RightHandPivotX { get; set; } = Defaults.PivotX;
        public virtual float RightHandPivotY { get; set; } = Defaults.PivotY;
        public virtual float RightHandPivotZ { get; set; } = Defaults.PivotZ;

        public virtual float RightHandSaberDirectionX { get; set; } = Defaults.SaberDirectionX;
        public virtual float RightHandSaberDirectionY { get; set; } = Defaults.SaberDirectionY;
        public virtual float RightHandSaberDirectionZ { get; set; } = Defaults.SaberDirectionZ;

        public virtual float RightHandZOffset { get; set; } = Defaults.ZOffset;

        #endregion

        #region LeftHand

        public virtual float LeftHandPivotX { get; set; } = Defaults.PivotX;
        public virtual float LeftHandPivotY { get; set; } = Defaults.PivotY;
        public virtual float LeftHandPivotZ { get; set; } = Defaults.PivotZ;

        public virtual float LeftHandSaberDirectionX { get; set; } = Defaults.SaberDirectionX;
        public virtual float LeftHandSaberDirectionY { get; set; } = Defaults.SaberDirectionY;
        public virtual float LeftHandSaberDirectionZ { get; set; } = Defaults.SaberDirectionZ;

        public virtual float LeftHandZOffset { get; set; } = Defaults.ZOffset;

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