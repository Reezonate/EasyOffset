﻿using IPA;
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
            ConfigUpgrade.TryUpgrade();
            ConfigFileData.Instance = config.Generated<ConfigFileData>();
        }

        #endregion

        #region InitializeAssets

        private static void InitializeAssets() {
            BundleLoader.Initialize();
        }

        #endregion

        #region OnApplicationStart

        [OnStart, UsedImplicitly]
        public void OnApplicationStart() {
            HarmonyHelper.Initialize();
        }

        #endregion

        #region OnApplicationQuit

        [OnExit, UsedImplicitly]
        public void OnApplicationQuit() { }

        #endregion
    }
}