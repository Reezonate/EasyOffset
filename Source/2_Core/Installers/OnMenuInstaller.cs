using JetBrains.Annotations;
using Zenject;

namespace EasyOffset.Installers {
    [UsedImplicitly]
    public class OnMenuInstaller : Installer<OnMenuInstaller> {
        [Inject] [UsedImplicitly] private MenuPlayerController _menuPlayerController;

        public override void InstallBindings() {
            BindInputManager();
            BindGizmosManager();
            BindBenchmarkManager();
            BindAdjustmentModeManagers();
            
            BindAbomination();
            BindUI();
        }

        private void BindAbomination() {
            var vrControllers = new VRControllers(
                _menuPlayerController.rightController,
                _menuPlayerController.leftController
            );

            Container.BindInstance(vrControllers).AsSingle();
            Container.BindInterfacesAndSelfTo<AbominationTransformManager>().AsSingle();
        }

        private void BindUI() {
            Container.BindInterfacesAndSelfTo<UIManager>().AsSingle();
            Container.Bind<UserGuideViewController>().FromNewComponentAsViewController().AsSingle();
            Container.BindInterfacesAndSelfTo<UserGuideFloatingManager>().AsSingle();
        }

        private void BindInputManager() {
            Container.BindInterfacesAndSelfTo<ReeInputManager>().AsSingle();
        }

        private void BindGizmosManager() {
            Container.BindInterfacesAndSelfTo<GizmosManager>().AsSingle();
            Container.BindExecutionOrder<GizmosManager>(1);
        }

        private void BindBenchmarkManager() {
            Container.BindInterfacesAndSelfTo<SwingBenchmarkManager>().AsSingle();
            Container.BindExecutionOrder<SwingBenchmarkManager>(1);
        }

        private void BindAdjustmentModeManagers() {
            Container.BindInterfacesAndSelfTo<AdjustmentBlockManager>().AsSingle();

            Container.BindInterfacesAndSelfTo<BasicAdjustmentModeManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<PositionAdjustmentModeManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<RotationAdjustmentModeManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<SwingBenchmarkAdjustmentModeManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<PositionAutoAdjustmentModeManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<RotationAutoAdjustmentModeManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<RoomOffsetAdjustmentModeManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<DirectAdjustmentModeManager>().AsSingle();
        }
    }
}