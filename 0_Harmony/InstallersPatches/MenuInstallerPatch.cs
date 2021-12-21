using EasyOffset.Installers;
using HarmonyLib;
using JetBrains.Annotations;

namespace EasyOffset {
    [HarmonyPatch(typeof(MainSettingsMenuViewControllersInstaller), "InstallBindings")]
    public static class MenuInstallerPatch {
        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private static void Postfix(MainSettingsMenuViewControllersInstaller __instance) {
            var container = __instance.GetContainer();
            OnMenuInstaller.Install(container);
        }
    }
}