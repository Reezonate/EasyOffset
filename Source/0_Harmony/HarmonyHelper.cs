using System.Reflection;
using HarmonyLib;

namespace EasyOffset {
    public static class HarmonyHelper {
        #region Initialize

        public static void Initialize() {
            ApplyPermanentPatches();

            PluginConfig.OnEnabledChange += OnEnabledChanged;
            OnEnabledChanged(PluginConfig.Enabled);
        }

        private static void OnEnabledChanged(bool enabled) {
            if (enabled) {
                ApplyRemovablePatches();
            } else {
                RemoveRemovablePatches();
            }
        }

        #endregion

        #region Patching

        private const string RemovableHarmonyID = "Reezonate.EasyOffset.Removable";
        private const string PermanentHarmonyID = "Reezonate.EasyOffset.Permanent";

        private static Harmony _removableHarmony;
        private static Harmony _permanentHarmony;

        private static bool _initialized;

        private static void LazyInit() {
            if (_initialized) return;
            _removableHarmony = new Harmony(RemovableHarmonyID);
            _permanentHarmony = new Harmony(PermanentHarmonyID);
            _initialized = true;
        }

        private static void ApplyPermanentPatches() {
            LazyInit();
            AppInstallerPermanentPatch.ApplyPatch(_permanentHarmony);
            MenuInstallerPermanentPatch.ApplyPatch(_permanentHarmony);
            MainSettingsModelPermanentPatch.ApplyPatch(_permanentHarmony);
        }

        private static void ApplyRemovablePatches() {
            LazyInit();
            _removableHarmony.PatchAll(Assembly.GetExecutingAssembly());
            VRControllerGetControllerOffsetPatch.ApplyPatch(_removableHarmony);
        }

        private static void RemoveRemovablePatches() {
            if (!_initialized) return;
            _removableHarmony.UnpatchSelf();
        }

        #endregion
    }
}