using System;
using BeatSaberMarkupLanguage.Attributes;
using IPA.Utilities;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal partial class ModPanelUI {
    #region Events

    private void SubscribeToPrecisePanelEvents() {
        PluginConfig.ConfigWasChangedEvent += OnConfigWasChanged;
        PluginConfig.IsModPanelVisibleChangedEvent += OnIsModPanelVisibleChanged;
        OnConfigWasChanged();
    }

    private void OnConfigWasChanged() {
        if (_smoothingEnabled) return;
        NotifySynchronizationRequired();
    }

    private void OnIsModPanelVisibleChanged(bool isVisible) {
        if (!isVisible) return;
        SetPrecisePanelState(_precisePanelState);
    }

    #endregion

    #region State

    private PrecisePanelState _precisePanelState = PrecisePanelState.Hidden;

    private void SetPrecisePanelState(PrecisePanelState newState) {
        switch (newState) {
            case PrecisePanelState.Hidden:
                PrecisePanelActive = false;
                break;
            case PrecisePanelState.ZOffsetOnly:
                PrecisePanelActive = true;
                PreciseZOffsetActive = true;
                PrecisePositionActive = false;
                PreciseRotationActive = false;
                PreciseRotationReferenceActive = false;
                PreciseSlidersHeight = 8.0f;
                PreciseFillerHeight = 0.0f;
                ApplyScale(0.82f);
                break;
            case PrecisePanelState.PositionOnly:
                PrecisePanelActive = true;
                PreciseZOffsetActive = true;
                PrecisePositionActive = true;
                PreciseRotationActive = false;
                PreciseRotationReferenceActive = false;
                PreciseSlidersHeight = 27.0f;
                PreciseFillerHeight = 0.0f;
                ApplyScale(0.82f);
                break;
            case PrecisePanelState.RotationOnly:
                PrecisePanelActive = true;
                PreciseZOffsetActive = false;
                PrecisePositionActive = false;
                PreciseRotationActive = false;
                PreciseRotationReferenceActive = true;
                PreciseSlidersHeight = 15.0f;
                PreciseFillerHeight = 0.0f;
                ApplyScale(0.82f);
                break;
            case PrecisePanelState.Full:
                PrecisePanelActive = true;
                PreciseZOffsetActive = true;
                PrecisePositionActive = true;
                PreciseRotationActive = true;
                PreciseRotationReferenceActive = false;
                PreciseSlidersHeight = 44.0f;
                PreciseFillerHeight = 22.0f;
                ApplyScale(0.8f);
                break;
            default: throw new ArgumentOutOfRangeException();
        }

        _precisePanelState = newState;
        NotifySynchronizationRequired();
    }

    private enum PrecisePanelState {
        Hidden,
        ZOffsetOnly,
        PositionOnly,
        RotationOnly,
        Full
    }

    #endregion

    #region Active

    private bool _precisePanelActive;

    [UIValue("precise-panel-active")]
    [UsedImplicitly]
    private bool PrecisePanelActive {
        get => _precisePanelActive;
        set {
            _precisePanelActive = value;
            NotifyPropertyChanged();
        }
    }

    private bool _preciseZOffsetActive;

    [UIValue("precise-z-offset-active")]
    [UsedImplicitly]
    private bool PreciseZOffsetActive {
        get => _preciseZOffsetActive;
        set {
            _preciseZOffsetActive = value;
            NotifyPropertyChanged();
        }
    }

    private bool _precisePositionActive;

    [UIValue("precise-position-active")]
    [UsedImplicitly]
    private bool PrecisePositionActive {
        get => _precisePositionActive;
        set {
            _precisePositionActive = value;
            NotifyPropertyChanged();
        }
    }

    private bool _preciseRotationActive;

    [UIValue("precise-rotation-active")]
    [UsedImplicitly]
    private bool PreciseRotationActive {
        get => _preciseRotationActive;
        set {
            _preciseRotationActive = value;
            NotifyPropertyChanged();
        }
    }

    private bool _preciseRotationReferenceActive;

    [UIValue("precise-rotation-reference-active")]
    [UsedImplicitly]
    private bool PreciseRotationReferenceActive {
        get => _preciseRotationReferenceActive;
        set {
            _preciseRotationReferenceActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Interactable

    private bool _preciseLeftRotationReferenceInteractable = PluginConfig.LeftSaberHasReference;

    [UIValue("precise-left-rotation-reference-interactable")]
    [UsedImplicitly]
    private bool PreciseLeftRotationReferenceInteractable {
        get => _preciseLeftRotationReferenceInteractable;
        set {
            _preciseLeftRotationReferenceInteractable = value;
            NotifyPropertyChanged();
        }
    }

    private bool _preciseRightRotationReferenceInteractable = PluginConfig.RightSaberHasReference;

    [UIValue("precise-right-rotation-reference-interactable")]
    [UsedImplicitly]
    private bool PreciseRightRotationReferenceInteractable {
        get => _preciseRightRotationReferenceInteractable;
        set {
            _preciseRightRotationReferenceInteractable = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Filler, Height, Scaling

    private float _preciseFillerHeight;

    [UIValue("precise-filler-height")]
    [UsedImplicitly]
    private float PreciseFillerHeight {
        get => _preciseFillerHeight;
        set {
            _preciseFillerHeight = value;
            NotifyPropertyChanged();
        }
    }

    private float _preciseSlidersHeight;

    [UIValue("precise-sliders-section-height")]
    [UsedImplicitly]
    private float PreciseSlidersHeight {
        get => _preciseSlidersHeight;
        set {
            _preciseSlidersHeight = value;
            NotifyPropertyChanged();
        }
    }

    [UIComponent("precise-panel-component")] [UsedImplicitly]
    private RectTransform _precisePanelComponent;

    private void ApplyScale(float scale) {
        _precisePanelComponent.localScale = Vector3.one * scale;
    }

    [UIValue("precise-sliders-section-pad")] [UsedImplicitly]
    private float _precisePanelSlidersWidth = UnityGame.GameVersion.ToString() switch {
        "1.18.3" => 0.0f,
        _ => 1.0f
    };

    #endregion
}