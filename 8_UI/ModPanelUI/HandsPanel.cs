using System;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;

namespace EasyOffset;

internal partial class ModPanelUI {
    #region Active

    private bool _handsPanelActive = true;

    [UIValue("hands-panel-active")]
    [UsedImplicitly]
    private bool HandsPanelActive {
        get => _handsPanelActive;
        set {
            _handsPanelActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region ZOffset sliders values

    [UIValue("zo-min")] [UsedImplicitly] private float _zOffsetSliderMin = -15f;

    [UIValue("zo-max")] [UsedImplicitly] private float _zOffsetSliderMax = 25f;

    [UIValue("zo-increment")] [UsedImplicitly]
    private float _zOffsetSliderIncrement = 1f;

    #endregion

    #region LeftHand Z Offset Slider

    [UIValue("lzo-value")]
    [UsedImplicitly]
    private float LeftZOffsetSliderValue {
        get => PluginConfig.LeftHandZOffset * 100f;
        set {
            PluginConfig.LeftHandZOffset = value / 100f;
            NotifyPropertyChanged();
        }
    }

    [UIAction("lzo-on-change")]
    [UsedImplicitly]
    private void OnLeftZOffsetValueChange(float value) {
        PluginConfig.LeftHandZOffset = value / 100f;
    }

    #endregion

    #region LeftHand actions menu

    [UIValue("lam-choices")] [UsedImplicitly]
    private List<object> _leftActionMenuChoices = HandMenuActionUtils.LeftHandMenuChoicesObjects.ToList();

    private string _leftActionMenuChoiceBackingField = HandMenuActionUtils.TypeToName(HandMenuAction.Default);

    [UIValue("lam-choice")]
    [UsedImplicitly]
    private string LeftActionMenuChoice {
        get => _leftActionMenuChoiceBackingField;
        set {
            _leftActionMenuChoiceBackingField = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("lam-on-change")]
    [UsedImplicitly]
    private void LeftActionMenuOnChange(string value) {
        OnHandAction(Hand.Left, HandMenuActionUtils.NameToType(value));
        _leftHandMenuResetAction.InvokeLater(10, ResetLeftHandMenu);
    }

    private readonly DelayedAction _leftHandMenuResetAction = new();

    private void ResetLeftHandMenu() {
        LeftActionMenuChoice = HandMenuActionUtils.TypeToName(HandMenuAction.Default);
    }

    #endregion

    #region RightHand Z Offset Slider

    [UIValue("rzo-value")]
    [UsedImplicitly]
    private float RightZOffsetSliderValue {
        get => PluginConfig.RightHandZOffset * 100f;
        set {
            PluginConfig.RightHandZOffset = value / 100f;
            NotifyPropertyChanged();
        }
    }

    [UIAction("rzo-on-change")]
    [UsedImplicitly]
    private void OnRightZOffsetValueChange(float value) {
        PluginConfig.RightHandZOffset = value / 100f;
    }

    #endregion

    #region RightHand actions menu

    [UIValue("ram-choices")] [UsedImplicitly]
    private List<object> _rightActionMenuChoices = HandMenuActionUtils.RightHandMenuChoicesObjects.ToList();

    private string _rightActionMenuChoiceBackingField = HandMenuActionUtils.TypeToName(HandMenuAction.Default);

    [UIValue("ram-choice")]
    [UsedImplicitly]
    private string RightActionMenuChoice {
        get => _rightActionMenuChoiceBackingField;
        set {
            _rightActionMenuChoiceBackingField = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("ram-on-change")]
    [UsedImplicitly]
    private void RightActionMenuOnChange(string value) {
        OnHandAction(Hand.Right, HandMenuActionUtils.NameToType(value));
        _rightHandMenuResetAction.InvokeLater(10, ResetRightHandMenu);
    }

    private readonly DelayedAction _rightHandMenuResetAction = new();

    private void ResetRightHandMenu() {
        RightActionMenuChoice = HandMenuActionUtils.TypeToName(HandMenuAction.Default);
    }

    #endregion

    #region OnHandMenuAction

    private void OnHandAction(Hand hand, HandMenuAction action) {
        switch (action) {
            case HandMenuAction.Default: return;

            case HandMenuAction.LeftMirrorAll:
                PluginConfig.MirrorPivot(hand);
                PluginConfig.MirrorSaberDirection(hand);
                PluginConfig.MirrorZOffset(hand);
                break;
            case HandMenuAction.RightMirrorAll:
                PluginConfig.MirrorPivot(hand);
                PluginConfig.MirrorSaberDirection(hand);
                PluginConfig.MirrorZOffset(hand);
                break;
            case HandMenuAction.MirrorPivot:
                PluginConfig.MirrorPivot(hand);
                break;
            case HandMenuAction.MirrorDirection:
                PluginConfig.MirrorSaberDirection(hand);
                break;
            case HandMenuAction.MirrorZOffset:
                PluginConfig.MirrorZOffset(hand);
                break;
            case HandMenuAction.Reset:
                PluginConfig.ResetOffsets(hand);
                break;
            default: throw new ArgumentOutOfRangeException();
        }

        UpdateZOffsetSliders();
    }

    private void UpdateZOffsetSliders() {
        LeftZOffsetSliderValue = PluginConfig.LeftHandZOffset * 100f;
        RightZOffsetSliderValue = PluginConfig.RightHandZOffset * 100f;
    }

    #endregion
}