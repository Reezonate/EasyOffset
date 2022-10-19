using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal class SwingBenchmarkPanel : ReeUIComponentV2 {
    #region Initialize

    protected override void OnInitialize() {
        PluginConfig.AdjustmentModeChangedEvent += OnAdjustmentModeChanged;
        SwingBenchmarkHelper.OnResetEvent += OnBenchmarkReset;
        SwingBenchmarkHelper.OnStartEvent += OnBenchmarkStart;
        SwingBenchmarkHelper.OnUpdateEvent += OnBenchmarkUpdate;
        SwingBenchmarkHelper.OnFailEvent += OnBenchmarkFail;
        SwingBenchmarkHelper.OnSuccessEvent += OnBenchmarkSuccess;
    }

    #endregion

    #region Events

    private void OnAdjustmentModeChanged(AdjustmentMode adjustmentMode) {
        BenchmarkPanelActive = adjustmentMode == AdjustmentMode.SwingBenchmark;
    }

    private void OnBenchmarkReset() {
        UpdateBenchmarkPanel(false);
    }

    private void OnBenchmarkStart() {
        UpdateBenchmarkPanel(false);
    }

    private void OnBenchmarkFail() {
        UpdateBenchmarkPanel(false);
    }

    private void OnBenchmarkSuccess() {
        UpdateBenchmarkPanel(true);
    }

    private void OnBenchmarkUpdate(
        float swingCurveAngle,
        float tipDeviation,
        float pivotDeviation,
        float minimalSwingAngle,
        float maximalSwingAngle
    ) {
        BenchmarkCurveText = $"{swingCurveAngle * Mathf.Rad2Deg:F1}° {(swingCurveAngle < 0 ? "Inward" : "Outward")}";
        BenchmarkTipWobbleText = $"{(tipDeviation * 200):F1} cm";
        BenchmarkArmUsageText = $"{(pivotDeviation * 200):F1} cm";

        var min = minimalSwingAngle * Mathf.Rad2Deg;
        var max = maximalSwingAngle * Mathf.Rad2Deg;
        var full = max - min;
        BenchmarkAngleText = $"{full:F1}° ({max:F1}°/{-min:F1}°)";
    }

    private void UpdateBenchmarkPanel(bool hasResults) {
        BenchmarkGuideActive = !hasResults;
        BenchmarkResultsActive = hasResults;
    }

    #endregion

    #region Active

    private bool _benchmarkPanelActive;

    [UIValue("benchmark-panel-active")]
    [UsedImplicitly]
    private bool BenchmarkPanelActive {
        get => _benchmarkPanelActive;
        set {
            _benchmarkPanelActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Guide

    #region Active

    private bool _benchmarkGuideActive = true;

    [UIValue("benchmark-guide-active")]
    [UsedImplicitly]
    private bool BenchmarkGuideActive {
        get => _benchmarkGuideActive;
        set {
            _benchmarkGuideActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Instructions

    [UIValue("benchmark-guide")] [UsedImplicitly]
    private string _benchmarkGuide = "Repeat one exact swing several times" +
                                     "\nwhile holding the button" +
                                     "\n" +
                                     "\nAt least 140° swing angle is required";

    #endregion

    #endregion

    #region Results

    #region Active

    private bool _benchmarkResultsActive;

    [UIValue("benchmark-results-active")]
    [UsedImplicitly]
    private bool BenchmarkResultsActive {
        get => _benchmarkResultsActive;
        set {
            _benchmarkResultsActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Curve

    [UIValue("benchmark-curve-hint")] [UsedImplicitly]
    private string _benchmarkCurveHint = "Deviation from the straight swing" +
                                         "\n" +
                                         "\nDepends only on your config";

    private string _benchmarkCurveText = "0,0° Inward";

    [UIValue("benchmark-curve-text")]
    [UsedImplicitly]
    private string BenchmarkCurveText {
        get => _benchmarkCurveText;
        set {
            _benchmarkCurveText = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region TipWobble

    [UIValue("benchmark-tip-wobble-hint")] [UsedImplicitly]
    private string _benchmarkTipWobbleHint = "Aim deviation" +
                                             "\n" +
                                             "\nDepends on your grip and skill";

    private string _benchmarkTipWobbleText = "0,0 cm";

    [UIValue("benchmark-tip-wobble-text")]
    [UsedImplicitly]
    private string BenchmarkTipWobbleText {
        get => _benchmarkTipWobbleText;
        set {
            _benchmarkTipWobbleText = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region ArmUsage

    [UIValue("benchmark-arm-usage-hint")] [UsedImplicitly]
    private string _benchmarkArmUsageHint = "Arm movement amount" +
                                            "\n" +
                                            "\nPersonal preference";

    private string _benchmarkArmUsageText = "0,0 cm";

    [UIValue("benchmark-arm-usage-text")]
    [UsedImplicitly]
    private string BenchmarkArmUsageText {
        get => _benchmarkArmUsageText;
        set {
            _benchmarkArmUsageText = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Angle

    [UIValue("benchmark-angle-hint")] [UsedImplicitly]
    private string _benchmarkAmplitudeHint = "Full swing angle (Backhand/Forehand)";

    private string _benchmarkAngleText = "150° (70°/80°)";

    [UIValue("benchmark-angle-text")]
    [UsedImplicitly]
    private string BenchmarkAngleText {
        get => _benchmarkAngleText;
        set {
            _benchmarkAngleText = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #endregion

    #region AutoFixButton

    [UIAction("benchmark-auto-fix-on-click")]
    [UsedImplicitly]
    private void BenchmarkAutoFixOnClick() {
        SwingBenchmarkHelper.InvokeAutoFix();
        SetBenchmarkStatusText("Done!");
    }

    #endregion

    #region SetAsReference button

    [UIAction("benchmark-set-as-reference-on-click")]
    [UsedImplicitly]
    private void BenchmarkSetAsReferenceOnClick() {
        SwingBenchmarkHelper.InvokeSetAsReference();
        SetBenchmarkStatusText("Done!");
    }

    #endregion

    #region ResetButton

    [UIAction("benchmark-reset-on-click")]
    [UsedImplicitly]
    private void BenchmarkResetOnClick() {
        SwingBenchmarkHelper.InvokeReset();
    }

    #endregion

    #region StatusText

    private string _benchmarkStatusText = "";

    [UIValue("benchmark-status-text")]
    [UsedImplicitly]
    private string BenchmarkStatusText {
        get => _benchmarkStatusText;
        set {
            _benchmarkStatusText = value;
            NotifyPropertyChanged();
        }
    }

    private void SetBenchmarkStatusText(string value) {
        BenchmarkStatusText = value;
        this.ReInvokeWithDelay(ref _benchmarkStatusResetCoroutine, ResetBenchmarkStatusText, 2.0f);
    }

    private Coroutine _benchmarkStatusResetCoroutine;

    private void ResetBenchmarkStatusText() {
        BenchmarkStatusText = "";
    }

    #endregion
}