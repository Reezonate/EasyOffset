using EasyOffset.AssetBundleScripts;
using EasyOffset.Configuration;
using EasyOffset.HarmonyPatches;
using EasyOffset.UI;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using JetBrains.Annotations;
using IPALogger = IPA.Logging.Logger;

namespace EasyOffset {
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin {
        internal static IPALogger Log { get; private set; }

        [Init]
        public Plugin(IPALogger logger, Config config) {
            Log = logger;

            InitializeConfig(config);
            InitializeAssets();
        }

        #region InitializeConfig

        private static void InitializeConfig(Config config) {
            ConfigFileData.Instance = config.Generated<ConfigFileData>();
        }

        #endregion

        #region InitializeAssets

        private static void InitializeAssets() {
            BundleLoader.Initialize();
        }

        #endregion

        #region Enabled observing

        private static void SubscribeEnabled() {
            PluginConfig.OnEnabledChange += EnabledChangeHandler;
        }

        private static void EnabledChangeHandler(bool enabled) {
            if (enabled) {
                HarmonyHelper.ApplyPatches();
                ModPanelUIHelper.AddTab();
            } else {
                HarmonyHelper.RemovePatches();
                ModPanelUIHelper.RemoveTab();
            }
        }

        #endregion

        #region OnApplicationStart

        [OnStart]
        [UsedImplicitly]
        public void OnApplicationStart() {
            SubscribeEnabled();
            EnabledChangeHandler(PluginConfig.Enabled);
            SettingsUIHelper.AddTab();
        }

        #endregion

        #region OnApplicationQuit

        [OnExit]
        [UsedImplicitly]
        public void OnApplicationQuit() { }

        #endregion
    }
}