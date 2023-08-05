using BeatSaberMarkupLanguage.Settings;
using BeatSaberMarkupLanguage.Util;

namespace EasyOffset {
    internal static class SettingsUIHelper {
        #region Initialize

        public static void Initialize() {
            AddTab();
        }

        #endregion

        #region Tab Management

        private const string ResourcePath = "EasyOffset._9_Resources.BSML.SettingsUI.bsml";
        private const string TabName = "Easy Offset";

        private static bool _tabActive;

        private static void AddTab() {
            if (_tabActive) return;
            PersistentSingleton<BSMLSettings>.instance.AddSettingsMenu(
                TabName,
                ResourcePath,
                PepegaSingletonFix<SettingsUI>.instance
            );
            _tabActive = true;
        }

        public static void RemoveTab() {
            if (!_tabActive) return;
            PersistentSingleton<BSMLSettings>.instance.RemoveSettingsMenu(TabName);
            _tabActive = false;
        }

        #endregion
    }
}