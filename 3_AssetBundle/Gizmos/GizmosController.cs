using UnityEngine;

namespace EasyOffset {
    public class GizmosController : MonoBehaviour {
        #region Serialized

        [SerializeField] private Transform controllerTransform;
        [SerializeField] private Transform saberTransform;
        [SerializeField] private Transform pivotTransform;

        [SerializeField] private Pivot pivot;
        [SerializeField] private SphericalBasis sphericalBasis;
        [SerializeField] private OrthonormalBasis orthonormalBasis;
        [SerializeField] private ControllerModel controllerModel;
        [SerializeField] private SwingPreview swingPreview;
        [SerializeField] private PreciseGimbal preciseGimbal;

        #endregion

        #region SetVisibility

        public void SetVisibility(
            bool isPivotVisible,
            bool isSphericalBasisVisible,
            bool isOrthonormalBasisVisible,
            bool isOrthonormalBasisPointerVisible,
            bool isControllerModelVisible,
            bool isSwingPreviewVisible,
            bool isPreciseGimbalVisible
        ) {
            pivot.SetVisible(isPivotVisible);
            sphericalBasis.SetVisible(isSphericalBasisVisible);
            orthonormalBasis.SetVisible(isOrthonormalBasisVisible, isOrthonormalBasisPointerVisible);
            controllerModel.SetVisible(isControllerModelVisible);
            swingPreview.SetVisible(isSwingPreviewVisible);
            preciseGimbal.SetVisible(isPreciseGimbalVisible);
        }

        #endregion

        #region SetConfigValues

        public void SetConfigValues(
            Vector3 pivotPosition,
            Quaternion saberRotation,
            float zOffset
        ) {
            var saberTranslation = pivotPosition + saberRotation * new Vector3(0, 0, zOffset);
            
            saberTransform.localPosition = saberTranslation;
            saberTransform.localRotation = saberRotation;
            pivotTransform.localPosition = new Vector3(0, 0, -zOffset);

            sphericalBasis.SetRotation(saberRotation);
            sphericalBasis.SetPivotPosition(pivotPosition);
            orthonormalBasis.SetCoordinates(pivotPosition);

            var rotationEuler = saberRotation.eulerAngles;
            preciseGimbal.SetValues(
                pivotPosition,
                rotationEuler.x,
                rotationEuler.y
            );
        }

        #endregion

        #region SetControllerTransform

        public void SetControllerTransform(Vector3 position, Quaternion rotation) {
            controllerTransform.SetPositionAndRotation(position, rotation);
            sphericalBasis.SetTextLookAt(position);
        }

        #endregion

        #region SetFocus

        public void SetOrthonormalBasisFocus(bool value) {
            orthonormalBasis.SetFocus(value);
        }

        public void SetSphericalBasisFocus(bool value) {
            sphericalBasis.SetFocus(value);
        }

        #endregion

        #region SetCameraPosition

        public void SetCameraPosition(Vector3 cameraWorldPosition) {
            orthonormalBasis.SetTextLookAt(cameraWorldPosition);
            swingPreview.SetLookAt(cameraWorldPosition);
            pivot.SetLookAt(cameraWorldPosition);
        }

        #endregion

        #region SetWristValues

        public void SetWristValues(Vector3 rotationAxis, bool visible) {
            sphericalBasis.SetWristValues(rotationAxis, visible);
        }

        #endregion

        #region SetPreviousRotation

        public void SetPreviousRotation(Quaternion previousRotation, bool visible) {
            sphericalBasis.SetPreviousRotation(previousRotation, visible);
        }

        #endregion

        #region Zoom

        public void Zoom(float magnitude) {
            sphericalBasis.Zoom(magnitude);
        }

        #endregion

        #region SetControllerType

        public void SetControllerType(ControllerType controllerType, Hand hand) {
            controllerModel.SetControllerType(controllerType, hand);
        }

        #endregion
    }
}