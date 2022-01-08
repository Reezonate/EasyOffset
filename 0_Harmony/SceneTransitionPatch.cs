using System;
using System.Collections.Generic;
using HarmonyLib;
using JetBrains.Annotations;

namespace EasyOffset;

[HarmonyPatch(typeof(GameScenesManager), "ScenesTransitionCoroutine")]
internal static class SceneTransitionPatch {
    [UsedImplicitly]
    private static void Prefix(
        List<string> scenesToPresent,
        List<string> scenesToDismiss,
        ref Action afterMinDurationCallback
    ) {
        foreach (var sceneName in scenesToPresent) {
            if (!sceneName.Equals("MainMenu")) continue;
            afterMinDurationCallback += OnEnterMainMenu;
            break;
        }

        foreach (var sceneName in scenesToDismiss) {
            if (!sceneName.Equals("MainMenu")) continue;
            afterMinDurationCallback += OnLeaveMainMenu;
            break;
        }
    }

    private static void OnEnterMainMenu() {
        PluginConfig.IsInMainMenu = true;
    }

    private static void OnLeaveMainMenu() {
        PluginConfig.IsInMainMenu = false;
    }
}