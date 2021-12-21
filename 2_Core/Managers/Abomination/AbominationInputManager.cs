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
            if (button != PluginConfig.AssignedButton) return;
            Abomination.OnAssignedButtonPressed(Hand.Left);
        }

        private static void OnLeftControllerButtonReleased(ControllerButton button) {
            if (button != PluginConfig.AssignedButton) return;
            Abomination.OnAssignedButtonReleased(Hand.Left);
        }

        private static void OnRightControllerButtonPressed(ControllerButton button) {
            if (button != PluginConfig.AssignedButton) return;
            Abomination.OnAssignedButtonPressed(Hand.Right);
        }

        private static void OnRightControllerButtonReleased(ControllerButton button) {
            if (button != PluginConfig.AssignedButton) return;
            Abomination.OnAssignedButtonReleased(Hand.Right);
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