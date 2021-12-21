using JetBrains.Annotations;
using UnityEngine.XR;
using Zenject;

namespace EasyOffset {
    [UsedImplicitly]
    public class ReeInputManager : ITickable {
        public readonly ReeInputDevice LeftReeInput;
        public readonly ReeInputDevice RightReeInput;

        public ReeInputManager() {
            LeftReeInput = new ReeInputDevice(XRNode.LeftHand);
            RightReeInput = new ReeInputDevice(XRNode.RightHand);
        }

        public void Tick() {
            LeftReeInput.Update();
            RightReeInput.Update();
        }
    }
}