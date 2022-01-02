using System;
using JetBrains.Annotations;
using Zenject;

namespace EasyOffset;

[UsedImplicitly]
public class AdjustmentBlockManager : IInitializable, IDisposable {
    #region ChangeAdjustmentMode

    private AdjustmentMode _lastAdjustmentMode;
    private bool _enabled;

    private void EnableAdjustments() {
        if (_enabled) return;
        PluginConfig.AdjustmentMode = _lastAdjustmentMode;
        _enabled = true;
    }

    private void DisableAdjustments() {
        if (!_enabled) return;
        _lastAdjustmentMode = PluginConfig.AdjustmentMode;
        PluginConfig.AdjustmentMode = AdjustmentMode.None;
        _enabled = false;
    }

    #endregion

    #region Events

    private void OnIsModPanelVisibleChanged(bool value) {
        if (value) {
            EnableAdjustments();
        } else {
            DisableAdjustments();
        }
    }

    #endregion

    #region Subscription

    public void Initialize() {
        PluginConfig.IsModPanelVisibleChangedEvent += OnIsModPanelVisibleChanged;
        OnIsModPanelVisibleChanged(PluginConfig.IsModPanelVisible);
    }

    public void Dispose() {
        PluginConfig.IsModPanelVisibleChangedEvent -= OnIsModPanelVisibleChanged;
    }

    #endregion
}