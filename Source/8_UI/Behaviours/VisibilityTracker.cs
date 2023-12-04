using UnityEngine;

namespace EasyOffset;

internal class VisibilityTracker : MonoBehaviour {
    private void OnEnable() {
        PluginConfig.IsModPanelVisible = true;
    }

    private void OnDisable() {
        PluginConfig.IsModPanelVisible = false;
        ConfigFileData.Instance.Changed();
    }
}