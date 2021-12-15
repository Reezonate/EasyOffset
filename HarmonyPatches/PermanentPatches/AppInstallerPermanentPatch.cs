using System.Reflection;
using EasyOffset.Configuration;
using HarmonyLib;
using JetBrains.Annotations;

namespace EasyOffset.HarmonyPatches {
    public static class AppInstallerPermanentPatch {
        #region ApplyPatch

        private static MethodInfo OriginalMethodInfo => typeof(PCAppInit).GetMethod("InstallBindings");
        private static MethodInfo PostfixMethodInfo => typeof(AppInstallerPermanentPatch).GetMethod("Postfix", BindingFlags.Static | BindingFlags.NonPublic);

        public static void ApplyPatch(Harmony harmony) {
            Plugin.Log.Info($"OriginalMethodInfo: {OriginalMethodInfo}");
            Plugin.Log.Info($"PostfixMethodInfo: {PostfixMethodInfo}");
            harmony.Patch(OriginalMethodInfo, null, new HarmonyMethod(PostfixMethodInfo));
        }

        #endregion

        #region Prefix

        private static readonly FieldInfo MainSettingsFieldInfo = typeof(PCAppInit).GetField(
            "_mainSettingsModel",
            BindingFlags.Instance | BindingFlags.NonPublic
        );

        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private static void Postfix(PCAppInit __instance) {
            var container = __instance.GetContainer();
            PluginConfig.VRPlatformHelper = container.TryResolve<IVRPlatformHelper>();
            PluginConfig.MainSettingsModel = (MainSettingsModelSO) MainSettingsFieldInfo.GetValue(__instance);
        }

        #endregion
    }
}