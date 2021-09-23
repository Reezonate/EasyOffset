using EasyOffset.Installers;
using HarmonyLib;
using JetBrains.Annotations;

namespace EasyOffset.HarmonyPatches {
    [HarmonyPatch(typeof(PCAppInit), "InstallBindings")]
    public static class AppInstallerPatch {
        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private static void Postfix(PCAppInit __instance) {
            var container = __instance.GetContainer();
            OnAppInstaller.PreInstall(__instance, container);
            OnAppInstaller.Install(container);
        }
    }
}