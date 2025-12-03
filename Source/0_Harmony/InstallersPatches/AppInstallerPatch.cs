using BGLib.AppFlow;
using EasyOffset.Installers;
using HarmonyLib;
using JetBrains.Annotations;

namespace EasyOffset {
    [HarmonyPatch(typeof(BeatSaberInit), "InstallBindings")]
    public static class AppInstallerPatch {
        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private static void Postfix(BeatSaberInit __instance) {
            OnAppInstaller.Install(__instance.Container);
        }
    }
}