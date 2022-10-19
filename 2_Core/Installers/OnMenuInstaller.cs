using JetBrains.Annotations;
using Zenject;

namespace EasyOffset.Installers {
    [UsedImplicitly]
    public class OnMenuInstaller : Installer<OnMenuInstaller> {
        [Inject] [UsedImplicitly] private MenuPlayerController _menuPlayerController;

        public override void InstallBindings() {
            var vrControllers = new VRControllers(
                _menuPlayerController.rightController,
                _menuPlayerController.leftController
            );

            Container.BindInstance(vrControllers).AsSingle();
            Container.BindInterfacesAndSelfTo<AbominationTransformManager>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<UIManager>().AsSingle();

            Container.Bind<UserGuideViewController>().FromNewComponentAsViewController().AsSingle();
            Container.BindInterfacesAndSelfTo<UserGuideFloatingManager>().AsSingle();
        }
    }
}