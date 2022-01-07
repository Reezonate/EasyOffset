using System.Collections.Generic;
using System.Linq;
using IPA.Loader;

namespace EasyOffset;

internal static class CompatibilityUtils {
    #region Compatibility list

    private static readonly List<CompatibilityIssue> IncompatiblePlugins = new() {
        new CompatibilityIssue(
            "SaberTailor",
            WarningLevel.NonCritical,
            "Make sure to disable grip modification"
        ),
        new CompatibilityIssue(
            "ControllerSettingsHelper",
            WarningLevel.Critical,
            "May lead to unexpected behavior"
        )
    };

    #endregion

    #region GetCompatibilityIssues

    public static void GetCompatibilityIssues(out List<CompatibilityIssue> issues, out WarningLevel mostCriticalLevel) {
        issues = new List<CompatibilityIssue>();
        mostCriticalLevel = WarningLevel.NonCritical;

        foreach (var compatibilityIssue in IncompatiblePlugins.Where(it => IsPluginPresent(it.PluginName))) {
            if (compatibilityIssue.WarningLevel > mostCriticalLevel) mostCriticalLevel = compatibilityIssue.WarningLevel;
            issues.Add(compatibilityIssue);
        }
    }

    private static bool IsPluginPresent(string name) {
        return PluginManager.GetPluginFromId(name) != null;
    }

    #endregion

    #region CompatibilityIssue

    public readonly struct CompatibilityIssue {
        public readonly string PluginName;
        public readonly WarningLevel WarningLevel;
        public readonly string WarningMessage;

        public CompatibilityIssue(string pluginName, WarningLevel warningLevel, string warningMessage) {
            PluginName = pluginName;
            WarningLevel = warningLevel;
            WarningMessage = warningMessage;
        }
    }

    #endregion
}