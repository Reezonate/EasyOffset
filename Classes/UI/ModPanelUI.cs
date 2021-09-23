using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using EasyOffset.Configuration;
using JetBrains.Annotations;

namespace EasyOffset.UI {
    public class ModPanelUI : NotifiableSingleton<ModPanelUI> {
        #region Grip Button Action

        [UIValue("gba-hint")] [UsedImplicitly] private string _gripButtonActionHint = "Usage: hold Grip button and move your hand" +
                                                                                      "\n" +
                                                                                      "\nFull grip - rough offset adjustment" +
                                                                                      "\nPivot Only - change saber origin position" +
                                                                                      "\nDirection only - change saber rotation" +
                                                                                      "\nRoom Offset - move yourself in space";

        [UIValue("gba-choices")] [UsedImplicitly]
        private List<object> _gripButtonActionOptions = GripButtonActionUtils.AllNamesObjects.ToList();

        [UIValue("gba-choice")] [UsedImplicitly]
        private string _gripButtonActionValue = GripButtonActionUtils.TypeToName(PluginConfig.GripButtonAction);

        [UIAction("gba-on-change")]
        [UsedImplicitly]
        private void OnGripButtonActionChange(string selectedValue) {
            PluginConfig.GripButtonAction = GripButtonActionUtils.NameToType(selectedValue);
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

        #region Both Hands

        [UIValue("zo-min")] [UsedImplicitly] private float _zOffsetSliderMin = -0.2f;

        [UIValue("zo-max")] [UsedImplicitly] private float _zOffsetSliderMax = 0.2f;

        [UIValue("zo-increment")] [UsedImplicitly]
        private float _zOffsetSliderIncrement = 0.005f;

        #endregion

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
    }
}