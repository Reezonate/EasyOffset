using System;
using BeatSaberMarkupLanguage.Attributes;
using IPA.Utilities;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal partial class SlidersPanel {
    #region Events

    private void OnConfigWasChanged() {
        if (_smoothingEnabled) return;
        NotifySynchronizationRequired();
    }

    private void OnIsModPanelVisibleChanged(bool isVisible) {
        if (!isVisible) return;
        SetDirectPanelState(_directPanelState);
    }

    #endregion

    #region SetDirectPanelState

    private DirectPanelState _directPanelState = DirectPanelState.Hidden;

    private void SetDirectPanelState(DirectPanelState newState) {
        switch (newState) {
            case DirectPanelState.Hidden:
                DirectPanelActive = false;
                break;
            case DirectPanelState.ZOffsetOnly:
                DirectPanelActive = true;
                DirectZOffsetActive = true;
                DirectPositionActive = false;
                DirectRotationActive = false;
                DirectRotationReferenceActive = false;
                DirectSlidersHeight = 15.0f;
                DirectFillerHeight = 0.0f;
                SetDirectPanelScale(0.8f);
                break;
            case DirectPanelState.PositionOnly:
                DirectPanelActive = true;
                DirectZOffsetActive = true;
                DirectPositionActive = true;
                DirectRotationActive = false;
                DirectRotationReferenceActive = false;
                DirectSlidersHeight = 37.0f;
                DirectFillerHeight = 0.0f;
                SetDirectPanelScale(0.8f);
                break;
            case DirectPanelState.RotationOnly:
                DirectPanelActive = true;
                DirectZOffsetActive = false;
                DirectPositionActive = false;
                DirectRotationActive = false;
                DirectRotationReferenceActive = true;
                DirectSlidersHeight = 22.0f;
                DirectFillerHeight = 0.0f;
                SetDirectPanelScale(0.8f);
                break;
            case DirectPanelState.Full:
                DirectPanelActive = true;
                DirectZOffsetActive = true;
                DirectPositionActive = true;
                DirectRotationActive = true;
                DirectRotationReferenceActive = false;
                DirectSlidersHeight = 64.0f;
                DirectFillerHeight = 10.0f;
                SetDirectPanelScale(0.78f);
                break;
            default: throw new ArgumentOutOfRangeException();
        }

        _directPanelState = newState;
        NotifySynchronizationRequired();
    }

    private enum DirectPanelState {
        Hidden,
        ZOffsetOnly,
        PositionOnly,
        RotationOnly,
        Full
    }

    #endregion

    #region Active

    private bool _directPanelActive;

    [UIValue("direct-panel-active")]
    [UsedImplicitly]
    private bool DirectPanelActive {
        get => _directPanelActive;
        set {
            if (_directPanelActive.Equals(value)) return;
            _directPanelActive = value;
            NotifyPropertyChanged();
        }
    }

    private bool _directZOffsetActive;

    [UIValue("direct-z-offset-active")]
    [UsedImplicitly]
    private bool DirectZOffsetActive {
        get => _directZOffsetActive;
        set {
            if (_directZOffsetActive.Equals(value)) return;
            _directZOffsetActive = value;
            NotifyPropertyChanged();
        }
    }

    private bool _directPositionActive;

    [UIValue("direct-position-active")]
    [UsedImplicitly]
    private bool DirectPositionActive {
        get => _directPositionActive;
        set {
            if (_directPositionActive.Equals(value)) return;
            _directPositionActive = value;
            NotifyPropertyChanged();
        }
    }

    private bool _directRotationActive;

    [UIValue("direct-rotation-active")]
    [UsedImplicitly]
    private bool DirectRotationActive {
        get => _directRotationActive;
        set {
            if (_directRotationActive.Equals(value)) return;
            _directRotationActive = value;
            NotifyPropertyChanged();
        }
    }

    private bool _directRotationReferenceActive;

    [UIValue("direct-rotation-reference-active")]
    [UsedImplicitly]
    private bool DirectRotationReferenceActive {
        get => _directRotationReferenceActive;
        set {
            if (_directRotationReferenceActive.Equals(value)) return;
            _directRotationReferenceActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Scale

    [UIComponent("direct-panel-component")] [UsedImplicitly]
    private RectTransform _directPanelComponent;

    private void SetDirectPanelScale(float scale) {
        _directPanelComponent.localScale = new Vector3(scale, scale, scale);
    }

    #endregion

    #region Dynamic style variables

    private float _directFillerHeight;

    [UIValue("direct-filler-height")]
    [UsedImplicitly]
    private float DirectFillerHeight {
        get => _directFillerHeight;
        set {
            if (_directFillerHeight.Equals(value)) return;
            _directFillerHeight = value;
            NotifyPropertyChanged();
        }
    }

    private float _directSlidersHeight;

    [UIValue("direct-sliders-section-height")]
    [UsedImplicitly]
    private float DirectSlidersHeight {
        get => _directSlidersHeight;
        set {
            if (_directSlidersHeight.Equals(value)) return;
            _directSlidersHeight = value;
            NotifyPropertyChanged();
        }
    }

    [UIValue("direct-sliders-section-pad")] [UsedImplicitly]
    private float _directSlidersSectionPad = UnityGame.GameVersion.ToString() switch {
        "1.18.3" => 0.0f,
        _ => 1.0f
    };

    #endregion
}