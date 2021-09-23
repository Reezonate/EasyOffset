using System;
using EasyOffset.Configuration;
using JetBrains.Annotations;
using Zenject;

namespace EasyOffset {
    [UsedImplicitly]
    public class MidPlayAdjustmentsManager : IInitializable, IDisposable {
        private GripButtonAction _gripButtonAction;

        public void Initialize() {
            _gripButtonAction = PluginConfig.GripButtonAction;
            if (PluginConfig.EnableMidPlayAdjustment) return;

            PluginConfig.HideController();
            PluginConfig.GripButtonAction = GripButtonAction.None;
        }

        public void Dispose() {
            PluginConfig.GripButtonAction = _gripButtonAction;
            PluginConfig.ShowController();
        }
    }
}