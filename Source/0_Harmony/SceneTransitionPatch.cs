using System;
using System.Collections.Generic;
using HarmonyLib;
using JetBrains.Annotations;
using Zenject;
using static GameScenesManager;

namespace EasyOffset;

[HarmonyPatch(typeof(GameScenesManager), "ScenesTransitionCoroutine")]
internal static class SceneTransitionPatch {
    [UsedImplicitly]
    private static void Prefix(
        ScenesTransitionSetupDataSO newScenesTransitionSetupData,
        IReadOnlyList<string> scenesToPresent,
        ScenePresentType presentType,
        IReadOnlyList<string> scenesToDismiss,
        SceneDismissType dismissType,
        float minDuration,
        bool canTriggerGarbageCollector,
        ref Action afterMinDurationCallback,
        Action<DiContainer> extraBindingsCallback,
        Action<DiContainer> finishCallback
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