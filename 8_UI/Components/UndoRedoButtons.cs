using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;

namespace EasyOffset;

internal class UndoRedoButtons : ReeUIComponentV2 {
    #region OnInitialize

    protected override void OnInitialize() {
        PluginConfig.UndoAvailableChangedEvent += OnUndoAvailableChanged;
        PluginConfig.RedoAvailableChangedEvent += OnRedoAvailableChanged;
    }

    protected override void OnDispose() {
        PluginConfig.UndoAvailableChangedEvent -= OnUndoAvailableChanged;
        PluginConfig.RedoAvailableChangedEvent -= OnRedoAvailableChanged;
    }

    #endregion

    #region Events

    private void OnUndoAvailableChanged(bool isAvailable, string description) {
        UndoButtonInteractable = isAvailable;
        UndoButtonHoverHint = isAvailable ? $"Undo '{description}'" : "Undo";
    }

    private void OnRedoAvailableChanged(bool isAvailable, string description) {
        RedoButtonInteractable = isAvailable;
        RedoButtonHoverHint = isAvailable ? $"Redo '{description}'" : "Redo";
    }

    #endregion

    #region Undo button

    private bool _undoButtonInteractable;

    [UIValue("undo-button-interactable"), UsedImplicitly]
    private bool UndoButtonInteractable {
        get => _undoButtonInteractable;
        set {
            if (_undoButtonInteractable.Equals(value)) return;
            _undoButtonInteractable = value;
            NotifyPropertyChanged();
        }
    }

    private string _undoButtonHoverHint = "Undo";

    [UIValue("undo-button-hover-hint"), UsedImplicitly]
    private string UndoButtonHoverHint {
        get => _undoButtonHoverHint;
        set {
            _undoButtonHoverHint = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("undo-button-on-click"), UsedImplicitly]
    private void UndoButtonOnClick() {
        PluginConfig.UndoLastChange();
    }

    #endregion

    #region Redo button

    private bool _redoButtonInteractable;

    [UIValue("redo-button-interactable"), UsedImplicitly]
    private bool RedoButtonInteractable {
        get => _redoButtonInteractable;
        set {
            if (_redoButtonInteractable.Equals(value)) return;
            _redoButtonInteractable = value;
            NotifyPropertyChanged();
        }
    }

    private string _redoButtonHoverHint = "Redo";

    [UIValue("redo-button-hover-hint"), UsedImplicitly]
    private string RedoButtonHoverHint {
        get => _redoButtonHoverHint;
        set {
            _redoButtonHoverHint = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("redo-button-on-click"), UsedImplicitly]
    private void RedoButtonOnClick() {
        PluginConfig.RedoLastChange();
    }

    #endregion
}