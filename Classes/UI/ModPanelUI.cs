using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using EasyOffset.Configuration;
using HMUI;
using JetBrains.Annotations;

namespace EasyOffset.UI {
    public class ModPanelUI : NotifiableSingleton<ModPanelUI> {
        #region Main page

        #region Active

        private bool _mainPageActive = true;

        [UIValue("main-page-active")]
        [UsedImplicitly]
        private bool MainPageActive {
            get => _mainPageActive;
            set {
                _mainPageActive = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Options panel

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

        #region UseFreeHand

        [UIValue("ufh-value")] [UsedImplicitly]
        private bool _useFreeHandValue = PluginConfig.UseFreeHand;

        [UIAction("ufh-on-change")]
        [UsedImplicitly]
        private void UseFreeHandOnChange(bool value) {
            PluginConfig.UseFreeHand = value;
        }

        #endregion

        #endregion

        #region Hands panel

        #region ZOffset sliders values

        [UIValue("zo-min")] [UsedImplicitly] private float _zOffsetSliderMin = -15f;

        [UIValue("zo-max")] [UsedImplicitly] private float _zOffsetSliderMax = 25f;

        [UIValue("zo-increment")] [UsedImplicitly]
        private float _zOffsetSliderIncrement = 1f;

        #endregion

        #region LeftHand Z Offset Slider

        [UIValue("lzo-value")]
        [UsedImplicitly]
        private float LeftZOffsetSliderValue {
            get => PluginConfig.LeftHandZOffset * 100f;
            set {
                PluginConfig.LeftHandZOffset = value / 100f;
                NotifyPropertyChanged();
            }
        }

        [UIAction("lzo-on-change")]
        [UsedImplicitly]
        private void OnLeftZOffsetValueChange(float value) {
            PluginConfig.LeftHandZOffset = value / 100f;
        }

        #endregion

        #region LeftHand actions menu

        [UIValue("lam-choices")] [UsedImplicitly]
        private List<object> _leftActionMenuChoices = HandMenuActionUtils.LeftHandMenuChoicesObjects.ToList();

        private string _leftActionMenuChoiceBackingField = HandMenuActionUtils.TypeToName(HandMenuAction.Default);

        [UIValue("lam-choice")]
        [UsedImplicitly]
        private string LeftActionMenuChoice {
            get => _leftActionMenuChoiceBackingField;
            set {
                _leftActionMenuChoiceBackingField = value;
                NotifyPropertyChanged();
            }
        }

        [UIAction("lam-on-change")]
        [UsedImplicitly]
        private void LeftActionMenuOnChange(string value) {
            OnHandAction(Hand.Left, HandMenuActionUtils.NameToType(value));
            ResetLeftMenuAction();
        }

        //TODO: Works but stinks, find a better solution
        private void ResetLeftMenuAction() {
            new Thread(() => {
                Thread.Sleep(10);
                LeftActionMenuChoice = HandMenuActionUtils.TypeToName(HandMenuAction.Default);
            }).Start();
        }

        #endregion

        #region RightHand Z Offset Slider

        [UIValue("rzo-value")]
        [UsedImplicitly]
        private float RightZOffsetSliderValue {
            get => PluginConfig.RightHandZOffset * 100f;
            set {
                PluginConfig.RightHandZOffset = value / 100f;
                NotifyPropertyChanged();
            }
        }

        [UIAction("rzo-on-change")]
        [UsedImplicitly]
        private void OnRightZOffsetValueChange(float value) {
            PluginConfig.RightHandZOffset = value / 100f;
        }

        #endregion

        #region RightHand actions menu

        [UIValue("ram-choices")] [UsedImplicitly]
        private List<object> _rightActionMenuChoices = HandMenuActionUtils.RightHandMenuChoicesObjects.ToList();

        private string _rightActionMenuChoiceBackingField = HandMenuActionUtils.TypeToName(HandMenuAction.Default);

        [UIValue("ram-choice")]
        [UsedImplicitly]
        private string RightActionMenuChoice {
            get => _rightActionMenuChoiceBackingField;
            set {
                _rightActionMenuChoiceBackingField = value;
                NotifyPropertyChanged();
            }
        }

        [UIAction("ram-on-change")]
        [UsedImplicitly]
        private void RightActionMenuOnChange(string value) {
            OnHandAction(Hand.Right, HandMenuActionUtils.NameToType(value));
            ResetRightActionMenu();
        }

        //TODO: Works but stinks, find a better solution
        private void ResetRightActionMenu() {
            new Thread(() => {
                Thread.Sleep(10);
                RightActionMenuChoice = HandMenuActionUtils.TypeToName(HandMenuAction.Default);
            }).Start();
        }

        #endregion

        #region OnHandMenuAction

        private void OnHandAction(Hand hand, HandMenuAction action) {
            switch (action) {
                case HandMenuAction.Default: return;

                case HandMenuAction.LeftMirrorAll:
                    PluginConfig.MirrorPivot(hand);
                    PluginConfig.MirrorSaberDirection(hand);
                    PluginConfig.MirrorZOffset(hand);
                    break;
                case HandMenuAction.RightMirrorAll:
                    PluginConfig.MirrorPivot(hand);
                    PluginConfig.MirrorSaberDirection(hand);
                    PluginConfig.MirrorZOffset(hand);
                    break;
                case HandMenuAction.MirrorPivot:
                    PluginConfig.MirrorPivot(hand);
                    break;
                case HandMenuAction.MirrorDirection:
                    PluginConfig.MirrorSaberDirection(hand);
                    break;
                case HandMenuAction.MirrorZOffset:
                    PluginConfig.MirrorZOffset(hand);
                    break;
                case HandMenuAction.Reset:
                    PluginConfig.ResetOffsets(hand);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            UpdateZOffsetSliders();
        }

        private void UpdateZOffsetSliders() {
            LeftZOffsetSliderValue = PluginConfig.LeftHandZOffset * 100f;
            RightZOffsetSliderValue = PluginConfig.RightHandZOffset * 100f;
        }

        #endregion

        #endregion

        #region BottomPanel

        #region Save button

        [UIAction("bp-save-on-click")]
        [UsedImplicitly]
        private void BottomPanelSaveOnClick() {
            UpdatePresetsBrowserList();

            MainPageActive = false;
            PresetsBrowserActive = true;
            PresetsBrowserSaveActive = true;
            PresetsBrowserLoadActive = false;
        }

        #endregion

        #region Load button

        [UIAction("bp-load-on-click")]
        [UsedImplicitly]
        private void BottomPanelLoadOnClick() {
            UpdatePresetsBrowserList();

            MainPageActive = false;
            PresetsBrowserActive = true;
            PresetsBrowserSaveActive = false;
            PresetsBrowserLoadActive = true;
        }

        #endregion

        #region UI Lock

        [UIValue("interactable")]
        [UsedImplicitly]
        private bool Interactable {
            get => !PluginConfig.UILock;
            set {
                PluginConfig.UILock = !value;
                if (!value) {
                    PluginConfig.AdjustmentMode = AdjustmentMode.None;
                    AdjustmentModeChoice = AdjustmentModeUtils.TypeToName(AdjustmentMode.None);
                }

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

        #endregion

        #endregion

        #region PresetsBrowser

        #region Active

        private bool _presetsBrowserActive;

        [UIValue("pb-active")]
        [UsedImplicitly]
        private bool PresetsBrowserActive {
            get => _presetsBrowserActive;
            set {
                _presetsBrowserActive = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Preset file name

        private string _presetFileName = "NewPreset";

        [UIValue("pb-name-value")]
        [UsedImplicitly]
        private string PresetFileName {
            get => _presetFileName;
            set {
                _presetFileName = value;
                NotifyPropertyChanged();
            }
        }

        [UIAction("pb-name-on-change")]
        [UsedImplicitly]
        private void PresetFilenameOnChange(string value) {
            for (var i = 0; i < _storedConfigPresets.Count; i++) {
                if (_storedConfigPresets[i].Name != value) continue;
                _presetsBrowserList.tableView.SelectCellWithIdx(i);
                return;
            }

            _presetsBrowserList.tableView.ClearSelection();
        }

        #endregion

        #region List

        private List<StoredConfigPreset> _storedConfigPresets = new();

        [UIComponent("pb-list")] [UsedImplicitly]
        private CustomListTableData _presetsBrowserList;

        [UIAction("pb-list-select-cell")]
        [UsedImplicitly]
        private void PresetsBrowserListSelectCell(TableView tableView, int row) {
            if (row >= _storedConfigPresets.Count) return;
            var selectedConfig = _storedConfigPresets[row];
            PresetFileName = selectedConfig.Name;
        }

        [UIAction("pb-refresh-on-click")]
        [UsedImplicitly]
        private void PresetsBrowserRefreshOnClick() {
            UpdatePresetsBrowserList();
        }

        private void UpdatePresetsBrowserList() {
            _presetsBrowserList.data.Clear();
            _storedConfigPresets = ConfigPresetsStorage.GetAllStoredPresets();

            foreach (var storedConfigPreset in _storedConfigPresets) {
                _presetsBrowserList.data.Add(new CustomListTableData.CustomCellInfo(
                    PresetUtils.GetPresetCellString(storedConfigPreset)
                ));
            }

            _presetsBrowserList.tableView.ReloadData();
            _presetsBrowserList.tableView.ClearSelection();
        }

        #endregion

        #region Cancel button

        [UIAction("pb-cancel-on-click")]
        [UsedImplicitly]
        private void PresetsBrowserCancelOnClick() {
            MainPageActive = true;
            PresetsBrowserActive = false;
        }

        #endregion

        #region Save button

        [UIValue("pb-save-hint")] [UsedImplicitly]
        private string _presetsBrowserSaveHint = "Save current preset to file" +
                                                 "\n" +
                                                 "\n<color=red>This action will overwrite existing files</color>";

        private bool _presetsBrowserSaveActive;

        [UIValue("pb-save-active")]
        [UsedImplicitly]
        private bool PresetsBrowserSaveActive {
            get => _presetsBrowserSaveActive;
            set {
                _presetsBrowserSaveActive = value;
                NotifyPropertyChanged();
            }
        }

        [UIAction("pb-save-on-click")]
        [UsedImplicitly]
        private void PresetsBrowserSaveOnClick() {
            if (!ConfigPresetsStorage.SaveCurrentPreset(PresetFileName)) return;
            MainPageActive = true;
            PresetsBrowserActive = false;
        }

        #endregion

        #region Load button

        private bool _presetsBrowserLoadActive;

        [UIValue("pb-load-hint")] [UsedImplicitly]
        private string _presetsBrowserLoadHint = "Load preset from file" +
                                                 "\n" +
                                                 "\n<color=red>Any unsaved changes will be lost</color>";

        [UIValue("pb-load-active")]
        [UsedImplicitly]
        private bool PresetsBrowserLoadActive {
            get => _presetsBrowserLoadActive;
            set {
                _presetsBrowserLoadActive = value;
                NotifyPropertyChanged();
            }
        }

        [UIAction("pb-load-on-click")]
        [UsedImplicitly]
        private void PresetsBrowserLoadOnClick() {
            if (!ConfigPresetsStorage.LoadPreset(PresetFileName)) return;
            MainPageActive = true;
            PresetsBrowserActive = false;
            UpdateZOffsetSliders();
        }

        #endregion

        #endregion
    }
}