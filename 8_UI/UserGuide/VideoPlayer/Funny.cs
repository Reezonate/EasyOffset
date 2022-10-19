using BeatSaberMarkupLanguage.Attributes;
using HMUI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EasyOffset;

internal class Funny : ReeUIComponentV2 {
    #region Initialize

    protected override void OnInitialize() {
        _imageComponent.gameObject.AddComponent<FunnyController>();
    }

    #endregion

    #region Interaction

    public void SetActive(bool value) {
        _container.gameObject.SetActive(value);
    }

    #endregion

    #region Image

    [UIComponent("container-component"), UsedImplicitly]
    private RectTransform _container;

    [UIComponent("image-component"), UsedImplicitly]
    private ImageView _imageComponent;

    #endregion

    #region FunnyController

    private class FunnyController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {
        #region Animation

        private static readonly Vector3 IdleScale = new(1.0f, 1.0f, 1.0f);
        private static readonly Vector3 PressedScale = new(2.0f, 0.7f, 1.0f);

        private bool _pressed;
        private float _press;
        private bool _hovered;
        private float _hover;

        private const float MaxScale = 4.0f;
        private const float ScalePerClick = 0.1f;
        private float _targetScale = 1.0f;
        private float _scale = 1.0f;

        private void Update() {
            _press = Mathf.Lerp(_press, _pressed ? 1.0f : 0.0f, Time.deltaTime * 20.0f);
            _hover = Mathf.Lerp(_hover, _hovered ? 1.0f : 0.0f, Time.deltaTime * 20.0f);
            _scale = Mathf.Lerp(_scale, _targetScale, Time.deltaTime * 10.0f);
            transform.localScale = Vector3.Lerp(IdleScale, PressedScale, _press) * (_scale + 0.3f * _hover);
        }

        private void OnDisable() {
            _press = _hover = 0.0f;
            _scale = _targetScale = 1.0f;
            transform.localScale = Vector3.one;
        }

        #endregion

        #region Pointerhandlers

        public void OnPointerDown(PointerEventData eventData) {
            _pressed = true;
            _targetScale += ScalePerClick;
            if (_targetScale > MaxScale) _targetScale = MaxScale;
        }

        public void OnPointerUp(PointerEventData eventData) {
            _pressed = false;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            _hovered = true;
        }

        public void OnPointerExit(PointerEventData eventData) {
            _hovered = false;
            _pressed = false;
        }

        #endregion
    }

    #endregion
}