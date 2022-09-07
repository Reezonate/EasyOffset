using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EasyOffset {
    public class HoverController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        public event Action<bool> HoverStateChangedEvent;

        private bool _isHovered;

        public bool IsHovered {
            get => _isHovered;
            private set {
                if (_isHovered == value) return;
                _isHovered = value;
                HoverStateChangedEvent?.Invoke(value);
            }
        }

        public void AddHoverListener(Action<bool> handler) {
            HoverStateChangedEvent += handler;
            handler?.Invoke(IsHovered);
        }

        public void RemoveHoverListener(Action<bool> handler) {
            HoverStateChangedEvent -= handler;
        }

        private void OnDisable() {
            IsHovered = false;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            IsHovered = true;
        }

        public void OnPointerExit(PointerEventData eventData) {
            IsHovered = false;
        }
    }
}