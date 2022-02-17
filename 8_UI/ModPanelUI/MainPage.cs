using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components.Settings;
using JetBrains.Annotations;

namespace EasyOffset;

internal partial class ModPanelUI {
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

    [UIValue("am-hint")] [UsedImplicitly] private string _adjustmentModeHint = "Basic - Drag and drop adjustment mode" +
                                                                               "\nPosition - Pivot position only" +
                                                                               "\nRotation - Saber rotation only" +
                                                                               "\nPrecise - Direct config values change" +
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

    #region Events

    private void SubscribeToAssignedButtonEvents() {
        PluginConfig.ControllerTypeChangedEvent += OnControllerTypeChanged;
        OnControllerTypeChanged(PluginConfig.SelectedControllerType, false);
    }

    private void OnControllerTypeChanged(ControllerType controllerType) {
        OnControllerTypeChanged(controllerType, true);
    }

    private void OnControllerTypeChanged(ControllerType controllerType, bool forceChoices) {
        _buttonAliasDictionary = ControllerButtonUtils.GetAvailableOptions(controllerType, ConfigMigration.IsVRModeOculus);
        _assignedButtonChoices = _buttonAliasDictionary.Keys.Cast<object>().ToList();

        if (!forceChoices) return;
        _assignedButtonComponent.values = _assignedButtonChoices;
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

    #region Display Controller

    [UIValue("dc-choices")] [UsedImplicitly]
    private List<object> _displayControllerOptions = ControllerTypeUtils.AllNamesObjects.ToList();

    [UIValue("dc-choice")] [UsedImplicitly]
    private string _displayControllerValue = ControllerTypeUtils.TypeToName(PluginConfig.SelectedControllerType);

    [UIAction("dc-on-change")]
    [UsedImplicitly]
    private void OnDisplayControllerChange(string selectedValue) {
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

    #region Events

    private void SubscribeToUndoRedoEvents() {
        PluginConfig.UndoAvailableChangedEvent += OnUndoAvailableChanged;
        PluginConfig.RedoAvailableChangedEvent += OnRedoAvailableChanged;
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

    #region Active

    private bool _undoRedoButtonsActive;

    [UIValue("undo-redo-buttons-active")]
    [UsedImplicitly]
    private bool UndoRedoButtonsActive {
        get => _undoRedoButtonsActive;
        set {
            _undoRedoButtonsActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Undo button

    private bool _undoButtonInteractable;

    [UIValue("undo-button-interactable")]
    [UsedImplicitly]
    private bool UndoButtonInteractable {
        get => _undoButtonInteractable;
        set {
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
}