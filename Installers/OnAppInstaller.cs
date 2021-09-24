using System.Reflection;
using JetBrains.Annotations;
using Zenject;

namespace EasyOffset.Installers {
    [UsedImplicitly]
    public class OnAppInstaller : Installer<OnAppInstaller> {
        private static readonly FieldInfo MainSettingsFieldInfo = typeof(PCAppInit).GetField(
            "_mainSettingsModel",
            BindingFlags.Instance | BindingFlags.NonPublic
        );

        public static void PreInstall(PCAppInit appInstaller, DiContainer container) {
            var mainSettingsModel = (MainSettingsModelSO) MainSettingsFieldInfo.GetValue(appInstaller);
            container.BindInstance(mainSettingsModel).AsSingle();
        }

        public override void InstallBindings() {
            BindInputManagers();
            BindGizmosManagers();
            BindOffsetManagers();
            Container.BindInterfacesAndSelfTo<DisplayControllerManager>().AsSingle();
        }

        private void BindInputManagers() {
            Container.BindInterfacesAndSelfTo<ReeInputManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<AbominationInputManager>().AsSingle();
        }

        private void BindGizmosManagers() {
            Container.BindInterfacesAndSelfTo<PivotGizmosManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<TrailGizmosManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<DirectionGridGizmosManager>().AsSingle();

            Container.BindExecutionOrder<PivotGizmosManager>(1);
            Container.BindExecutionOrder<TrailGizmosManager>(1);
            Container.BindExecutionOrder<DirectionGridGizmosManager>(1);
        }

        private void BindOffsetManagers() {
            Container.BindInterfacesAndSelfTo<BasicOffsetManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<PivotOnlyOffsetManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<DirectionOnlyOffsetManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<DirectionAutoOffsetManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<RoomOffsetManager>().AsSingle();
        }
    }
}