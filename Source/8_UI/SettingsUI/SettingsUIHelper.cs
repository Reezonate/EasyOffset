using BeatSaberMarkupLanguage.Settings;
using Zenject;

namespace EasyOffset {
    internal class SettingsUIHelper : IInitializable {
        #region Initialize

        public void Initialize() {
            AddTab();
        }

        #endregion

        #region Tab Management

        private const string ResourcePath = "EasyOffset._9_Resources.BSML.SettingsUI.bsml";
        private const string TabName = "Easy Offset";

        private bool _tabActive;

        private void AddTab() {
            if (_tabActive) return;
            BSMLSettings.Instance.AddSettingsMenu(
                TabName,
                ResourcePath,
                PepegaSingletonFix<SettingsUI>.instance
            );
            _tabActive = true;
        }

        public void RemoveTab() {
            if (!_tabActive) return;
            BSMLSettings.Instance.RemoveSettingsMenu(TabName);
            _tabActive = false;
        }

        #endregion
    }
}