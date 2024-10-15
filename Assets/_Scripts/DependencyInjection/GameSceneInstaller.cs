using XRTest.Input;
using XRTest.Signals;
using Zenject;

namespace XRTest.DependencyInjection
{
    public class GameSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<ScoreChangedSignal>().OptionalSubscriber();
            Container.DeclareSignal<MagazineInsertedSignal>().OptionalSubscriber();
            Container.DeclareSignal<MagazineRemovedSignal>().OptionalSubscriber();
            Container.DeclareSignal<AmmoCountChangedSignal>().OptionalSubscriber();

            Container.BindInterfacesAndSelfTo<InputLogger>().AsSingle();
        }
    }
}