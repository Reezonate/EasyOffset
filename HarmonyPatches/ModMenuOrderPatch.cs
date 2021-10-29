using System;
using System.Collections.Generic;
using System.Reflection;
using BeatSaberMarkupLanguage.GameplaySetup;
using HarmonyLib;
using JetBrains.Annotations;

namespace EasyOffset.HarmonyPatches {
    [HarmonyPatch(
        typeof(GameplaySetup),
        "AddTab",
        typeof(Assembly),
        typeof(string),
        typeof(string),
        typeof(object),
        typeof(MenuType)
    )]
    internal class ModMenuOrderPatch {
        #region Reflection

        private static readonly FieldInfo MenusFieldInfo = typeof(GameplaySetup).GetField(
            "menus",
            BindingFlags.Instance | BindingFlags.NonPublic
        );

        [CanBeNull] private static readonly Type GameplaySetupMenuType = BsmlUtils.GetType("GameplaySetupMenu");

        [CanBeNull] private static readonly FieldInfo NameFieldInfo = GameplaySetupMenuType?.GetField(
            "name",
            BindingFlags.Instance | BindingFlags.Public
        );

        private static readonly bool ReflectionError = (MenusFieldInfo == null || NameFieldInfo == null);

        #endregion

        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private static void Postfix(GameplaySetup __instance) {
            if (ReflectionError) {
                Plugin.Log.Debug("BSML tab re-order patch failed: reflection error");
                return;
            }

            try {
                var menusList = (List<object>) MenusFieldInfo.GetValue(__instance);

                for (var i = menusList.Count - 1; i >= 0; i--) {
                    var menu = menusList[i];

                    var name = NameFieldInfo!.GetValue(menu).ToString();
                    if (name != Plugin.ModPanelTabName) continue;

                    menusList.RemoveAt(i);
                    menusList.Add(menu);
                    break;
                }
            } catch (Exception) {
                Plugin.Log.Debug("BSML tab re-order patch failed: exception");
                // ignored
            }
        }
    }
}