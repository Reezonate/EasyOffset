using System.Reflection;
using EasyOffset.Configuration;
using JetBrains.Annotations;
using Zenject;

namespace EasyOffset.Installers {
    [UsedImplicitly]
    public class OnAppInstaller : Installer<OnAppInstaller> {
        #region PreInstall

        private static readonly FieldInfo MainSettingsFieldInfo = typeof(PCAppInit).GetField(
            "_mainSettingsModel",
            BindingFlags.Instance | BindingFlags.NonPublic
        );

        public static void PreInstall(PCAppInit appInstaller) {
            PluginConfig.MainSettingsModel = (MainSettingsModelSO)MainSettingsFieldInfo.GetValue(appInstaller);
        }

        #endregion

        #region InstallBindings

        public override void InstallBindings() {
            PluginConfig.VRPlatformHelper = Container.TryResolve<IVRPlatformHelper>();

            BindInputManagers();
            BindGizmosManager();
            BindBenchmarkManager();
            BindOffsetManagers();
        }

        private void BindInputManagers() {
            Container.BindInterfacesAndSelfTo<ReeInputManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<AbominationInputManager>().AsSingle();
        }

        private void BindGizmosManager() {
            Container.BindInterfacesAndSelfTo<GizmosManager>().AsSingle();
            Container.BindExecutionOrder<GizmosManager>(1);
        }

        private void BindBenchmarkManager() {
            Container.BindInterfacesAndSelfTo<SwingBenchmarkManager>().AsSingle();
            Container.BindExecutionOrder<SwingBenchmarkManager>(1);
        }

        private void BindOffsetManagers() {
            Container.BindInterfacesAndSelfTo<BasicAdjustmentModeManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<PositionAdjustmentModeManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<RotationAdjustmentModeManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<SwingBenchmarkAdjustmentModeManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<RotationAutoAdjustmentModeManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<RoomOffsetAdjustmentModeManager>().AsSingle();
        }

        #endregion
    }
}