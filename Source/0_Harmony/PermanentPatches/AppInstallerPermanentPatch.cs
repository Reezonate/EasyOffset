using BGLib.AppFlow;
using HarmonyLib;
using JetBrains.Annotations;
using System.Reflection;

namespace EasyOffset {
    public static class AppInstallerPermanentPatch {
        public static void ApplyPatch(Harmony harmony) {
            var originalMethodInfo = typeof(BeatSaberInit).GetMethod("InstallBindings");
            var postfixMethodInfo = typeof(AppInstallerPermanentPatch).GetMethod(nameof(Postfix), BindingFlags.Static | BindingFlags.NonPublic);
            harmony.Patch(originalMethodInfo, null, new HarmonyMethod(postfixMethodInfo));
        }

        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private static void Postfix(BeatSaberInit __instance) {
            PluginConfig.VRPlatformHelper = __instance.Container.TryResolve<IVRPlatformHelper>();
        }
    }
}