using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

[HarmonyPatch(typeof(VRController), nameof(VRController.TryGetControllerOffset))]
internal static class VRControllerGetControllerOffsetPatch {
    [UsedImplicitly]
    private static bool Prefix(ref Pose poseOffset) {
        if (PluginConfig.IsDeviceless && !PluginConfig.EnabledForDeviceless) return true;
        poseOffset = Pose.identity;
        return false;
    }
}