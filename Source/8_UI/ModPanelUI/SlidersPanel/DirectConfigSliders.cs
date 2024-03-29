using System;
using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal class DirectConfigSliders : ReeUIComponentV2 {
    #region Labels

    private const string ZOffsetLabel = "ZOffset";

    private const string PosXLabel = "Pos X";
    private const string PosYLabel = "Pos Y";
    private const string PosZLabel = "Pos Z";

    private const string RotXLabel = "Rot X";
    private const string RotYLabel = "Rot Y";
    private const string RotZLabel = "Rot Z";

    private const string CurveLabel = "Curve";
    private const string BalanceLabel = "Balance";

    private const string CurveDecLabel = "outward";
    private const string CurveIncLabel = "inward";

    #endregion

    #region RangeDescriptors

    private static readonly SmoothSlider.RangeDescriptor ZOffsetRangeDescriptor = new(
        -0.2f, 0.25f, 0.001f, 3f
    );

    private static readonly SmoothSlider.RangeDescriptor XYPositionRangeDescriptor = new(
        -0.15f, 0.15f, 0.001f, 3f
    );

    private static readonly SmoothSlider.RangeDescriptor ZPositionRangeDescriptor = new(
        -0.20f, 0.15f, 0.001f, 3f
    );

    private static readonly SmoothSlider.RangeDescriptor Rotation90RangeDescriptor = new(
        -89.5f, 89.5f, 0.1f, 0.8f
    );

    private static readonly SmoothSlider.RangeDescriptor Rotation180RangeDescriptor = new(
        -180.0f, 180.0f, 0.1f, 0.8f
    );

    private static readonly SmoothSlider.RangeDescriptor PreciseRotationRangeDescriptor = new(
        -40.0f, 40.0f, 0.1f, 0.8f
    );

    #endregion

    #region AppearanceDescriptors

    private static readonly Color XColor = new Color(1.0f, 0.6f, 0.6f);
    private static readonly Color YColor = new Color(0.6f, 1.0f, 0.6f);
    private static readonly Color ZColor = new Color(0.6f, 0.6f, 1.0f);
    private static readonly Color BalanceColor = new Color(1.0f, 0.6f, 1.0f);
    private static readonly Color CurveColor = new Color(0.6f, 1.0f, 1.0f);

    private static string PositionFormatter(float value) => $"{(value * 100):F1}<size=70%> cm";

    private static string RotationFormatter(float value) => $"{value:F1}°";

    private static string LeftCurveFormatter(float value) => (value >= 0) ? $"{value:F1}°<size=70%> in" : $"{-value:F1}°<size=70%> out";
    private static string RightCurveFormatter(float value) => (value > 0) ? $"{value:F1}°<size=70%> out" : $"{-value:F1}°<size=70%> in";

    private static readonly SmoothSlider.AppearanceDescriptor ZOffsetAppearanceDescriptor = new(ZColor, PositionFormatter);

    private static readonly SmoothSlider.AppearanceDescriptor PosXAppearanceDescriptor = new(XColor, PositionFormatter);
    private static readonly SmoothSlider.AppearanceDescriptor PosYAppearanceDescriptor = new(YColor, PositionFormatter);
    private static readonly SmoothSlider.AppearanceDescriptor PosZAppearanceDescriptor = new(ZColor, PositionFormatter);

    private static readonly SmoothSlider.AppearanceDescriptor RotXAppearanceDescriptor = new(XColor, RotationFormatter);
    private static readonly SmoothSlider.AppearanceDescriptor RotYAppearanceDescriptor = new(YColor, RotationFormatter);
    private static readonly SmoothSlider.AppearanceDescriptor RotZAppearanceDescriptor = new(ZColor, RotationFormatter);

    private static readonly SmoothSlider.AppearanceDescriptor BalanceAppearanceDescriptor = new(BalanceColor, RotationFormatter);
    private static readonly SmoothSlider.AppearanceDescriptor LeftCurveAppearanceDescriptor = new(CurveColor, LeftCurveFormatter);
    private static readonly SmoothSlider.AppearanceDescriptor RightCurveAppearanceDescriptor = new(CurveColor, RightCurveFormatter);

    #endregion

    #region Components

    [UIValue("z-offset-sliders-row"), UsedImplicitly]
    private LabeledSlidersRow _zOffsetSlidersRow;

    [UIValue("pos-x-sliders-row"), UsedImplicitly]
    private LabeledSlidersRow _posXSlidersRow;

    [UIValue("pos-y-sliders-row"), UsedImplicitly]
    private LabeledSlidersRow _posYSlidersRow;

    [UIValue("pos-z-sliders-row"), UsedImplicitly]
    private LabeledSlidersRow _posZSlidersRow;

    [UIValue("rot-x-sliders-row"), UsedImplicitly]
    private LabeledSlidersRow _rotXSlidersRow;

    [UIValue("rot-y-sliders-row"), UsedImplicitly]
    private LabeledSlidersRow _rotYSlidersRow;

    [UIValue("rot-z-sliders-row"), UsedImplicitly]
    private LabeledSlidersRow _rotZSlidersRow;

    [UIValue("balance-sliders-row"), UsedImplicitly]
    private LabeledSlidersRow _balanceSlidersRow;

    [UIValue("curve-sliders-row"), UsedImplicitly]
    private LabeledSlidersRow _curveSlidersRow;

    [UIValue("curve-row-annotations"), UsedImplicitly]
    private SlidersRowAnnotations _curveRowAnnotations;

    private void Awake() {
        _zOffsetSlidersRow = Instantiate<LabeledSlidersRow>(transform);

        _posXSlidersRow = Instantiate<LabeledSlidersRow>(transform);
        _posYSlidersRow = Instantiate<LabeledSlidersRow>(transform);
        _posZSlidersRow = Instantiate<LabeledSlidersRow>(transform);

        _rotXSlidersRow = Instantiate<LabeledSlidersRow>(transform);
        _rotYSlidersRow = Instantiate<LabeledSlidersRow>(transform);
        _rotZSlidersRow = Instantiate<LabeledSlidersRow>(transform);

        _balanceSlidersRow = Instantiate<LabeledSlidersRow>(transform);
        _curveSlidersRow = Instantiate<LabeledSlidersRow>(transform);
        
        _curveRowAnnotations = Instantiate<SlidersRowAnnotations>(transform);
    }

    #endregion

    #region Initialize

    protected override void OnInitialize() {
        SetupSliders();

        PluginConfig.AdjustmentModeChangedEvent += OnAdjustmentModeChanged;
        PluginConfig.ConfigWasChangedEvent += OnConfigWasChanged;
        OnAdjustmentModeChanged(PluginConfig.AdjustmentMode);
    }

    protected override void OnDispose() {
        PluginConfig.AdjustmentModeChangedEvent -= OnAdjustmentModeChanged;
    }

    #endregion

    #region Events

    private void OnConfigWasChanged() {
        _balanceSlidersRow.leftSlider.Interactable = PluginConfig.LeftSaberHasReference;
        _balanceSlidersRow.rightSlider.Interactable = PluginConfig.RightSaberHasReference;

        _curveSlidersRow.leftSlider.Interactable = PluginConfig.LeftSaberHasReference;
        _curveSlidersRow.rightSlider.Interactable = PluginConfig.RightSaberHasReference;
        
        _curveRowAnnotations.leftSliderAnnotations.IsActive = PluginConfig.LeftSaberHasReference;
        _curveRowAnnotations.rightSliderAnnotations.IsActive = PluginConfig.RightSaberHasReference;
    }

    private void OnAdjustmentModeChanged(AdjustmentMode adjustmentMode) {
        switch (adjustmentMode) {
            case AdjustmentMode.None:
            case AdjustmentMode.SwingBenchmark:
            case AdjustmentMode.RoomOffset:
                ZOffsetActive = false;
                PositionActive = false;
                RotationActive = false;
                ReferenceActive = false;
                break;
            case AdjustmentMode.Basic:
                ZOffsetActive = true;
                PositionActive = false;
                RotationActive = false;
                ReferenceActive = false;
                break;
            case AdjustmentMode.Position:
            case AdjustmentMode.PositionAuto:
                ZOffsetActive = true;
                PositionActive = true;
                RotationActive = false;
                ReferenceActive = false;
                break;
            case AdjustmentMode.Direct:
                ZOffsetActive = true;
                PositionActive = true;
                RotationActive = true;
                ReferenceActive = false;
                break;
            case AdjustmentMode.Rotation:
            case AdjustmentMode.RotationAuto:
                ZOffsetActive = false;
                PositionActive = false;
                RotationActive = false;
                ReferenceActive = true;
                break;
            default: throw new ArgumentOutOfRangeException(nameof(adjustmentMode), adjustmentMode, null);
        }
    }

    #endregion

    #region SetupSliders

    private void SetupSliders() {
        _zOffsetSlidersRow.Setup(
            ZOffsetLabel,
            ZOffsetRangeDescriptor,
            ZOffsetAppearanceDescriptor,
            DirectAdjustmentModeManager.LeftZOffset,
            DirectAdjustmentModeManager.RightZOffset
        );

        _posXSlidersRow.Setup(
            PosXLabel,
            XYPositionRangeDescriptor,
            PosXAppearanceDescriptor,
            DirectAdjustmentModeManager.LeftPosX,
            DirectAdjustmentModeManager.RightPosX
        );

        _posYSlidersRow.Setup(
            PosYLabel,
            XYPositionRangeDescriptor,
            PosYAppearanceDescriptor,
            DirectAdjustmentModeManager.LeftPosY,
            DirectAdjustmentModeManager.RightPosY
        );

        _posZSlidersRow.Setup(
            PosZLabel,
            ZPositionRangeDescriptor,
            PosZAppearanceDescriptor,
            DirectAdjustmentModeManager.LeftPosZ,
            DirectAdjustmentModeManager.RightPosZ
        );

        _rotXSlidersRow.Setup(
            RotXLabel,
            Rotation90RangeDescriptor,
            RotXAppearanceDescriptor,
            DirectAdjustmentModeManager.LeftRotX,
            DirectAdjustmentModeManager.RightRotX
        );

        _rotYSlidersRow.Setup(
            RotYLabel,
            Rotation180RangeDescriptor,
            RotYAppearanceDescriptor,
            DirectAdjustmentModeManager.LeftRotY,
            DirectAdjustmentModeManager.RightRotY
        );

        _rotZSlidersRow.Setup(
            RotZLabel,
            Rotation180RangeDescriptor,
            RotZAppearanceDescriptor,
            DirectAdjustmentModeManager.LeftRotZ,
            DirectAdjustmentModeManager.RightRotZ
        );

        _balanceSlidersRow.Setup(
            BalanceLabel,
            PreciseRotationRangeDescriptor,
            BalanceAppearanceDescriptor,
            DirectAdjustmentModeManager.LeftBalance,
            DirectAdjustmentModeManager.RightBalance
        );

        _curveSlidersRow.Setup(
            CurveLabel,
            PreciseRotationRangeDescriptor,
            LeftCurveAppearanceDescriptor,
            RightCurveAppearanceDescriptor,
            DirectAdjustmentModeManager.LeftCurve,
            DirectAdjustmentModeManager.RightCurve
        );

        _curveRowAnnotations.Setup(CurveColor, CurveDecLabel, CurveIncLabel, true);
    }

    #endregion

    #region IsActive

    private bool _zOffsetActive;

    [UIValue("z-offset-active"), UsedImplicitly]
    private bool ZOffsetActive {
        get => _zOffsetActive;
        set {
            if (_zOffsetActive == value) return;
            _zOffsetActive = value;
            NotifyPropertyChanged();
        }
    }

    private bool _positionActive;

    [UIValue("position-active"), UsedImplicitly]
    private bool PositionActive {
        get => _positionActive;
        set {
            if (_positionActive == value) return;
            _positionActive = value;
            NotifyPropertyChanged();
        }
    }

    private bool _rotationActive;

    [UIValue("rotation-active"), UsedImplicitly]
    private bool RotationActive {
        get => _rotationActive;
        set {
            if (_rotationActive == value) return;
            _rotationActive = value;
            NotifyPropertyChanged();
        }
    }

    private bool _referenceActive;

    [UIValue("reference-active"), UsedImplicitly]
    private bool ReferenceActive {
        get => _referenceActive;
        set {
            if (_referenceActive == value) return;
            _referenceActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion
}