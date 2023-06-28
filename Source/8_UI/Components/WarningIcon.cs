using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;

namespace EasyOffset;

internal class WarningIcon : ReeUIComponentV2 {
    #region Initialize & Dispose

    protected override void OnInitialize() {
        PluginConfig.IsModPanelVisibleChangedEvent += OnIsModPanelVisibleChanged;
        PluginConfig.MinimalWarningLevelChangedEvent += OnMinimalWaringLevelChanged;
        OnMinimalWaringLevelChanged(PluginConfig.MinimalWarningLevel);
    }

    protected override void OnDispose() {
        PluginConfig.IsModPanelVisibleChangedEvent -= OnIsModPanelVisibleChanged;
        PluginConfig.MinimalWarningLevelChangedEvent -= OnMinimalWaringLevelChanged;
    }

    #endregion

    #region Events

    private void OnIsModPanelVisibleChanged(bool value) {
        if (!value) return;
        OnMinimalWaringLevelChanged(PluginConfig.MinimalWarningLevel);
    }

    private void OnMinimalWaringLevelChanged(WarningLevel minimalWarningLevel) {
        CompatibilityUtils.GetCompatibilityIssues(out var issues, out var mostCriticalLevel);

        // if (issues.Count == 0 || mostCriticalLevel < minimalWarningLevel) {
        //     WarningActive = false;
        //     WarningHint = "";
        //     return;
        // }
        //
        // switch (mostCriticalLevel) {
        //     case WarningLevel.NonCritical:
        //         WarningColor = NonCriticalImageColor;
        //         break;
        //     case WarningLevel.Critical:
        //         WarningColor = CriticalImageColor;
        //         break;
        //     case WarningLevel.Disable:
        //     default: return;
        // }

        WarningColor = CriticalImageColor;
        WarningHint = BuildWarningMessage(issues, minimalWarningLevel);
        WarningActive = true;
    }

    #endregion

    #region BuildWarningMessage

    private static string BuildWarningMessage(IEnumerable<CompatibilityUtils.CompatibilityIssue> issues, WarningLevel minimalWarningLevel) {
        var stringBuilder = new StringBuilder();
        
        stringBuilder.AppendLine($"<color={CriticalTextColor}>!IMPORTANT! OpenXR update (1.29+)</color>");
        stringBuilder.AppendLine("<size=80%>Due to breaking changes in the base game\r\nall offsets are different and migration is not possible\r\nYou have to redo your settings</size>");
        stringBuilder.AppendLine($"<color={CriticalTextColor}><size=80%>\r\nThis may be changed again in the future updates if game devs decide to revert this changes!</size></color>");

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

        stringBuilder.AppendLine();
        stringBuilder.Append("<size=80%>You can disable warnings in the mod settings</size>");
        return stringBuilder.ToString();
    }

    #endregion

    #region WarningActive

    private bool _warningActive;

    [UIValue("warning-active"), UsedImplicitly]
    private bool WarningActive {
        get => _warningActive;
        set {
            if (_warningActive.Equals(value)) return;
            _warningActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region WarningColor

    private const string NonCriticalImageColor = "#FFFF00";
    private const string CriticalImageColor = "#FF0000";
    private const string NonCriticalTextColor = "#CE8600";
    private const string CriticalTextColor = "#9E0000";

    private string _warningColor = NonCriticalImageColor;

    [UIValue("warning-color"), UsedImplicitly]
    private string WarningColor {
        get => _warningColor;
        set {
            if (_warningColor.Equals(value)) return;
            _warningColor = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region WarningHint

    private string _warningHint = "";

    [UIValue("warning-hint"), UsedImplicitly]
    private string WarningHint {
        get => _warningHint;
        set {
            if (_warningHint.Equals(value)) return;
            _warningHint = value;
            NotifyPropertyChanged();
        }
    }

    #endregion
}