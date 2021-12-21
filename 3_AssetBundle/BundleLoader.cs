using System;
using UnityEngine;

namespace EasyOffset {
    public static class BundleLoader {
        private const string BundleName = "EasyOffset._9_Resources.AssetBundles.asset_bundle";

        #region Assets

        public static GameObject GizmosController;
        public static GameObject SwingBenchmarkController;

        #endregion

        #region Initialize

        private static bool _ready;

        public static void Initialize() {
            if (_ready) return;

            using var stream = ResourcesUtils.GetEmbeddedResourceStream(BundleName);
            var localAssetBundle = AssetBundle.LoadFromStream(stream);

            if (localAssetBundle == null) {
                throw new Exception("AssetBundle load error!");
            }

            LoadAssets(localAssetBundle);

            localAssetBundle.Unload(false);
            _ready = true;
        }

        #endregion

        #region LoadAssets

        private static void LoadAssets(AssetBundle assetBundle) {
            GizmosController = assetBundle.LoadAsset<GameObject>("GizmosController");
            SwingBenchmarkController = assetBundle.LoadAsset<GameObject>("SwingBenchmarkController");
        }

        #endregion
    }
}