using Goons.Debug;
using Goons.Model;
using Goons.View;
using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField]
    private GameView _gameView;

    public override void InstallBindings()
    {
        Container.Bind<GameView>().FromInstance(_gameView).AsSingle() ;

        Container.BindInterfacesTo<GameModel>().AsSingle();

        Container.BindInterfacesTo<CheatSystem>().AsSingle().NonLazy();
    }
}