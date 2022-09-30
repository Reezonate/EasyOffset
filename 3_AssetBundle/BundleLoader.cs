using System;
using UnityEngine;

namespace EasyOffset {
    public static class BundleLoader {
        private const string BundleName = "EasyOffset._9_Resources.AssetBundles.asset_bundle";

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
            LoadMaterials(localAssetBundle);
            LoadSprites(localAssetBundle);

            localAssetBundle.Unload(false);
            _ready = true;
        }

        #endregion

        #region Assets

        public static GameObject GizmosController;
        public static GameObject SwingBenchmarkController;

        private static void LoadAssets(AssetBundle assetBundle) {
            GizmosController = assetBundle.LoadAsset<GameObject>("GizmosController");
            SwingBenchmarkController = assetBundle.LoadAsset<GameObject>("SwingBenchmarkController");
        }

        #endregion

        #region Materials

        public static Material UndoRedoButtonsMaterial;
        public static Material UIVideoPlayerMaterial;

        private static void LoadMaterials(AssetBundle assetBundle) {
            UndoRedoButtonsMaterial = assetBundle.LoadAsset<Material>("UndoRedoButtonsMaterial");
            UIVideoPlayerMaterial = assetBundle.LoadAsset<Material>("UIVideoPlayerMaterial");
        }

        #endregion

        #region Sprites

        public static Sprite PlayIcon;
        public static Sprite PauseIcon;
        public static Sprite KappaIcon;
        public static Sprite KuuramaIcon;
        public static Sprite ChromiaIcon;

        private static void LoadSprites(AssetBundle assetBundle) {
            PlayIcon = assetBundle.LoadAsset<Sprite>("EO_PlayIcon");
            PauseIcon = assetBundle.LoadAsset<Sprite>("EO_PauseIcon");
            KappaIcon = assetBundle.LoadAsset<Sprite>("EO_KappaIcon");
            KuuramaIcon = assetBundle.LoadAsset<Sprite>("EO_Kuurama");
            ChromiaIcon = assetBundle.LoadAsset<Sprite>("EO_Chromia");
        }

        #endregion
    }
}