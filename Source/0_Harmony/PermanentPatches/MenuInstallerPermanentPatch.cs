using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;

namespace EasyOffset {
    public static class MenuInstallerPermanentPatch {
        public static void ApplyPatch(Harmony harmony) {
            var originalMethodInfo = typeof(MainSettingsMenuViewControllersInstaller).GetMethod("InstallBindings");
            var postfixMethodInfo = typeof(MenuInstallerPermanentPatch).GetMethod(nameof(Postfix), BindingFlags.Static | BindingFlags.NonPublic);
            harmony.Patch(originalMethodInfo, null, new HarmonyMethod(postfixMethodInfo));
        }

        [UsedImplicitly]
        private static void Postfix() {
            Plugin.InitializeUI();
        }
    }
}