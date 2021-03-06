namespace EasyOffset {
    internal static class SettingsUIHelper {
        private const string ResourcePath = "EasyOffset._9_Resources.BSML.SettingsUI.bsml";
        private const string TabName = "Easy Offset";

        private static bool _tabActive;

        public static void AddTab() {
            if (_tabActive) return;
            PersistentSingleton<BeatSaberMarkupLanguage.Settings.BSMLSettings>.instance.AddSettingsMenu(
                TabName,
                ResourcePath,
                PersistentSingleton<SettingsUI>.instance
            );
            _tabActive = true;
        }

        public static void RemoveTab() {
            if (!_tabActive) return;
            PersistentSingleton<BeatSaberMarkupLanguage.Settings.BSMLSettings>.instance.RemoveSettingsMenu(TabName);
            _tabActive = false;
        }
    }
}