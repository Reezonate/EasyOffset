using System.Reflection;
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

        [Init]
        public Plugin(IPALogger logger, Config config) {
            Log = logger;

            InitializeConfig(config);
            InitializeAssets();
            InitializeHarmony();
            InitializeUI();
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

        #region InitializeHarmony

        private static void InitializeHarmony() {
            var harmony = new Harmony("Reezonate.EasyOffset");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        #endregion

        #region InitializeUI

        private static void InitializeUI() {
            PersistentSingleton<BeatSaberMarkupLanguage.GameplaySetup.GameplaySetup>.instance.AddTab(
                "Easy Offset",
                "EasyOffset.Resources.BSML.ModPanelUI.bsml",
                PersistentSingleton<ModPanelUI>.instance
            );
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