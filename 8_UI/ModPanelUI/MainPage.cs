using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components.Settings;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal partial class ModPanelUI {
    #region Active

    private bool _mainPageActive = true;

    [UIValue("main-page-active")]
    [UsedImplicitly]
    private bool MainPageActive {
        get => _mainPageActive;
        set {
            if (_mainPageActive.Equals(value)) return;
            _mainPageActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Options panel

    #region AdjustmentMode

    [UIValue("am-hint")] [UsedImplicitly] private string _adjustmentModeHint = "Basic - Drag and drop adjustment mode" +
                                                                               "\nPosition - Pivot position only" +
                                                                               "\nRotation - Saber rotation only" +
                                                                               "\nSwing Benchmark - Swing analysis" +
                                                                               "\nDirect - Raw config values change" +
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
            if (_adjustmentModeChoice.Equals(value)) return;
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

    #region Events

    private void SubscribeToAssignedButtonEvents() {
        PluginConfig.ControllerTypeChangedEvent += OnControllerTypeChanged;
        PluginConfig.IsModPanelVisibleChangedEvent += IsPanelVisibleChanged;
        OnControllerTypeChanged(PluginConfig.SelectedControllerType, false);
    }

    private void IsPanelVisibleChanged(bool value) {
        OnControllerTypeChanged(PluginConfig.SelectedControllerType, true);
    }

    private void OnControllerTypeChanged(ControllerType controllerType) {
        OnControllerTypeChanged(controllerType, true);
    }

    private void OnControllerTypeChanged(ControllerType controllerType, bool forceUpdate) {
        _buttonAliasDictionary = ControllerButtonUtils.GetAvailableOptions(controllerType);
        _assignedButtonChoices = _buttonAliasDictionary.Keys.Cast<object>().ToList();

        if (!forceUpdate) return;
        _assignedButtonComponent.values = _assignedButtonChoices;
        _assignedButtonComponent.Value = PluginConfig.AssignedButton;
        _assignedButtonComponent.UpdateChoices();
    }

    #endregion

    #region Formatter

    private Dictionary<ControllerButton, string> _buttonAliasDictionary;

    [UIAction("ab-formatter")]
    [UsedImplicitly]
    private string AssignedButtonFormatter(ControllerButton controllerButton) {
        return _buttonAliasDictionary.ContainsKey(controllerButton) ? _buttonAliasDictionary[controllerButton] : "None";
    }

    #endregion

    [UIComponent("ab-component")] [UsedImplicitly]
    private DropDownListSetting _assignedButtonComponent;

    [UIValue("ab-choices")] [UsedImplicitly]
    private List<object> _assignedButtonChoices;

    [UIValue("ab-choice")]
    [UsedImplicitly]
    private ControllerButton AssignedButtonChoice {
        get => PluginConfig.AssignedButton;
        set => PluginConfig.AssignedButton = value;
    }

    #endregion

    #region Controller Type

    [UIValue("controller-type-choices")] [UsedImplicitly]
    private List<object> _controllerTypeOptions = ControllerTypeUtils.AllNamesObjects.ToList();

    [UIValue("controller-type-choice")] [UsedImplicitly]
    private string _controllerTypeValue = ControllerTypeUtils.TypeToName(PluginConfig.SelectedControllerType);

    [UIAction("controller-type-on-change")]
    [UsedImplicitly]
    private void ControllerTypeOnChange(string selectedValue) {
        PluginConfig.SelectedControllerType = ControllerTypeUtils.NameToType(selectedValue);
    }

    #endregion

    #region UseFreeHand

    private bool _useFreeHandActive = true;

    [UIValue("ufh-active")]
    [UsedImplicitly]
    private bool UseFreeHandActive {
        get => _useFreeHandActive;
        set {
            if (_useFreeHandActive.Equals(value)) return;
            _useFreeHandActive = value;
            NotifyPropertyChanged();
        }
    }

    [UIValue("ufh-value")] [UsedImplicitly]
    private bool _useFreeHandValue = PluginConfig.UseFreeHand;

    [UIAction("ufh-on-change")]
    [UsedImplicitly]
    private void UseFreeHandOnChange(bool value) {
        PluginConfig.UseFreeHand = value;
    }

    #endregion

    #endregion

    #region BottomPanel

    #region Events

    private void SubscribeToBottomPanelEvents() {
        PluginConfig.UndoAvailableChangedEvent += OnUndoAvailableChanged;
        PluginConfig.RedoAvailableChangedEvent += OnRedoAvailableChanged;
        PluginConfig.IsModPanelVisibleChangedEvent += BottomPanelOnVisibleChanged;
    }

    private void BottomPanelOnVisibleChanged(bool value) {
        if (!value) return;
        ApplyBottomPanelScale();
    }

    private void OnUndoAvailableChanged(bool isAvailable, string description) {
        UndoButtonInteractable = isAvailable;
        UndoButtonHoverHint = isAvailable ? $"Undo '{description}'" : "Undo";
    }

    private void OnRedoAvailableChanged(bool isAvailable, string description) {
        RedoButtonInteractable = isAvailable;
        RedoButtonHoverHint = isAvailable ? $"Redo '{description}'" : "Redo";
    }

    #endregion

    #region Scale

    private static readonly Vector3 BottomElementsScale = Vector3.one * 0.85f;

    private void ApplyBottomPanelScale() {
        _undoRedoButtonsContainer.localScale = BottomElementsScale;
        _uiLockContainer.localScale = BottomElementsScale;
    }

    #endregion

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

    #region WarningIcon

    private const string NonCriticalImageColor = "#FFFF00";
    private const string CriticalImageColor = "#FF0000";
    private const string NonCriticalTextColor = "#CE8600";
    private const string CriticalTextColor = "#9E0000";

    private bool _warningActive;

    [UIValue("warning-active")]
    [UsedImplicitly]
    private bool WarningActive {
        get => _warningActive;
        set {
            if (_warningActive.Equals(value)) return;
            _warningActive = value;
            NotifyPropertyChanged();
        }
    }

    private string _warningColor = NonCriticalImageColor;

    [UIValue("warning-color")]
    [UsedImplicitly]
    private string WarningColor {
        get => _warningColor;
        set {
            if (_warningColor.Equals(value)) return;
            _warningColor = value;
            NotifyPropertyChanged();
        }
    }

    private string _warningHint = "";

    [UIValue("warning-hint")]
    [UsedImplicitly]
    private string WarningHint {
        get => _warningHint;
        set {
            if (_warningHint.Equals(value)) return;
            _warningHint = value;
            NotifyPropertyChanged();
        }
    }

    private void SubscribeToWarningEvents() {
        PluginConfig.MinimalWarningLevelChangedEvent += UpdateWarning;
        PluginConfig.IsModPanelVisibleChangedEvent += OnModPanelVisibleChanged;
        UpdateWarning(PluginConfig.MinimalWarningLevel);
    }

    private void OnModPanelVisibleChanged(bool value) {
        if (!value) return;
        UpdateWarning(PluginConfig.MinimalWarningLevel);
    }

    private void UpdateWarning(WarningLevel minimalWarningLevel) {
        CompatibilityUtils.GetCompatibilityIssues(out var issues, out var mostCriticalLevel);

        if (issues.Count == 0 || mostCriticalLevel < minimalWarningLevel) {
            WarningActive = false;
            WarningHint = "";
            return;
        }

        switch (mostCriticalLevel) {
            case WarningLevel.NonCritical:
                WarningColor = NonCriticalImageColor;
                break;
            case WarningLevel.Critical:
                WarningColor = CriticalImageColor;
                break;
            case WarningLevel.Disable:
            default: return;
        }

        WarningHint = BuildWarningMessage(issues, minimalWarningLevel);
        WarningActive = true;
    }

    private static string BuildWarningMessage(IEnumerable<CompatibilityUtils.CompatibilityIssue> issues, WarningLevel minimalWarningLevel) {
        var stringBuilder = new StringBuilder();

        foreach (var issue in issues.Where(issue => issue.WarningLevel >= minimalWarningLevel)) {
            switch (issue.WarningLevel) {
                case WarningLevel.NonCritical:
                    stringBuilder.Append("<color=");
                    stringBuilder.Append(NonCriticalTextColor);
                    stringBuilder.Append(">Interference</color> - ");
                    break;
                case WarningLevel.Critical:
                    stringBuilder.Append("<color=");
                    stringBuilder.Append(CriticalTextColor);
                    stringBuilder.Append(">Incompatible</color> - ");
                    break;
                case WarningLevel.Disable:
                default: continue;
            }

            stringBuilder.AppendLine(issue.PluginName);
            stringBuilder.Append("<size=80%>");
            stringBuilder.Append(issue.WarningMessage);
            stringBuilder.AppendLine("</size>");
            stringBuilder.AppendLine();
        }

        stringBuilder.Append("<size=80%>You can disable warnings in the mod settings</size>");
        return stringBuilder.ToString();
    }

    #endregion

    #region Undo / Redo buttons

    #region Active

    private bool _undoRedoButtonsActive;

    [UIValue("undo-redo-buttons-active")]
    [UsedImplicitly]
    private bool UndoRedoButtonsActive {
        get => _undoRedoButtonsActive;
        set {
            if (_undoRedoButtonsActive.Equals(value)) return;
            _undoRedoButtonsActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Container

    [UIComponent("undo-redo-buttons-container")] [UsedImplicitly]
    private RectTransform _undoRedoButtonsContainer;

    #endregion

    #region Undo button

    private bool _undoButtonInteractable;

    [UIValue("undo-button-interactable")]
    [UsedImplicitly]
    private bool UndoButtonInteractable {
        get => _undoButtonInteractable;
        set {
            if (_undoButtonInteractable.Equals(value)) return;
            _undoButtonInteractable = value;
            NotifyPropertyChanged();
        }
    }

    private string _undoButtonHoverHint = "Undo";

    [UIValue("undo-button-hover-hint")]
    [UsedImplicitly]
    private string UndoButtonHoverHint {
        get => _undoButtonHoverHint;
        set {
            _undoButtonHoverHint = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("undo-button-on-click")]
    [UsedImplicitly]
    private void UndoButtonOnClick() {
        PluginConfig.UndoLastChange();
    }

    #endregion

    #region Redo button

    private bool _redoButtonInteractable;

    [UIValue("redo-button-interactable")]
    [UsedImplicitly]
    private bool RedoButtonInteractable {
        get => _redoButtonInteractable;
        set {
            if (_redoButtonInteractable.Equals(value)) return;
            _redoButtonInteractable = value;
            NotifyPropertyChanged();
        }
    }

    private string _redoButtonHoverHint = "Redo";

    [UIValue("redo-button-hover-hint")]
    [UsedImplicitly]
    private string RedoButtonHoverHint {
        get => _redoButtonHoverHint;
        set {
            _redoButtonHoverHint = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("redo-button-on-click")]
    [UsedImplicitly]
    private void RedoButtonOnClick() {
        PluginConfig.RedoLastChange();
    }

    #endregion

    #endregion

    #region UI Lock

    private AdjustmentMode _previousAdjustmentMode;

    [UIComponent("ui-lock-container")] [UsedImplicitly]
    private RectTransform _uiLockContainer;

    [UIValue("interactable")]
    [UsedImplicitly]
    private bool Interactable {
        get => !PluginConfig.UILock;
        set {
            PluginConfig.UILock = !value;

            if (!value) {
                _previousAdjustmentMode = PluginConfig.AdjustmentMode;
                PluginConfig.AdjustmentMode = AdjustmentMode.None;
            } else {
                PluginConfig.AdjustmentMode = _previousAdjustmentMode;
            }

            AdjustmentModeChoice = AdjustmentModeUtils.TypeToName(PluginConfig.AdjustmentMode);
            GoToMainPage();

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
}