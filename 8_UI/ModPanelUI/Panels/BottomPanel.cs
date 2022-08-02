using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal class BottomPanel : ReeUIComponentV2 {
    #region Components

    [UIValue("undo-redo-buttons"), UsedImplicitly] private UndoRedoButtons _undoRedoButtons;

    private void Awake() {
        _undoRedoButtons = Instantiate<UndoRedoButtons>(transform, false);
    }

    #endregion
    
    #region Initialize & Dispose

    protected override void OnInitialize() {
        PluginConfig.IsModPanelVisibleChangedEvent += BottomPanelOnVisibleChanged;
        ApplyBottomPanelScale();

        PluginConfig.MinimalWarningLevelChangedEvent += UpdateWarning;
        PluginConfig.IsModPanelVisibleChangedEvent += OnModPanelVisibleChanged;
        UpdateWarning(PluginConfig.MinimalWarningLevel);
    }

    protected override void OnDispose() {
        PluginConfig.IsModPanelVisibleChangedEvent -= BottomPanelOnVisibleChanged;
        PluginConfig.MinimalWarningLevelChangedEvent -= UpdateWarning;
        PluginConfig.IsModPanelVisibleChangedEvent -= OnModPanelVisibleChanged;
    }

    #endregion

    #region Scale

    [UIComponent("undo-redo-buttons-container"), UsedImplicitly]  private RectTransform _undoRedoButtonsContainer;
    [UIComponent("ui-lock-container"), UsedImplicitly]  private RectTransform _uiLockContainer;

    private static readonly Vector3 BottomElementsScale = Vector3.one * 0.85f;

    private void ApplyBottomPanelScale() {
        _undoRedoButtonsContainer.localScale = BottomElementsScale;
        _uiLockContainer.localScale = BottomElementsScale;
    }

    private void BottomPanelOnVisibleChanged(bool value) {
        if (!value) return;
        ApplyBottomPanelScale();
    }

    #endregion

    #region Save & Load buttons

    [UIAction("bp-save-on-click"), UsedImplicitly]
    private void BottomPanelSaveOnClick() {
        ModPanelUI.OpenBrowserPage(true, false);
    }

    [UIAction("bp-load-on-click"), UsedImplicitly]
    private void BottomPanelLoadOnClick() {
        ModPanelUI.OpenBrowserPage(false, true);
    }

    #endregion

    #region Warnings

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

    private void InitializeWarnings() {
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

    #region UI Lock

    private AdjustmentMode _previousAdjustmentMode;

    [UIValue("interactable"), UsedImplicitly]
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

            ModPanelUI.OpenMainPage();
            NotifyPropertyChanged();
        }
    }

    [UIValue("lock-value"), UsedImplicitly] private bool _lockValue = PluginConfig.UILock;

    [UIAction("lock-on-change"), UsedImplicitly]
    private void LockOnChange(bool value) {
        Interactable = !value;
    }

    #endregion
}