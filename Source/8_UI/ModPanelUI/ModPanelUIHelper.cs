using BeatSaberMarkupLanguage.Util;

namespace EasyOffset {
    public static class ModPanelUIHelper {
        #region Initialize

        public static void Initialize() {
            PluginConfig.OnEnabledChange += OnEnabledChanged;
            OnEnabledChanged(PluginConfig.Enabled);
        }

        private static void OnEnabledChanged(bool enabled) {
            if (enabled) {
                AddTab();
            } else {
                RemoveTab();
            }
        }

        #endregion
        
        #region Tab management

        private const string ResourcePath = "EasyOffset._9_Resources.BSML.ModPanelUI.bsml";
        public const string TabName = "Easy Offset";

        private static bool _tabActive;

        private static void AddTab() {
            if (_tabActive) return;
            BeatSaberMarkupLanguage.GameplaySetup.GameplaySetup.Instance.AddTab(
                TabName,
                ResourcePath,
                PepegaSingletonFix<ModPanelUI>.instance
            );
            _tabActive = true;
        }

        public static void RemoveTab() {
            if (!_tabActive) return;
            BeatSaberMarkupLanguage.GameplaySetup.GameplaySetup.Instance.RemoveTab(TabName);
            _tabActive = false;
        }

        #endregion
    }
}