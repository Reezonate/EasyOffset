using System;
using UnityEngine;

namespace EasyOffset {
    public static class BundleLoader {
        private const string BundleName = "EasyOffset.Resources.AssetBundles.easy_offset";

        #region Assets

        public static Mesh SphereMesh;
        public static GameObject GizmosController;

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
            SphereMesh = assetBundle.LoadAsset<GameObject>("Sphere").GetComponent<MeshFilter>().mesh;
            GizmosController = assetBundle.LoadAsset<GameObject>("GizmosController");
        }

        #endregion
    }
}