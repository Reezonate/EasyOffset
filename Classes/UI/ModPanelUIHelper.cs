using EasyOffset.Configuration;

namespace EasyOffset.UI {
    public static class ModPanelUIHelper {
        #region Tab management

        private const string ResourcePath = "EasyOffset.Resources.BSML.ModPanelUI.bsml";
        public const string TabName = "Easy Offset";

        private static bool _tabActive;

        public static void AddTab() {
            if (_tabActive) return;
            PersistentSingleton<BeatSaberMarkupLanguage.GameplaySetup.GameplaySetup>.instance.AddTab(
                TabName,
                ResourcePath,
                PersistentSingleton<ModPanelUI>.instance
            );
            _tabActive = true;
        }

        public static void RemoveTab() {
            if (!_tabActive) return;
            PersistentSingleton<BeatSaberMarkupLanguage.GameplaySetup.GameplaySetup>.instance.RemoveTab(TabName);
            _tabActive = false;
        }

        #endregion

        #region Visibility

        private static AdjustmentMode _lastAdjustmentMode = AdjustmentMode.None;

        public static void OnPanelBecomeVisible() {
            PluginConfig.AdjustmentMode = _lastAdjustmentMode;
            PluginConfig.ShowController();
        }

        public static void OnPanelBecomeInvisible() {
            _lastAdjustmentMode = PluginConfig.AdjustmentMode;
            PluginConfig.AdjustmentMode = AdjustmentMode.None;
            PluginConfig.HideController();
        }

        #endregion
    }
}