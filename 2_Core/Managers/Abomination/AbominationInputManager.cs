using System;
using JetBrains.Annotations;
using Zenject;

namespace EasyOffset {
    [UsedImplicitly]
    public class AbominationInputManager : IInitializable, IDisposable {
        private readonly ReeInputManager _inputManager;

        public AbominationInputManager(
            ReeInputManager inputManager
        ) {
            _inputManager = inputManager;
        }

        private static void OnLeftControllerButtonPressed(ControllerButton button) {
            Abomination.OnButtonPressed(Hand.Left, button);
        }

        private static void OnLeftControllerButtonReleased(ControllerButton button) {
            Abomination.OnButtonReleased(Hand.Left, button);
        }

        private static void OnRightControllerButtonPressed(ControllerButton button) {
            Abomination.OnButtonPressed(Hand.Right, button);
        }

        private static void OnRightControllerButtonReleased(ControllerButton button) {
            Abomination.OnButtonReleased(Hand.Right, button);
        }

        public void Initialize() {
            _inputManager.LeftReeInput.OnButtonPressed += OnLeftControllerButtonPressed;
            _inputManager.LeftReeInput.OnButtonReleased += OnLeftControllerButtonReleased;

            _inputManager.RightReeInput.OnButtonPressed += OnRightControllerButtonPressed;
            _inputManager.RightReeInput.OnButtonReleased += OnRightControllerButtonReleased;
        }

        public void Dispose() {
            _inputManager.LeftReeInput.OnButtonPressed -= OnLeftControllerButtonPressed;
            _inputManager.LeftReeInput.OnButtonReleased -= OnLeftControllerButtonReleased;

            _inputManager.RightReeInput.OnButtonPressed -= OnRightControllerButtonPressed;
            _inputManager.RightReeInput.OnButtonReleased -= OnRightControllerButtonReleased;
        }
    }
}