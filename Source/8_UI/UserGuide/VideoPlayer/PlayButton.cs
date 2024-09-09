using System;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EasyOffset;

internal class PlayButton : ReeUIComponentV2 {
    #region Initialize

    protected override void OnInitialize() {
        InitializeButton();
        UpdateVisuals();
    }

    #endregion
    
    #region Interaction

    public event Action OnClickEvent;

    public void SetActive(bool value) {
        _container.gameObject.SetActive(value);
    }

    public void NotifyWasClicked(PointerEventData pointerEventData) {
        OnClickEvent?.Invoke();
    }

    #endregion

    #region State

    private State _currentState = State.Pause;
    private float _alpha;

    public void SetState(State newState) {
        _currentState = newState;
        UpdateVisuals();
    }

    public enum State {
        Play,
        Pause
    }

    #endregion

    #region UpdateVisuals

    private static readonly Color BaseColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    private void UpdateVisuals() {
        var col = BaseColor.ColorWithAlpha(_alpha);
        _button.DefaultColor = col;
        _button.HighlightColor = col;
        
        _button.sprite = _currentState switch {
            State.Play => BundleLoader.PlayIcon,
            State.Pause => BundleLoader.PauseIcon,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    #endregion
    
    #region Button

    [UIComponent("container-component"), UsedImplicitly]
    private RectTransform _container;

    [UIComponent("image-component"), UsedImplicitly]
    private ClickableImage _button;

    private SmoothHoverController _hoverController;

    private void InitializeButton() {
        _hoverController = _button.gameObject.AddComponent<SmoothHoverController>();
        _hoverController.HoverStateChangedEvent += OnHover;
        _button.OnClickEvent += NotifyWasClicked;
    }

    private void OnHover(bool isHovered, float progress) {
        var scale = 1.0f + 0.5f * progress;
        _button.transform.localScale = new Vector3(scale, scale, scale);
        _alpha = 0.5f + 0.5f * progress;
        UpdateVisuals();
    }

    #endregion
}