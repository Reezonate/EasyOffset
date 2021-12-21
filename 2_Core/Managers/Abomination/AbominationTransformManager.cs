using JetBrains.Annotations;
using Zenject;

namespace EasyOffset {
    [UsedImplicitly]
    public class AbominationTransformManager : ITickable {
        private readonly VRController _leftController;
        private readonly VRController _rightController;
        private readonly IVRPlatformHelper _vrPlatformHelper;

        public AbominationTransformManager(
            VRControllers vrControllers,
            IVRPlatformHelper vrPlatformHelper
        ) {
            _leftController = vrControllers.LeftController;
            _rightController = vrControllers.RightController;
            _vrPlatformHelper = vrPlatformHelper;
        }

        public void Tick() {
            var leftUpdateResult = _vrPlatformHelper.GetNodePose(
                _leftController.node,
                _leftController.nodeIdx,
                out var leftHandPos,
                out var leftHandRot
            );

            if (!leftUpdateResult) {
                leftHandPos = Abomination.LeftControllerTransform.Position;
                leftHandRot = Abomination.LeftControllerTransform.Rotation;
            }

            var rightUpdateResult = _vrPlatformHelper.GetNodePose(
                _rightController.node,
                _rightController.nodeIdx,
                out var rightHandPos,
                out var rightHandRot
            );

            if (!rightUpdateResult) {
                rightHandPos = Abomination.RightControllerTransform.Position;
                rightHandRot = Abomination.RightControllerTransform.Rotation;
            }

            if (_vrPlatformHelper.vrPlatformSDK == VRPlatformSDK.Oculus) {
                TransformUtils.RemoveOculusModeOffsets(ref leftHandPos, ref leftHandRot);
                TransformUtils.RemoveOculusModeOffsets(ref rightHandPos, ref rightHandRot);
            }

            Abomination.UpdateTransforms(leftHandPos, leftHandRot, rightHandPos, rightHandRot);
        }
    }
}