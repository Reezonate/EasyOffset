using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EasyOffset;

internal class Signature : ReeUIComponentV2 {
    #region Initialize

    protected override void OnInitialize() {
        InitializeText();
    }

    #endregion

    #region Events

    private static void OnClick(PointerEventData _) {
        Application.OpenURL(RemoteConfig.DiscordInvite);
    }

    private void OnHoverStateChanged(bool hovered, float hoverProgress) {
        var scale = 1.0f + 0.2f * hoverProgress;
        _textComponent.transform.localScale = new Vector3(scale, scale, scale);

        var alpha = 0.6f + 0.4f * hoverProgress;
        var brightness = 0.6f * hoverProgress;
        SetColor(brightness, alpha);
    }

    #endregion

    #region Components

    [UIComponent("text-component"), UsedImplicitly]
    private ClickableText _textComponent;

    private void InitializeText() {
        var hoverController = _textComponent.gameObject.AddComponent<SmoothHoverController>();
        hoverController.HoverStateChangedEvent += OnHoverStateChanged;
        SetColor(0.0f, 0.8f);
        _textComponent.OnClickEvent = OnClick;
    }

    private void SetColor(float brightness, float alpha) {
        var color = new Color(brightness, brightness, brightness, alpha);
        _textComponent.DefaultColor = color;
        _textComponent.HighlightColor = color;
    }

    #endregion
}