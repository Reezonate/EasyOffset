using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;

namespace EasyOffset;

internal partial class ModPanelUI {
    #region Active

    private bool _nonePanelActive = true;

    [UIValue("none-panel-active")]
    [UsedImplicitly]
    private bool NonePanelActive {
        get => _nonePanelActive;
        set {
            _nonePanelActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion
}