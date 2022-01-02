using System.Collections.Generic;
using HarmonyLib;
using JetBrains.Annotations;

namespace EasyOffset;

[HarmonyPatch(typeof(GameScenesManager), "ScenesTransitionCoroutine")]
internal static class SceneTransitionPatch {
    [UsedImplicitly]
    private static void Prefix(
        List<string> scenesToPresent,
        List<string> scenesToDismiss
    ) {
        foreach (var sceneName in scenesToPresent) {
            if (!sceneName.Equals("MainMenu")) continue;
            PluginConfig.IsInMainMenu = true;
            break;
        }

        foreach (var sceneName in scenesToDismiss) {
            if (!sceneName.Equals("MainMenu")) continue;
            PluginConfig.IsInMainMenu = false;
            break;
        }
    }
}