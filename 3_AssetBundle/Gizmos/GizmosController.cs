using UnityEngine;

namespace EasyOffset {
    public class GizmosController : MonoBehaviour {
        #region Serialized

        [SerializeField] private Pivot pivot;
        [SerializeField] private SphericalBasis sphericalBasis;
        [SerializeField] private OrthonormalBasis orthonormalBasis;
        [SerializeField] private ControllerModel controllerModel;
        [SerializeField] private SwingPreview swingPreview;

        #endregion

        #region Visibility

        public void SetVisibility(
            bool isPivotVisible,
            bool isSphericalBasisVisible,
            bool isOrthonormalBasisVisible,
            bool isOrthonormalBasisPointerVisible,
            bool isControllerModelVisible,
            bool isSwingPreviewVisible,
            bool isCoordinatesVisible
        ) {
            pivot.SetVisible(isPivotVisible);
            sphericalBasis.SetVisible(isSphericalBasisVisible, isCoordinatesVisible);
            orthonormalBasis.SetVisible(isOrthonormalBasisVisible, isOrthonormalBasisPointerVisible, isCoordinatesVisible);
            controllerModel.SetVisible(isControllerModelVisible);
            swingPreview.SetVisible(isSwingPreviewVisible);
        }

        #endregion

        #region Interaction

        public void SetOrthonormalBasisFocus(bool value) {
            orthonormalBasis.SetFocus(value);
        }

        public void SetSphericalBasisFocus(bool value) {
            sphericalBasis.SetFocus(value);
        }

        public void SetCameraPosition(Vector3 cameraWorldPosition) {
            orthonormalBasis.SetTextLookAt(cameraWorldPosition);
            swingPreview.SetLookAt(cameraWorldPosition);
            pivot.SetLookAt(cameraWorldPosition);
        }

        public void SetWristValues(Vector3 rotationAxis, bool visible) {
            sphericalBasis.SetWristValues(rotationAxis, visible);
        }

        public void SetControllerTransform(Vector3 position, Quaternion rotation) {
            var tr = transform;
            tr.position = position;
            tr.rotation = rotation;
            sphericalBasis.SetTextLookAt(position);
        }

        public void SetPivotPosition(Vector3 position) {
            pivot.SetPosition(position);
            orthonormalBasis.SetCoordinates(position);
        }

        public void SetPreviousDirection(Vector3 orthoDirection, bool visible) {
            sphericalBasis.SetPreviousDirection(orthoDirection, visible);
        }

        public void SetSaberDirection(Vector3 orthoDirection) {
            sphericalBasis.SetDirection(orthoDirection);

            var saberRotation = TransformUtils.GetSaberLocalRotation(orthoDirection);
            swingPreview.SetSaberRotation(saberRotation);
        }

        public void Zoom(float magnitude) {
            sphericalBasis.Zoom(magnitude);
        }

        public void SetControllerType(ControllerType controllerType, Hand hand) {
            controllerModel.SetControllerType(controllerType, hand);
        }

        #endregion
    }
}