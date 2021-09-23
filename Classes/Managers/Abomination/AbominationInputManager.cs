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

        private static void OnLeftGripPressed() {
            Abomination.OnGripButtonPressed(Hand.Left);
        }

        private static void OnLeftGripReleased() {
            Abomination.OnGripButtonReleased(Hand.Left);
        }

        private static void OnRightGripPressed() {
            Abomination.OnGripButtonPressed(Hand.Right);
        }

        private static void OnRightGripReleased() {
            Abomination.OnGripButtonReleased(Hand.Right);
        }

        public void Initialize() {
            _inputManager.LeftHand.OnGripButtonPressed += OnLeftGripPressed;
            _inputManager.LeftHand.OnGripButtonReleased += OnLeftGripReleased;

            _inputManager.RightHand.OnGripButtonPressed += OnRightGripPressed;
            _inputManager.RightHand.OnGripButtonReleased += OnRightGripReleased;
        }

        public void Dispose() {
            _inputManager.LeftHand.OnGripButtonPressed -= OnLeftGripPressed;
            _inputManager.LeftHand.OnGripButtonReleased -= OnLeftGripReleased;

            _inputManager.RightHand.OnGripButtonPressed -= OnRightGripPressed;
            _inputManager.RightHand.OnGripButtonReleased -= OnRightGripReleased;
        }
    }
}