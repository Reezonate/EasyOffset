using JetBrains.Annotations;
using Zenject;

namespace EasyOffset.Installers {
    [UsedImplicitly]
    public class OnAppInstaller : Installer<OnAppInstaller> {
        public override void InstallBindings() {
            Container.Bind<RemoteConfig>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        }
    }
}