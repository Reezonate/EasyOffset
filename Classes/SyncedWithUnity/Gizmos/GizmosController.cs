using UnityEngine;

namespace EasyOffset.SyncedWithUnity {
    public class GizmosController : MonoBehaviour {
        #region Serialized

        [SerializeField] private Pivot pivot;
        [SerializeField] private SphericalBasis sphericalBasis;
        [SerializeField] private OrthonormalBasis orthonormalBasis;
        [SerializeField] private ControllerModel controllerModel;
        [SerializeField] private SwingPreview swingPreview;

        #endregion

        #region Start

        public bool isVrModeOculus;

        private void Start() {
            if (!isVrModeOculus) return;
            controllerModel.SetModelOffset(Vector3.zero, Quaternion.Euler(-40f, 0f, 0f));
        }

        #endregion

        #region Visibility

        public void SetVisibility(
            bool isPivotVisible,
            bool isSphericalBasisVisible,
            bool isOrthonormalBasisVisible,
            bool isControllerModelVisible,
            bool isSwingPreviewVisible
        ) {
            pivot.SetVisible(isPivotVisible);
            sphericalBasis.SetVisible(isSphericalBasisVisible);
            orthonormalBasis.SetVisible(isOrthonormalBasisVisible);
            controllerModel.SetVisible(isControllerModelVisible);
            swingPreview.SetVisible(isSwingPreviewVisible);
        }

        #endregion

        #region Interaction

        public void SetSphericalBasisFocus(bool value) {
            sphericalBasis.SetFocus(value);
        }

        public void SetCameraPosition(Vector3 cameraWorldPosition) {
            orthonormalBasis.SetTextLookAt(cameraWorldPosition);
        }

        public void SetControllerTransform(
            Vector3 position,
            Quaternion rotation
        ) {
            var tr = transform;
            tr.position = position;
            tr.rotation = rotation;
            sphericalBasis.SetTextLookAt(position);
        }

        public void SetPivotPosition(
            Vector3 position
        ) {
            pivot.SetPosition(position);
            orthonormalBasis.SetCoordinates(position);
        }

        public void SetPreviousDirection(
            Vector3 orthoDirection,
            bool visible
        ) {
            sphericalBasis.SetPreviousDirection(orthoDirection, visible);
        }

        public void SetSaberDirection(
            Vector3 orthoDirection
        ) {
            sphericalBasis.SetDirection(orthoDirection);

            var saberRotation = TransformUtils.GetSaberLocalRotation(orthoDirection);
            swingPreview.SetSaberRotation(saberRotation);
        }

        public void Zoom(float magnitude) {
            sphericalBasis.Zoom(magnitude);
        }

        public void SetControllerType(
            ControllerType controllerType,
            Hand hand
        ) {
            controllerModel.SetControllerType(controllerType, hand);
        }

        #endregion
    }
}