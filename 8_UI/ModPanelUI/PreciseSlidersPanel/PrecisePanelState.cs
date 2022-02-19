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

    #region SetPrecisePanelState

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
                PreciseSlidersHeight = 15.0f;
                PreciseFillerHeight = 0.0f;
                SetPrecisePanelScale(0.8f);
                break;
            case PrecisePanelState.PositionOnly:
                PrecisePanelActive = true;
                PreciseZOffsetActive = true;
                PrecisePositionActive = true;
                PreciseRotationActive = false;
                PreciseRotationReferenceActive = false;
                PreciseSlidersHeight = 37.0f;
                PreciseFillerHeight = 0.0f;
                SetPrecisePanelScale(0.8f);
                break;
            case PrecisePanelState.RotationOnly:
                PrecisePanelActive = true;
                PreciseZOffsetActive = false;
                PrecisePositionActive = false;
                PreciseRotationActive = false;
                PreciseRotationReferenceActive = true;
                PreciseSlidersHeight = 22.0f;
                PreciseFillerHeight = 0.0f;
                SetPrecisePanelScale(0.8f);
                break;
            case PrecisePanelState.Full:
                PrecisePanelActive = true;
                PreciseZOffsetActive = true;
                PrecisePositionActive = true;
                PreciseRotationActive = true;
                PreciseRotationReferenceActive = false;
                PreciseSlidersHeight = 64.0f;
                PreciseFillerHeight = 10.0f;
                SetPrecisePanelScale(0.78f);
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
            if (_precisePanelActive.Equals(value)) return;
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
            if (_preciseZOffsetActive.Equals(value)) return;
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
            if (_precisePositionActive.Equals(value)) return;
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
            if (_preciseRotationActive.Equals(value)) return;
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
            if (_preciseRotationReferenceActive.Equals(value)) return;
            _preciseRotationReferenceActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Scale

    [UIComponent("precise-panel-component")] [UsedImplicitly]
    private RectTransform _precisePanelComponent;

    private void SetPrecisePanelScale(float scale) {
        _precisePanelComponent.localScale = new Vector3(scale, scale, scale);
    }

    #endregion

    #region Dynamic style variables

    private float _preciseFillerHeight;

    [UIValue("precise-filler-height")]
    [UsedImplicitly]
    private float PreciseFillerHeight {
        get => _preciseFillerHeight;
        set {
            if (_preciseFillerHeight.Equals(value)) return;
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
            if (_preciseSlidersHeight.Equals(value)) return;
            _preciseSlidersHeight = value;
            NotifyPropertyChanged();
        }
    }

    [UIValue("precise-sliders-section-pad")] [UsedImplicitly]
    private float _preciseSlidersSectionPad = UnityGame.GameVersion.ToString() switch {
        "1.18.3" => 0.0f,
        _ => 1.0f
    };

    #endregion
}