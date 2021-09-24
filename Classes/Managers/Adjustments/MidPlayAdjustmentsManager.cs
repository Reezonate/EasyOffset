using System;
using EasyOffset.Configuration;
using JetBrains.Annotations;
using Zenject;

namespace EasyOffset {
    [UsedImplicitly]
    public class MidPlayAdjustmentsManager : IInitializable, IDisposable {
        private AdjustmentMode _adjustmentMode;

        public void Initialize() {
            _adjustmentMode = PluginConfig.AdjustmentMode;
            if (PluginConfig.EnableMidPlayAdjustment) return;

            PluginConfig.HideController();
            PluginConfig.AdjustmentMode = AdjustmentMode.None;
        }

        public void Dispose() {
            PluginConfig.AdjustmentMode = _adjustmentMode;
            PluginConfig.ShowController();
        }
    }
}