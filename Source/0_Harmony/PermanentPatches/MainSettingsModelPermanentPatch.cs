using System.Reflection;
using BeatSaber.GameSettings;
using HarmonyLib;
using JetBrains.Annotations;

namespace EasyOffset {
    public static class MainSettingsModelPermanentPatch {
        public static void ApplyPatch(Harmony harmony) {
            var originalMethodInfo = typeof(MainSettingsHandler).GetMethod("PerformPostLoad");
            var postfixMethodInfo = typeof(MainSettingsModelPermanentPatch).GetMethod(nameof(Postfix), BindingFlags.Static | BindingFlags.NonPublic);
            harmony.Patch(originalMethodInfo, null, new HarmonyMethod(postfixMethodInfo));
        }

        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private static void Postfix(MainSettingsHandler __instance) {
            PluginConfig.MainSettingsHandler = __instance;
        }
    }
}