using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;

namespace EasyOffset {
    public static class AppInstallerPermanentPatch {
        public static void ApplyPatch(Harmony harmony) {
            var originalMethodInfo = typeof(PCAppInit).GetMethod("InstallBindings");
            var postfixMethodInfo = typeof(AppInstallerPermanentPatch).GetMethod(nameof(Postfix), BindingFlags.Static | BindingFlags.NonPublic);
            harmony.Patch(originalMethodInfo, null, new HarmonyMethod(postfixMethodInfo));
        }

        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private static void Postfix(PCAppInit __instance) {
            PluginConfig.VRPlatformHelper = __instance.Container.TryResolve<IVRPlatformHelper>();
            PluginConfig.MainSettingsModel = __instance._mainSystemInit._mainSettingsModel;
        }
    }
}