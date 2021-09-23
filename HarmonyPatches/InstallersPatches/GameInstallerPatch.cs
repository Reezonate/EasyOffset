using EasyOffset.Installers;
using HarmonyLib;
using JetBrains.Annotations;

namespace EasyOffset.HarmonyPatches {
    [HarmonyPatch(typeof(GameplayCoreInstaller), "InstallBindings")]
    public static class GameInstallerPatch {
        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private static void Postfix(GameplayCoreInstaller __instance) {
            var container = __instance.GetContainer();
            OnGameInstaller.Install(container);
        }
    }
}