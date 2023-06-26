using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine.XR;

namespace EasyOffset;

[HarmonyPatch(typeof(VRController), "UpdateAnchorPosition")]
internal class VRControllerUpdateAnchorPatch {
    [UsedImplicitly]
    private static bool Prefix(XRNode ____node) {
        return ____node switch {
            XRNode.LeftHand => false,
            XRNode.RightHand => false,
            _ => true
        };
    }
}