using System.Reflection;
using EasyOffset.AssetBundleScripts;
using EasyOffset.Configuration;
using EasyOffset.UI;
using HarmonyLib;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using JetBrains.Annotations;
using IPALogger = IPA.Logging.Logger;

namespace EasyOffset {
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin {
        internal static IPALogger Log { get; private set; }
        private static Harmony _harmony;

        [Init]
        public Plugin(IPALogger logger, Config config) {
            Log = logger;
            _harmony = new Harmony("Reezonate.EasyOffset");

            InitializeConfig(config);
            InitializeAssets();
            InitializeSettingsUI();

            SubscribeEnabled();
            EnabledChangeHandler(PluginConfig.Enabled);
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

        #region Harmony

        private static void ApplyHarmonyPatches() {
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        private static void RemoveHarmonyPatches() {
            _harmony.UnpatchAll();
        }

        #endregion

        #region UI

        private static void InitializeSettingsUI() {
            PersistentSingleton<BeatSaberMarkupLanguage.Settings.BSMLSettings>.instance.AddSettingsMenu(
                "Easy Offset",
                "EasyOffset.Resources.BSML.SettingsUI.bsml",
                PersistentSingleton<SettingsUI>.instance
            );
        }

        private static void AddModPanelTab() {
            PersistentSingleton<BeatSaberMarkupLanguage.GameplaySetup.GameplaySetup>.instance.AddTab(
                "Easy Offset",
                "EasyOffset.Resources.BSML.ModPanelUI.bsml",
                PersistentSingleton<ModPanelUI>.instance
            );
        }

        private static void RemoveModPanelTab() {
            PersistentSingleton<BeatSaberMarkupLanguage.GameplaySetup.GameplaySetup>.instance.RemoveTab("Easy Offset");
        }

        #endregion

        #region Enabled observing

        private static void SubscribeEnabled() {
            PluginConfig.OnEnabledChange += EnabledChangeHandler;
        }

        private static void EnabledChangeHandler(bool enabled) {
            if (enabled) {
                ApplyHarmonyPatches();
                AddModPanelTab();
            } else {
                RemoveHarmonyPatches();
                RemoveModPanelTab();
            }
        }

        #endregion

        #region Generic IPA methods

        [OnStart]
        [UsedImplicitly]
        public void OnApplicationStart() { }

        [OnExit]
        [UsedImplicitly]
        public void OnApplicationQuit() { }

        #endregion
    }
}