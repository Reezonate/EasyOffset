using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.XR;

namespace EasyOffset;

[HarmonyPatch(typeof(VRController), "Update")]
internal class VRControllerUpdatePatch {
    private static readonly Vector3 DefaultLeftPosition = new Vector3(-0.2f, 0.05f, 0.0f);
    private static readonly Vector3 DefaultRightPosition = new Vector3(0.2f, 0.05f, 0.0f);

    [UsedImplicitly]
    // ReSharper disable InconsistentNaming
    private static bool Prefix(
        VRController __instance,
        IVRPlatformHelper ____vrPlatformHelper,
        ref Vector3 ____lastTrackedPosition,
        ref Quaternion ____lastTrackedRotation
    ) {
        if (PluginConfig.IsDeviceless && !PluginConfig.EnabledForDeviceless) return true;

        if (____vrPlatformHelper.GetNodePose(__instance.node, __instance.nodeIdx, out var pos, out var rot)) {
            ____lastTrackedPosition = pos;
            ____lastTrackedRotation = rot;
        } else {
            pos = ____lastTrackedPosition != Vector3.zero ? ____lastTrackedPosition : (__instance.node == XRNode.LeftHand ? DefaultLeftPosition : DefaultRightPosition);
            rot = ____lastTrackedRotation != Quaternion.identity ? ____lastTrackedRotation : Quaternion.identity;
        }

        var transform = __instance.transform;
        transform.SetLocalPositionAndRotation(pos, rot);
        TransformUtils.AdjustControllerTransform(__instance.node, transform);
        return false;
    }
}