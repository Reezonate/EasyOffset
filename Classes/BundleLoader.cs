using System;
using UnityEngine;

namespace EasyOffset {
    public static class BundleLoader {
        private const string BundleName = "EasyOffset.Resources.AssetBundles.easy_offset";

        #region Gizmos

        public static GameObject PivotPrefab;
        public static GameObject DirectionGridPrefab;
        public static GameObject TrailPrefab;

        #endregion

        #region Controllers

        public static GameObject OculusCV1Right;
        public static GameObject OculusCV1Left;

        public static GameObject OculusQuest2Right;
        public static GameObject OculusQuest2Left;

        public static GameObject RiftSRight;
        public static GameObject RiftSLeft;

        public static GameObject ValveIndexRight;
        public static GameObject ValveIndexLeft;

        public static GameObject ViveTracker2;

        public static GameObject ViveTracker3;

        public static GameObject Vive;

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

            LoadGizmosPrefabs(localAssetBundle);
            LoadControllersPrefabs(localAssetBundle);

            localAssetBundle.Unload(false);
            _ready = true;
        }

        #endregion

        #region LoadGizmosPrefabs

        private static void LoadGizmosPrefabs(AssetBundle assetBundle) {
            PivotPrefab = assetBundle.LoadAsset<GameObject>("Pivot");
            DirectionGridPrefab = assetBundle.LoadAsset<GameObject>("DirectionGrid");
            TrailPrefab = assetBundle.LoadAsset<GameObject>("Trail");
        }

        #endregion

        #region LoadControllersPrefabs

        private static void LoadControllersPrefabs(AssetBundle assetBundle) {
            OculusCV1Right = assetBundle.LoadAsset<GameObject>("OculusCV1_Right");
            OculusCV1Left = assetBundle.LoadAsset<GameObject>("OculusCV1_Left");

            OculusQuest2Right = assetBundle.LoadAsset<GameObject>("OculusQuest2_Right");
            OculusQuest2Left = assetBundle.LoadAsset<GameObject>("OculusQuest2_Left");

            RiftSRight = assetBundle.LoadAsset<GameObject>("RiftS_Right");
            RiftSLeft = assetBundle.LoadAsset<GameObject>("RiftS_Left");

            ValveIndexRight = assetBundle.LoadAsset<GameObject>("ValveIndex_Right");
            ValveIndexLeft = assetBundle.LoadAsset<GameObject>("ValveIndex_Left");

            ViveTracker2 = assetBundle.LoadAsset<GameObject>("ViveTracker2");

            ViveTracker3 = assetBundle.LoadAsset<GameObject>("ViveTracker3");

            Vive = assetBundle.LoadAsset<GameObject>("Vive");
        }

        #endregion
    }
}