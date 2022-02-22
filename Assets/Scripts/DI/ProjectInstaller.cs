using Goons.Network;
using UnityEngine;
using Zenject;
using Goons.ScenesManagement;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<Prematch>().AsSingle();
        Container.BindInterfacesTo<MatchMaking>().AsSingle();
        Container.BindInterfacesTo<NetworkMatch>().AsSingle();
        Container.BindInterfacesTo<GameMessagesSystem>().AsSingle();
        Container.BindInterfacesTo<ScenesManager>().AsSingle();
        Container.BindInterfacesTo<WWWNNetworkInteractions>().AsSingle();
#if UNITY_WEBGL
        var hook = FindObjectOfType<JavascriptHook>();
        Container.Bind<JavascriptHook>().FromInstance(hook).AsSingle();
#endif
    }
}