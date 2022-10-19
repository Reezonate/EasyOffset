using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace EasyOffset;

internal class DownloadProgress : ReeUIComponentV2 {
    #region Components

    [UIComponent("container-component"), UsedImplicitly]
    private RectTransform _container;

    [UIComponent("text-component"), UsedImplicitly]
    private TextMeshProUGUI _textComponent;

    private void UpdateText() {
        _textComponent.text = $"{Label} {Progress * 100.0f:F0}<size=60%>%";
    }

    #endregion

    #region Properties

    private string _label = string.Empty;

    public string Label {
        get => _label;
        set {
            _label = value;
            UpdateText();
        }
    }

    private float _progress;

    public float Progress {
        get => _progress;
        set {
            _progress = value;
            UpdateText();
        }
    }

    #endregion

    #region Interaction

    public void SetActive(bool value) {
        _container.gameObject.SetActive(value);
    }

    #endregion
}