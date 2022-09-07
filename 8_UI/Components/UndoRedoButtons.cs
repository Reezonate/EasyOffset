using System.Collections;
using BeatSaberMarkupLanguage.Attributes;
using HMUI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace EasyOffset;

internal class UndoRedoButtons : ReeUIComponentV2 {
    #region OnInitialize

    protected override void OnInitialize() {
        InitializeAnimations();

        PluginConfig.UndoAvailableChangedEvent += OnUndoAvailableChanged;
        PluginConfig.RedoAvailableChangedEvent += OnRedoAvailableChanged;
        OnUndoAvailableChanged(PluginConfig.UndoAvailable, PluginConfig.UndoDescription);
        OnRedoAvailableChanged(PluginConfig.RedoAvailable, PluginConfig.RedoDescription);
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
        if (isAvailable) _undoBlinkAnimation.Play();
    }

    private void OnRedoAvailableChanged(bool isAvailable, string description) {
        RedoButtonInteractable = isAvailable;
        RedoButtonHoverHint = isAvailable ? $"Redo '{description}'" : "Redo";
        if (isAvailable) _redoBlinkAnimation.Play();
    }

    #endregion

    #region Animation

    private BlinkAnimation _undoBlinkAnimation;
    private BlinkAnimation _redoBlinkAnimation;

    private void InitializeAnimations() {
        _undoBlinkAnimation = _undoButton.gameObject.AddComponent<BlinkAnimation>();
        _redoBlinkAnimation = _redoButton.gameObject.AddComponent<BlinkAnimation>();
    }

    private class BlinkAnimation : MonoBehaviour {
        private static readonly int TintValuePropertyId = Shader.PropertyToID("_TintValue");

        private Material _materialInstance;
        private float _tintValue;
        private bool _ready;

        private void Awake() {
            _materialInstance = Instantiate(BundleLoader.UndoRedoButtonsMaterial);
            gameObject.GetComponentInChildren<ImageView>().material = _materialInstance;
            _ready = true;
        }

        private void OnEnable() {
            _tintValue = 0.0f;
            UpdateMaterial();
        }

        private void OnDestroy() {
            _ready = false;
        }

        public void Play() {
            if (!_ready || !gameObject.activeInHierarchy) return;
            StopAllCoroutines();
            StartCoroutine(BlinkCoroutine());
        }

        private IEnumerator BlinkCoroutine() {
            _tintValue = 1.0f;
            UpdateMaterial();
            yield return new WaitForEndOfFrame();

            while (_tintValue > 1e-4f) {
                _tintValue = Mathf.Lerp(_tintValue, 0.0f, Time.deltaTime * 10);
                UpdateMaterial();
                yield return new WaitForEndOfFrame();
            }

            _tintValue = 0.0f;
            UpdateMaterial();
        }

        private void UpdateMaterial() {
            _materialInstance.SetFloat(TintValuePropertyId, _tintValue);
        }
    }

    #endregion

    #region Undo button

    [UIComponent("undo-button"), UsedImplicitly] private Button _undoButton;

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
            if (_undoButtonHoverHint.Equals(value)) return;
            _undoButtonHoverHint = value;
            NotifyPropertyChanged();
            UIEvents.NotifyHoverHintUpdated();
        }
    }

    [UIAction("undo-button-on-click"), UsedImplicitly]
    private void UndoButtonOnClick() {
        PluginConfig.UndoLastChange();
    }

    #endregion

    #region Redo button

    [UIComponent("redo-button"), UsedImplicitly] private Button _redoButton;

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
            if (_redoButtonHoverHint.Equals(value)) return;
            _redoButtonHoverHint = value;
            NotifyPropertyChanged();
            UIEvents.NotifyHoverHintUpdated();
        }
    }

    [UIAction("redo-button-on-click"), UsedImplicitly]
    private void RedoButtonOnClick() {
        PluginConfig.RedoLastChange();
    }

    #endregion
}