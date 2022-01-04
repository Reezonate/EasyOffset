using System;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using HMUI;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset {
    public class ModPanelUI : NotifiableSingleton<ModPanelUI> {
        #region Constructor

        public ModPanelUI() {
            SubscribeToBenchmarkEvents();
            SubscribeToRoomOffsetEvents();
        }

        #endregion

        #region Update

        private void Update() {
            UpdatePanelVisibility();
        }

        #endregion

        #region Visibility

        [UIObject("root")] [UsedImplicitly] private GameObject _root;

        private void UpdatePanelVisibility() {
            var isVisible = (_root != null) && _root.activeInHierarchy;
            PluginConfig.IsModPanelVisible = isVisible;
        }

        #endregion

        #region Flow

        private void UpdateBenchmarkPanel(bool hasResults) {
            if (hasResults) {
                BenchmarkGuideActive = false;
                BenchmarkResultsActive = true;
            } else {
                BenchmarkGuideActive = true;
                BenchmarkResultsActive = false;
            }
        }

        private void GoToMainPage() {
            MainPageActive = true;
            PresetsBrowserActive = false;

            switch (PluginConfig.AdjustmentMode) {
                case AdjustmentMode.None:
                case AdjustmentMode.Basic:
                case AdjustmentMode.Position:
                case AdjustmentMode.Rotation:
                case AdjustmentMode.RotationAuto:
                    HandsPanelActive = true;
                    BenchmarkPanelActive = false;
                    RoomOffsetPanelActive = false;
                    break;
                case AdjustmentMode.SwingBenchmark:
                    HandsPanelActive = false;
                    BenchmarkPanelActive = true;
                    RoomOffsetPanelActive = false;
                    break;
                case AdjustmentMode.RoomOffset:
                    HandsPanelActive = false;
                    BenchmarkPanelActive = false;
                    RoomOffsetPanelActive = true;
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private void GoToBrowserPage(bool allowSave, bool allowLoad) {
            MainPageActive = false;
            PresetsBrowserActive = true;
            PresetsBrowserSaveActive = allowSave;
            PresetsBrowserLoadActive = allowLoad;
        }

        #endregion

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
                                                                                   "\nBasic - Drag and drop adjustment mode" +
                                                                                   "\nPosition - Pivot position only" +
                                                                                   "\nRotation - Saber rotation only" +
                                                                                   "\nSwing Benchmark - Swing analysis" +
                                                                                   "\nRotation Auto - Automatic rotation" +
                                                                                   "\nRoom Offset - World pulling locomotion";

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
            GoToMainPage();
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

        #region Active

        private bool _handsPanelActive = true;

        [UIValue("hands-panel-active")]
        [UsedImplicitly]
        private bool HandsPanelActive {
            get => _handsPanelActive;
            set {
                _handsPanelActive = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

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
            _leftHandMenuResetAction.InvokeLater(10, ResetLeftHandMenu);
        }

        private readonly DelayedAction _leftHandMenuResetAction = new();

        private void ResetLeftHandMenu() {
            LeftActionMenuChoice = HandMenuActionUtils.TypeToName(HandMenuAction.Default);
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
            _rightHandMenuResetAction.InvokeLater(10, ResetRightHandMenu);
        }

        private readonly DelayedAction _rightHandMenuResetAction = new();

        private void ResetRightHandMenu() {
            RightActionMenuChoice = HandMenuActionUtils.TypeToName(HandMenuAction.Default);
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

        #region BenchmarkPanel

        #region Events

        private void SubscribeToBenchmarkEvents() {
            SwingBenchmarkHelper.OnResetEvent += OnBenchmarkReset;
            SwingBenchmarkHelper.OnStartEvent += OnBenchmarkStart;
            SwingBenchmarkHelper.OnUpdateEvent += OnBenchmarkUpdate;
            SwingBenchmarkHelper.OnFailEvent += OnBenchmarkFail;
            SwingBenchmarkHelper.OnSuccessEvent += OnBenchmarkSuccess;
        }

        private void OnBenchmarkReset() {
            UpdateBenchmarkPanel(false);
        }

        private void OnBenchmarkStart() {
            UpdateBenchmarkPanel(false);
        }

        private void OnBenchmarkUpdate(
            float swingCurveAngle,
            float tipDeviation,
            float pivotDeviation,
            float minimalSwingAngle,
            float maximalSwingAngle
        ) {
            BenchmarkCurveText = $"{swingCurveAngle * Mathf.Rad2Deg:F1}° {(swingCurveAngle < 0 ? "Inward" : "Outward")}";
            BenchmarkTipWobbleText = $"{(tipDeviation * 200):F1} cm";
            BenchmarkArmUsageText = $"{(pivotDeviation * 200):F1} cm";

            var min = minimalSwingAngle * Mathf.Rad2Deg;
            var max = maximalSwingAngle * Mathf.Rad2Deg;
            var full = max - min;
            BenchmarkAngleText = $"{full:F1}° ({max:F1}°/{-min:F1}°)";
        }

        private void OnBenchmarkFail() {
            UpdateBenchmarkPanel(false);
        }

        private void OnBenchmarkSuccess() {
            UpdateBenchmarkPanel(true);
        }

        #endregion

        #region Active

        private bool _benchmarkPanelActive;

        [UIValue("benchmark-panel-active")]
        [UsedImplicitly]
        private bool BenchmarkPanelActive {
            get => _benchmarkPanelActive;
            set {
                _benchmarkPanelActive = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Guide

        #region Active

        private bool _benchmarkGuideActive = true;

        [UIValue("benchmark-guide-active")]
        [UsedImplicitly]
        private bool BenchmarkGuideActive {
            get => _benchmarkGuideActive;
            set {
                _benchmarkGuideActive = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Instructions

        [UIValue("benchmark-guide")] [UsedImplicitly]
        private string _benchmarkGuide = "Repeat one exact swing several times" +
                                         "\nwhile holding the button" +
                                         "\n" +
                                         "\nAt least 140° swing angle is required";

        #endregion

        #endregion

        #region Results

        #region Active

        private bool _benchmarkResultsActive;

        [UIValue("benchmark-results-active")]
        [UsedImplicitly]
        private bool BenchmarkResultsActive {
            get => _benchmarkResultsActive;
            set {
                _benchmarkResultsActive = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Curve

        [UIValue("benchmark-curve-hint")] [UsedImplicitly]
        private string _benchmarkCurveHint = "Deviation from the straight swing" +
                                             "\n" +
                                             "\nDepends only on your config";

        private string _benchmarkCurveText = "0,0° Inward";

        [UIValue("benchmark-curve-text")]
        [UsedImplicitly]
        private string BenchmarkCurveText {
            get => _benchmarkCurveText;
            set {
                _benchmarkCurveText = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region TipWobble

        [UIValue("benchmark-tip-wobble-hint")] [UsedImplicitly]
        private string _benchmarkTipWobbleHint = "Aim deviation" +
                                                 "\n" +
                                                 "\nDepends on your grip and skill";

        private string _benchmarkTipWobbleText = "0,0 cm";

        [UIValue("benchmark-tip-wobble-text")]
        [UsedImplicitly]
        private string BenchmarkTipWobbleText {
            get => _benchmarkTipWobbleText;
            set {
                _benchmarkTipWobbleText = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region ArmUsage

        [UIValue("benchmark-arm-usage-hint")] [UsedImplicitly]
        private string _benchmarkArmUsageHint = "Arm movement amount" +
                                                "\n" +
                                                "\nPersonal preference";

        private string _benchmarkArmUsageText = "0,0 cm";

        [UIValue("benchmark-arm-usage-text")]
        [UsedImplicitly]
        private string BenchmarkArmUsageText {
            get => _benchmarkArmUsageText;
            set {
                _benchmarkArmUsageText = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Angle

        [UIValue("benchmark-angle-hint")] [UsedImplicitly]
        private string _benchmarkAmplitudeHint = "Full swing angle (Backhand/Forehand)";

        private string _benchmarkAngleText = "150° (70°/80°)";

        [UIValue("benchmark-angle-text")]
        [UsedImplicitly]
        private string BenchmarkAngleText {
            get => _benchmarkAngleText;
            set {
                _benchmarkAngleText = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #endregion

        #region AutoFixButton

        [UIAction("benchmark-auto-fix-on-click")]
        [UsedImplicitly]
        private void BenchmarkAutoFixOnClick() {
            SwingBenchmarkHelper.InvokeAutoFix();
            SetBenchmarkStatusText("Done!");
        }

        #endregion

        #region ResetButton

        [UIAction("benchmark-reset-on-click")]
        [UsedImplicitly]
        private void BenchmarkResetOnClick() {
            SwingBenchmarkHelper.InvokeReset();
        }

        #endregion

        #region StatusText

        private string _benchmarkStatusText = "";

        [UIValue("benchmark-status-text")]
        [UsedImplicitly]
        private string BenchmarkStatusText {
            get => _benchmarkStatusText;
            set {
                _benchmarkStatusText = value;
                NotifyPropertyChanged();
            }
        }

        private readonly DelayedAction _benchmarkStatusTextResetAction = new();

        private void SetBenchmarkStatusText(string value) {
            BenchmarkStatusText = value;
            _benchmarkStatusTextResetAction.InvokeLater(2000, ResetBenchmarkStatusText);
        }

        private void ResetBenchmarkStatusText() {
            BenchmarkStatusText = "";
        }

        #endregion

        #endregion

        #region RoomOffsetPanel

        #region Events

        private Vector3SO _roomCenterSO;

        private void SubscribeToRoomOffsetEvents() {
            PluginConfig.MainSettingsModelChangedEvent += OnMainSettingsModelChanged;
        }

        private void OnMainSettingsModelChanged(MainSettingsModelSO mainSettingsModel) {
            if (_roomCenterSO != null) {
                _roomCenterSO.didChangeEvent -= OnRoomCenterChanged;
            }

            _roomCenterSO = mainSettingsModel.roomCenter;
            _roomCenterSO.didChangeEvent += OnRoomCenterChanged;
            OnRoomCenterChanged();
        }

        private void OnRoomCenterChanged() {
            var tmp = _roomCenterSO.value;
            RoomXText = $"X:\t{(tmp.x * 100):F2} cm";
            RoomYText = $"Y:\t{(tmp.y * 100):F2} cm";
            RoomZText = $"Z:\t{(tmp.z * 100):F2} cm";
        }

        #endregion

        #region Active

        private bool _roomOffsetPanelActive;

        [UIValue("room-offset-panel-active")]
        [UsedImplicitly]
        private bool RoomOffsetPanelActive {
            get => _roomOffsetPanelActive;
            set {
                _roomOffsetPanelActive = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region X

        private string _roomXText = "";

        [UIValue("room-x-text")]
        [UsedImplicitly]
        private string RoomXText {
            get => _roomXText;
            set {
                _roomXText = value;
                NotifyPropertyChanged();
            }
        }

        [UIValue("room-x-toggle-value")] [UsedImplicitly]
        private bool _roomXToggleValue = PluginConfig.AllowRoomXChange;

        [UIAction("room-x-toggle-on-change")]
        [UsedImplicitly]
        private void RoomXToggleOnChange(bool value) {
            PluginConfig.AllowRoomXChange = value;
        }

        [UIAction("room-x-reset-on-click")]
        [UsedImplicitly]
        private void RoomXResetOnClick() {
            var tmp = PluginConfig.MainSettingsModel.roomCenter.value;
            PluginConfig.MainSettingsModel.roomCenter.value = new Vector3(0, tmp.y, tmp.z);
        }

        #endregion

        #region Y

        private string _roomYText = "";

        [UIValue("room-y-text")]
        [UsedImplicitly]
        private string RoomYText {
            get => _roomYText;
            set {
                _roomYText = value;
                NotifyPropertyChanged();
            }
        }

        [UIValue("room-y-toggle-value")] [UsedImplicitly]
        private bool _roomYToggleValue = PluginConfig.AllowRoomYChange;

        [UIAction("room-y-toggle-on-change")]
        [UsedImplicitly]
        private void RoomYToggleOnChange(bool value) {
            PluginConfig.AllowRoomYChange = value;
        }

        [UIAction("room-y-reset-on-click")]
        [UsedImplicitly]
        private void RoomYResetOnClick() {
            var tmp = PluginConfig.MainSettingsModel.roomCenter.value;
            PluginConfig.MainSettingsModel.roomCenter.value = new Vector3(tmp.x, 0, tmp.z);
        }

        #endregion

        #region Z

        private string _roomZText = "";

        [UIValue("room-z-text")]
        [UsedImplicitly]
        private string RoomZText {
            get => _roomZText;
            set {
                _roomZText = value;
                NotifyPropertyChanged();
            }
        }

        [UIValue("room-z-toggle-value")] [UsedImplicitly]
        private bool _roomZToggleValue = PluginConfig.AllowRoomZChange;

        [UIAction("room-z-toggle-on-change")]
        [UsedImplicitly]
        private void RoomZToggleOnChange(bool value) {
            PluginConfig.AllowRoomZChange = value;
        }

        [UIAction("room-z-reset-on-click")]
        [UsedImplicitly]
        private void RoomZResetOnClick() {
            var tmp = PluginConfig.MainSettingsModel.roomCenter.value;
            PluginConfig.MainSettingsModel.roomCenter.value = new Vector3(tmp.x, tmp.y, 0);
        }

        #endregion

        #endregion

        #region BottomPanel

        #region Save button

        [UIAction("bp-save-on-click")]
        [UsedImplicitly]
        private void BottomPanelSaveOnClick() {
            UpdatePresetsBrowserList();
            GoToBrowserPage(true, false);
        }

        #endregion

        #region Load button

        [UIAction("bp-load-on-click")]
        [UsedImplicitly]
        private void BottomPanelLoadOnClick() {
            UpdatePresetsBrowserList();
            GoToBrowserPage(false, true);
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
                    GoToMainPage();
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
            GoToMainPage();
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
            GoToMainPage();
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
            UpdateZOffsetSliders();
            GoToMainPage();
        }

        #endregion

        #endregion
    }
}