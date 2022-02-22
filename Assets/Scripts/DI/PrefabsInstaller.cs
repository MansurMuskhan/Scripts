using Goons.View;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PrefabsInstaller : MonoInstaller
{
    [SerializeField]
    private CardView _cardPrefab;



    public override void InstallBindings()
    {
        Container.BindMemoryPool<CardView, CardView.Pool>().FromComponentInNewPrefab(_cardPrefab);

    }
}