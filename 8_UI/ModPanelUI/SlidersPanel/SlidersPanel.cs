using System;
using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal class SlidersPanel : ReeUIComponentV2 {
    #region Components

    [UIValue("reset-and-mirror-controls"), UsedImplicitly]
    private ResetAndMirrorControls _resetAndMirrorControls;

    [UIValue("direct-config-sliders"), UsedImplicitly]
    private DirectConfigSliders _directConfigSliders;

    [UIValue("reference-controls"), UsedImplicitly]
    private ReferenceControls _referenceControls;

    private void Awake() {
        _resetAndMirrorControls = Instantiate<ResetAndMirrorControls>(transform);
        _directConfigSliders = Instantiate<DirectConfigSliders>(transform);
        _referenceControls = Instantiate<ReferenceControls>(transform);
    }

    #endregion

    #region Initialize & Dispose

    protected override void OnInitialize() {
        PluginConfig.AdjustmentModeChangedEvent += OnAdjustmentModeChanged;
        OnAdjustmentModeChanged(PluginConfig.AdjustmentMode);
    }

    protected override void OnDispose() {
        PluginConfig.AdjustmentModeChangedEvent -= OnAdjustmentModeChanged;
    }

    #endregion

    #region Events

    private void OnAdjustmentModeChanged(AdjustmentMode adjustmentMode) {
        switch (adjustmentMode) {
            case AdjustmentMode.Basic:
            case AdjustmentMode.Position:
            case AdjustmentMode.Rotation:
            case AdjustmentMode.PositionAuto:
            case AdjustmentMode.RotationAuto:
                IsActive = true;
                SetScaleAndOffset(BigScale, BigOffset);
                break;
            case AdjustmentMode.Direct:
                IsActive = true;
                SetScaleAndOffset(SmallScale, SmallOffset);
                break;
            case AdjustmentMode.None:
            case AdjustmentMode.SwingBenchmark:
            case AdjustmentMode.RoomOffset:
                IsActive = false;
                break;
            default: throw new ArgumentOutOfRangeException(nameof(adjustmentMode), adjustmentMode, null);
        }
    }

    #endregion

    #region Scale & Offset

    private static Vector3 BigScale => new(0.95f, 0.95f, 0.95f);
    private static Vector3 BigOffset => new(0.0f, 0.0f, 0.0f);

    private static Vector3 SmallScale => new(0.84f, 0.84f, 0.84f);
    private static Vector3 SmallOffset => new(0.0f, 3.8f, 0.0f);


    [UIComponent("container"), UsedImplicitly]
    private RectTransform _container;

    private void SetScaleAndOffset(Vector3 scale, Vector3 offset) {
        _container.localScale = scale;
        _container.localPosition = offset;
    }

    #endregion

    #region IsActive

    private bool _isActive;

    [UIValue("is-active"), UsedImplicitly]
    private bool IsActive {
        get => _isActive;
        set {
            if (_isActive.Equals(value)) return;
            _isActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion
}