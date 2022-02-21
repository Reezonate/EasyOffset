using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EasyOffset;

[HarmonyPatch(typeof(Selectable), "OnPointerDown")]
internal static class OnPointerDownPatch {
    [UsedImplicitly]
    private static void Prefix(PointerEventData eventData) {
        ReeInputManager.NotifyPointerDown(eventData);
    }
}

[HarmonyPatch(typeof(Selectable), "OnPointerUp")]
internal static class OnPointerUpPatch {
    [UsedImplicitly]
    private static void Prefix(PointerEventData eventData) {
        ReeInputManager.NotifyPointerUp(eventData);
    }
}