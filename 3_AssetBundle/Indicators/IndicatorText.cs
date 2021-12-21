using TMPro;
using UnityEngine;

namespace EasyOffset {
    public class IndicatorText : MonoBehaviour {
        #region Serialized

        [SerializeField] private TextMeshPro textMesh;
        [SerializeField] private RectTransform textTransform;
        [SerializeField] private Transform lineTransform;
        [SerializeField] private Transform originTransform;
        [SerializeField] private float lineThickness = 1.0f;
        [SerializeField] private float lineMargin;

        #endregion

        #region Update

        private void Update() {
            var originLookDirection = originTransform.position - _lookAt;
            var textLookDirection = textTransform.position - _lookAt;

            originTransform.rotation = Quaternion.LookRotation(originLookDirection, Vector3.up);
            textTransform.rotation = Quaternion.LookRotation(textLookDirection, Vector3.up);
        }

        #endregion

        #region Interaction

        private Vector3 _lookAt = Vector3.forward;

        public void SetPosition(
            Vector3 position
        ) {
            transform.position = position;
        }

        public void SetLookAt(Vector3 lookAt) {
            _lookAt = lookAt;
        }

        public void SetText(string text) {
            textMesh.text = text;
        }

        public void SetOffset(Vector2 offset) {
            textMesh.transform.localPosition = offset;

            var offsetDistance = offset.magnitude;
            var lineDirection = -offset.normalized;
            var lineAngle = -Mathf.Atan2(lineDirection.y, lineDirection.x) * Mathf.Rad2Deg;

            lineTransform.localPosition = offset - lineDirection * lineMargin;
            lineTransform.localRotation = Quaternion.Euler(lineAngle, 90, -90);
            lineTransform.localScale = new Vector3(lineThickness, 1f, offsetDistance - lineMargin);

            if (offset.x < 0) {
                if (offset.y < 0) {
                    textTransform.pivot = new Vector2(1, 1);
                    textMesh.alignment = TextAlignmentOptions.TopRight;
                } else {
                    textTransform.pivot = new Vector2(1, 0);
                    textMesh.alignment = TextAlignmentOptions.BottomRight;
                }
            } else {
                if (offset.y < 0) {
                    textTransform.pivot = new Vector2(0, 1);
                    textMesh.alignment = TextAlignmentOptions.TopLeft;
                } else {
                    textTransform.pivot = new Vector2(0, 0);
                    textMesh.alignment = TextAlignmentOptions.BottomLeft;
                }
            }
        }

        #endregion
    }
}