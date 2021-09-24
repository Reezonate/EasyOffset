using EasyOffset.Configuration;
using JetBrains.Annotations;
using Zenject;

namespace EasyOffset.Installers {
    public class OnGameInstaller : Installer<OnGameInstaller> {
        [Inject] [UsedImplicitly] private SaberManager _saberManager;

        public override void InstallBindings() {
            Container.BindInterfacesAndSelfTo<MidPlayAdjustmentsManager>().AsSingle();
            if (!PluginConfig.EnableMidPlayAdjustment) return;

            var vrControllers = new VRControllers(
                _saberManager.rightSaber.GetComponentInParent<VRController>(),
                _saberManager.leftSaber.GetComponentInParent<VRController>()
            );

            Container.BindInstance(vrControllers).AsSingle();
            Container.BindInterfacesAndSelfTo<AbominationTransformManager>().AsSingle();
        }
    }
}