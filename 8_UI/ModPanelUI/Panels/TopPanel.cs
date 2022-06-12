using System;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components.Settings;
using JetBrains.Annotations;

namespace EasyOffset;

internal class TopPanel : ReeUIComponentV2 {
    #region Initialize

    public TopPanel() {
        UpdateButtonOptions(ControllerType.None);
    }

    protected override void OnInitialize() {
        SubscribeToAssignedButtonEvents();
        PluginConfig.UILockChangedEvent += OnUILockChanged;
        PluginConfig.AdjustmentModeChangedEvent += OnAdjustmentModeChanged;
        AdjustmentModeChoice = AdjustmentModeUtils.TypeToName(PluginConfig.AdjustmentMode);
        OnAdjustmentModeChanged(PluginConfig.AdjustmentMode);
        OnUILockChanged(PluginConfig.UILock);
    }

    #endregion

    #region Events

    private void OnAdjustmentModeChanged(AdjustmentMode adjustmentMode) {
        UseFreeHandActive = adjustmentMode switch {
            AdjustmentMode.None => false,
            AdjustmentMode.Basic => true,
            AdjustmentMode.Position => true,
            AdjustmentMode.Rotation => true,
            AdjustmentMode.SwingBenchmark => true,
            AdjustmentMode.Direct => false,
            AdjustmentMode.RotationAuto => true,
            AdjustmentMode.RoomOffset => true,
            _ => throw new ArgumentOutOfRangeException(nameof(adjustmentMode), adjustmentMode, null)
        };
    }

    private void OnUILockChanged(bool locked) {
        Interactable = !locked;
    }

    #endregion

    #region Interactable

    private bool _interactable;

    [UIValue("interactable"), UsedImplicitly]
    private bool Interactable {
        get => _interactable;
        set {
            if (_interactable.Equals(value)) return;
            _interactable = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region AdjustmentMode

    [UIValue("am-hint")] [UsedImplicitly]
    private string _adjustmentModeHint = "Basic - Drag and drop adjustment mode" +
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
        ModPanelUI.OpenMainPage();
    }

    #endregion

    #region AssignedButton

    #region Events

    private void SubscribeToAssignedButtonEvents() {
        PluginConfig.ControllerTypeChangedEvent += OnControllerTypeChanged;
        PluginConfig.IsModPanelVisibleChangedEvent += IsPanelVisibleChanged;
        OnControllerTypeChanged(PluginConfig.SelectedControllerType);
    }

    private void IsPanelVisibleChanged(bool value) {
        OnControllerTypeChanged(PluginConfig.SelectedControllerType);
    }

    private void OnControllerTypeChanged(ControllerType controllerType) {
        UpdateButtonOptions(controllerType);
        _assignedButtonComponent.values = _assignedButtonChoices;
        _assignedButtonComponent.Value = PluginConfig.AssignedButton;
        _assignedButtonComponent.UpdateChoices();
    }

    private void UpdateButtonOptions(ControllerType controllerType) {
        _buttonAliasDictionary = ControllerButtonUtils.GetAvailableOptions(controllerType);
        _assignedButtonChoices = _buttonAliasDictionary.Keys.Cast<object>().ToList();
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
}