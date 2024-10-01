using Zenject;

namespace EasyOffset {
    public class ModPanelUIHelper : IInitializable {
        #region Initialize

        public void Initialize() {
            PluginConfig.OnEnabledChange += OnEnabledChanged;
            OnEnabledChanged(PluginConfig.Enabled);
        }

        private void OnEnabledChanged(bool enabled) {
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

        private bool _tabActive;

        private void AddTab() {
            if (_tabActive) return;
            BeatSaberMarkupLanguage.GameplaySetup.GameplaySetup.Instance.AddTab(
                TabName,
                ResourcePath,
                PepegaSingletonFix<ModPanelUI>.instance
            );
            _tabActive = true;
        }

        public void RemoveTab() {
            if (!_tabActive) return;
            BeatSaberMarkupLanguage.GameplaySetup.GameplaySetup.Instance.RemoveTab(TabName);
            _tabActive = false;
        }

        #endregion
    }
}