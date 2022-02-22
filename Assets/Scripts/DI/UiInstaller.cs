using Goons.UI;
using UnityEngine;
using Zenject;

public class UiInstaller : MonoInstaller
{
    [SerializeField]
    private CardUI _cardUIPrefab;
    [SerializeField]
    private DeckButton _deckButtonPrefab;
    [SerializeField]
    private CardPlaceholder _cardPlaceholderPrefab;
    [SerializeField]
    private DeckCardPlaceholder _deckCardPlaceholderPrefab;
    [SerializeField]
    private StartWindow _startWindow;
    [SerializeField]
    private MainMenu _mainMenu;
    [SerializeField]
    private CharacterWindow _characterWindow;
    [SerializeField]
    private DeckWindow _deckWindow;
    [SerializeField]
    private CharacterBody _characterBody;
    [SerializeField]
    private LoadingScreen _loadingScreen;

    [SerializeField]
    private CharacterCustomizationItem _characterCustomizationItemPrefab;

    public override void InstallBindings()
    {
        Container.BindInterfacesTo<UiSystem>().FromInstance(FindObjectOfType<UiSystem>());
        Container.BindMemoryPool<CardUI, CardUI.Pool>().FromComponentInNewPrefab(_cardUIPrefab);
        Container.BindMemoryPool<DeckButton, DeckButton.Pool>().FromComponentInNewPrefab(_deckButtonPrefab);
        Container.BindFactory<CardPlaceholder, CardPlaceholder.Factory>().FromComponentInNewPrefab(_cardPlaceholderPrefab);
        Container.BindMemoryPool<DeckCardPlaceholder, DeckCardPlaceholder.Pool>().FromComponentInNewPrefab(_deckCardPlaceholderPrefab);
        Container.BindMemoryPool<CharacterCustomizationItem, CharacterCustomizationItem.Pool>().FromComponentInNewPrefab(_characterCustomizationItemPrefab);
        Container.BindInterfacesAndSelfTo<StartWindow>().FromInstance(_startWindow).AsSingle();
        Container.BindInterfacesAndSelfTo<MainMenu>().FromInstance(_mainMenu).AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterWindow>().FromInstance(_characterWindow).AsSingle();
        Container.BindInterfacesAndSelfTo<DeckWindow>().FromInstance(_deckWindow).AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterBody>().FromInstance(_characterBody).AsSingle();
        Container.BindInterfacesAndSelfTo<LoadingScreen>().FromInstance(_loadingScreen).AsSingle();
    }
}