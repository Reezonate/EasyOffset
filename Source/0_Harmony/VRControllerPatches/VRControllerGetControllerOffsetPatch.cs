using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.XR;

namespace EasyOffset;

internal static class VRControllerGetControllerOffsetPatch {
    public static void ApplyPatch(Harmony harmony) {
        var targetMethod = typeof(VRController).GetMethod(nameof(VRController.TryGetControllerOffset), BindingFlags.Public | BindingFlags.Static);
        var prefix = typeof(VRControllerGetControllerOffsetPatch).GetMethod(nameof(Prefix), BindingFlags.NonPublic | BindingFlags.Static);
        harmony.Patch(targetMethod, new HarmonyMethod(prefix));
    }

    [UsedImplicitly]
    private static bool Prefix(
        IVRPlatformHelper vrPlatformHelper,
        VRControllerTransformOffset transformOffset,
        XRNode node,
        out Pose poseOffset
    ) {
        poseOffset = Pose.identity;
        return PluginConfig.IsDeviceless && !PluginConfig.EnabledForDeviceless;
    }
}