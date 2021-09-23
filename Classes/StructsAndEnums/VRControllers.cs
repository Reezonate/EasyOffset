namespace EasyOffset {
    public readonly struct VRControllers {
        public readonly VRController RightController;
        public readonly VRController LeftController;

        public VRControllers(
            VRController rightController,
            VRController leftController
        ) {
            RightController = rightController;
            LeftController = leftController;
        }
    }
}