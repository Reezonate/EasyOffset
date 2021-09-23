using System.Reflection;
using EasyOffset.Installers;
using HarmonyLib;
using JetBrains.Annotations;

namespace EasyOffset.HarmonyPatches {
    [HarmonyPatch(typeof(MainSettingsMenuViewControllersInstaller), "InstallBindings")]
    public static class MenuInstallerPatch {
        private static readonly PropertyInfo ContainerPropertyInfo = typeof(MainSettingsMenuViewControllersInstaller).GetProperty(
            "Container",
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic
        );

        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private static void Postfix(MainSettingsMenuViewControllersInstaller __instance) {
            var container = __instance.GetContainer();
            OnMenuInstaller.Install(container);
        }
    }
}