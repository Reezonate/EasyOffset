using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;

namespace EasyOffset {
    public static class MenuInstallerPermanentPatch {
        #region ApplyPatch

        private static MethodInfo OriginalMethodInfo => typeof(MainSettingsMenuViewControllersInstaller).GetMethod("InstallBindings");
        private static MethodInfo PostfixMethodInfo => typeof(MenuInstallerPermanentPatch).GetMethod("Postfix", BindingFlags.Static | BindingFlags.NonPublic);

        public static void ApplyPatch(Harmony harmony) {
            harmony.Patch(OriginalMethodInfo, null, new HarmonyMethod(PostfixMethodInfo));
        }

        #endregion

        #region Postfix

        [UsedImplicitly]
        private static void Postfix() {
            Plugin.InitializeUI();
        }

        #endregion
    }
}