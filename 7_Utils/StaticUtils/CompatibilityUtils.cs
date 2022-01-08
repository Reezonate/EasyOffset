using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IPA.Loader;

namespace EasyOffset;

internal static class CompatibilityUtils {
    #region Compatibility list

    private static readonly List<CompatibilityIssue> IncompatiblePlugins = new() {
        new CompatibilityIssue(
            "SaberTailor",
            WarningLevel.NonCritical,
            "Make sure to disable grip modification",
            IsSaberTailorInterfering
        ),
        new CompatibilityIssue(
            "ControllerSettingsHelper",
            WarningLevel.Critical,
            "May lead to unexpected behavior",
            () => IsPluginLoaded("ControllerSettingsHelper")
        )
    };

    #endregion

    #region GetCompatibilityIssues

    public static void GetCompatibilityIssues(out List<CompatibilityIssue> issues, out WarningLevel mostCriticalLevel) {
        issues = new List<CompatibilityIssue>();
        mostCriticalLevel = WarningLevel.NonCritical;

        foreach (var compatibilityIssue in IncompatiblePlugins.Where(it => it.Condition())) {
            if (compatibilityIssue.WarningLevel > mostCriticalLevel) mostCriticalLevel = compatibilityIssue.WarningLevel;
            issues.Add(compatibilityIssue);
        }
    }

    #endregion

    #region IsPluginLoaded

    private static bool IsPluginLoaded(string name) {
        return PluginManager.GetPluginFromId(name) != null;
    }

    #endregion

    #region SaberTailorConditionCheck

    private static bool IsSaberTailorInterfering() {
        var pluginMetadata = PluginManager.GetPluginFromId("SaberTailor");
        if (pluginMetadata == null) return false;

        var type = pluginMetadata.Assembly.GetType("SaberTailor.Settings.Utilities.PluginConfig");
        if (type == null) return true;

        var instanceFieldInfo = type.GetField("Instance", BindingFlags.Static | BindingFlags.Public);
        var gripModEnabledFieldInfo = type.GetField("IsGripModEnabled", BindingFlags.Instance | BindingFlags.Public);
        if (instanceFieldInfo == null || gripModEnabledFieldInfo == null) return true;

        var instance = instanceFieldInfo.GetValue(null);
        if (instance == null) return true;

        return (bool) gripModEnabledFieldInfo.GetValue(instance);
    }

    #endregion

    #region CompatibilityIssue

    public readonly struct CompatibilityIssue {
        public readonly string PluginName;
        public readonly WarningLevel WarningLevel;
        public readonly string WarningMessage;
        public readonly Func<bool> Condition;

        public CompatibilityIssue(string pluginName, WarningLevel warningLevel, string warningMessage, Func<bool> condition) {
            PluginName = pluginName;
            WarningLevel = warningLevel;
            WarningMessage = warningMessage;
            Condition = condition;
        }
    }

    #endregion
}