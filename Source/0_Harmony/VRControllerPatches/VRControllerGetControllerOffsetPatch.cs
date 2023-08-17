using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

[HarmonyPatch(typeof(VRController), nameof(VRController.GetControllerOffset))]
internal static class VRControllerGetControllerOffsetPatch {
    [UsedImplicitly]
    private static bool Prefix(ref Pose __result) {
        if (PluginConfig.IsDeviceless && !PluginConfig.EnabledForDeviceless) return true;
        __result = Pose.identity;
        return false;
    }
}