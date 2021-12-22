using System.Reflection;
using HarmonyLib;

namespace EasyOffset {
    public static class HarmonyHelper {
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

        public static void ApplyPermanentPatches() {
            LazyInit();
            AppInstallerPermanentPatch.ApplyPatch(_permanentHarmony);
        }

        public static void ApplyRemovablePatches() {
            LazyInit();
            _removableHarmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public static void RemoveRemovablePatches() {
            if (!_initialized) return;
            _removableHarmony.UnpatchSelf();
        }
    }
}