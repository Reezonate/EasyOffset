using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.XR;

namespace EasyOffset.HarmonyPatches {
    [HarmonyPatch(typeof(DevicelessVRHelper))]
    [HarmonyPatch("AdjustControllerTransform")]
    internal class DevicelessVRHelperAdjustControllerTransform {
        [UsedImplicitly]
        private static bool Prefix(
            XRNode node,
            Transform transform,
            Vector3 position,
            Vector3 rotation
        ) {
            TransformUtils.AdjustControllerTransform(node, transform);
            return false;
        }
    }
}