using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using EasyOffset.Configuration;
using JetBrains.Annotations;

namespace EasyOffset.UI {
    public class ModPanelUI : NotifiableSingleton<ModPanelUI> {
        #region AdjustmentMode

        [UIValue("am-hint")] [UsedImplicitly] private string _adjustmentModeHint = "Hold assigned button and move your hand" +
                                                                                   "\n" +
                                                                                   "\nBasic - Easy adjustment mode for beginners" +
                                                                                   "\nPivot Only - Precise origin placement" +
                                                                                   "\nDirection Only - Saber rotation only" +
                                                                                   "\nDirection Auto - Automatic rotation (beta)" +
                                                                                   "\nRoom Offset - World Pulling locomotion";

        [UIValue("am-choices")] [UsedImplicitly]
        private List<object> _adjustmentModeChoices = AdjustmentModeUtils.AllNamesObjects.ToList();

        private string _adjustmentModeChoice = AdjustmentModeUtils.TypeToName(PluginConfig.AdjustmentMode);

        [UIValue("am-choice")]
        [UsedImplicitly]
        private string AdjustmentModeChoice {
            get => _adjustmentModeChoice;
            set {
                _adjustmentModeChoice = value;
                NotifyPropertyChanged();
            }
        }

        [UIAction("am-on-change")]
        [UsedImplicitly]
        private void AdjustmentModeOnChange(string selectedValue) {
            PluginConfig.AdjustmentMode = AdjustmentModeUtils.NameToType(selectedValue);
        }

        #endregion

        #region AssignedButton

        [UIValue("ab-choices")] [UsedImplicitly]
        private List<object> _assignedButtonChoices = ControllerButtonUtils.AllNamesObjects.ToList();

        [UIValue("ab-choice")] [UsedImplicitly]
        private string _assignedButtonChoice = ControllerButtonUtils.TypeToName(PluginConfig.AssignedButton);

        [UIAction("ab-on-change")]
        [UsedImplicitly]
        private void AssignedButtonOnChange(string selectedValue) {
            PluginConfig.AssignedButton = ControllerButtonUtils.NameToType(selectedValue);
        }

        #endregion

        #region Display Controller

        [UIValue("dc-choices")] [UsedImplicitly]
        private List<object> _displayControllerOptions = ControllerTypeUtils.AllNamesObjects.ToList();

        [UIValue("dc-choice")] [UsedImplicitly]
        private string _displayControllerValue = ControllerTypeUtils.TypeToName(PluginConfig.DisplayControllerType);

        [UIAction("dc-on-change")]
        [UsedImplicitly]
        private void OnDisplayControllerChange(string selectedValue) {
            PluginConfig.DisplayControllerType = ControllerTypeUtils.NameToType(selectedValue);
        }

        #endregion

        #region EnableMidPlayAdjustment

        [UIValue("mpa-value")] [UsedImplicitly]
        private bool _enableMidPlayAdjustmentValue = PluginConfig.EnableMidPlayAdjustment;

        [UIAction("mpa-on-change")]
        [UsedImplicitly]
        private void EnableMidPlayAdjustmentOnChange(bool value) {
            PluginConfig.EnableMidPlayAdjustment = value;
        }

        #endregion

        #region UseFreeHand

        [UIValue("ufh-value")] [UsedImplicitly]
        private bool _useFreeHandValue = PluginConfig.UseFreeHand;

        [UIAction("ufh-on-change")]
        [UsedImplicitly]
        private void UseFreeHandOnChange(bool value) {
            PluginConfig.UseFreeHand = value;
        }

        #endregion

        #region Hands

        [UIValue("zo-min")] [UsedImplicitly] private float _zOffsetSliderMin = -0.2f;

        [UIValue("zo-max")] [UsedImplicitly] private float _zOffsetSliderMax = 0.2f;

        [UIValue("zo-increment")] [UsedImplicitly]
        private float _zOffsetSliderIncrement = 0.01f;

        #region LeftHand

        #region Z Offset Slider

        [UIValue("lzo-value")]
        [UsedImplicitly]
        private float LeftZOffsetSliderValue {
            get => PluginConfig.LeftHandZOffset;
            set {
                PluginConfig.LeftHandZOffset = value;
                NotifyPropertyChanged();
            }
        }

        [UIAction("lzo-on-change")]
        [UsedImplicitly]
        private void OnLeftZOffsetValueChange(float value) {
            PluginConfig.LeftHandZOffset = value;
        }

        #endregion

        #region MirrorButton

        [UIAction("l2r-mirror-click")]
        [UsedImplicitly]
        private void OnLeftToRightMirrorClick() {
            PluginConfig.LeftToRightMirror();
            RightZOffsetSliderValue = PluginConfig.RightHandZOffset;
            NotifyPropertyChanged();
        }

        #endregion

        #endregion

        #region RightHand

        #region Z Offset Slider

        [UIValue("rzo-value")]
        [UsedImplicitly]
        private float RightZOffsetSliderValue {
            get => PluginConfig.RightHandZOffset;
            set {
                PluginConfig.RightHandZOffset = value;
                NotifyPropertyChanged();
            }
        }

        [UIAction("rzo-on-change")]
        [UsedImplicitly]
        private void OnRightZOffsetValueChange(float value) {
            PluginConfig.RightHandZOffset = value;
        }

        #endregion

        #region MirrorButton

        [UIAction("r2l-mirror-click")]
        [UsedImplicitly]
        private void OnRightToLeftMirrorClick() {
            PluginConfig.RightToLeftMirror();
            LeftZOffsetSliderValue = PluginConfig.LeftHandZOffset;
        }

        #endregion

        #endregion

        #endregion

        #region UI Lock

        [UIValue("interactable")]
        [UsedImplicitly]
        private bool Interactable {
            get => !PluginConfig.UILock;
            set {
                PluginConfig.UILock = !value;
                if (!value) AdjustmentModeChoice = AdjustmentModeUtils.TypeToName(AdjustmentMode.None);
                NotifyPropertyChanged();
            }
        }

        [UIValue("lock-value")] [UsedImplicitly]
        private bool _lockValue = PluginConfig.UILock;
        
        [UIAction("lock-on-change")]
        [UsedImplicitly]
        private void LockOnChange(bool value) {
            Interactable = !value;
        }

        #endregion

        #region ResetButton

        [UIAction("reset-click")]
        [UsedImplicitly]
        private void OnResetClick()
        {
            PluginConfig.ResetOffsets();
            NotifyPropertyChanged();
        }

        #endregion
    }
}