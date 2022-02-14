using JetBrains.Annotations;
using Zenject;

namespace EasyOffset {
    [UsedImplicitly]
    public class ReeInputManager : ITickable {
        private readonly ReeInputDevice _leftReeInputDevice;
        private readonly ReeInputDevice _rightReeInputDevice;

        public ReeInputManager() {
            _leftReeInputDevice = new ReeInputDevice(Hand.Left);
            _rightReeInputDevice = new ReeInputDevice(Hand.Right);
        }

        public void Tick() {
            _leftReeInputDevice.Update();
            _rightReeInputDevice.Update();
        }
    }
}