using JetBrains.Annotations;
using Zenject;

namespace EasyOffset.Installers {
    [UsedImplicitly]
    public class OnAppInstaller : Installer<OnAppInstaller> {
        public override void InstallBindings() {
            BindInputManagers();
            BindGizmosManager();
            BindBenchmarkManager();
            BindAdjustmentModeManagers();
        }

        private void BindInputManagers() {
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
            Container.BindInterfacesAndSelfTo<RotationAutoAdjustmentModeManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<RoomOffsetAdjustmentModeManager>().AsSingle();
        }
    }
}