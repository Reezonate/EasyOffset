using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

internal class SliderAnnotations : ReeUIComponentV2 {
    #region Setup

    public void Setup(Color color, string leftLabel, string rightLabel) {
        Color = $"#{ColorUtility.ToHtmlStringRGB(color)}";
        LeftText = $"← {leftLabel}";
        RightText = $"{rightLabel} →";
    }

    #endregion

    #region IsActive

    private bool _isActive = true;

    [UIValue("is-active"), UsedImplicitly]
    public bool IsActive {
        get => _isActive;
        set {
            if (_isActive.Equals(value)) return;
            _isActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Color

    private string _color = "#ffffff";

    [UIValue("color"), UsedImplicitly]
    private string Color {
        get => _color;
        set {
            if (_color.Equals(value)) return;
            _color = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region LeftText

    private string _leftText = "";

    [UIValue("left-text"), UsedImplicitly]
    private string LeftText {
        get => _leftText;
        set {
            if (_leftText.Equals(value)) return;
            _leftText = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region RightText

    private string _rightText = "";

    [UIValue("right-text"), UsedImplicitly]
    private string RightText {
        get => _rightText;
        set {
            if (_rightText.Equals(value)) return;
            _rightText = value;
            NotifyPropertyChanged();
        }
    }

    #endregion
}