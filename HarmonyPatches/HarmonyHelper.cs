using System.Reflection;
using HarmonyLib;

namespace EasyOffset.HarmonyPatches {
    public static class HarmonyHelper {
        private const string HarmonyID = "Reezonate.EasyOffset";
        private static Harmony _harmony;

        private static bool _initialized;

        private static void LazyInit() {
            if (_initialized) return;
            _harmony = new Harmony(HarmonyID);
            _initialized = true;
        }

        public static void ApplyPatches() {
            LazyInit();
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public static void RemovePatches() {
            if (!_initialized) return;
            _harmony.UnpatchAll(HarmonyID);
        }
    }
}