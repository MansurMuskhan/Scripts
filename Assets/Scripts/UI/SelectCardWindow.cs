using Goons.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SelectCardWindow : MonoBehaviour
{
    [SerializeField]
    private SelectCardListItem _itemPrefab;
    [SerializeField]
    private Transform _contentRoot;

    private Canvas _canvas;

    [Inject]
    private IGameModel _gameModel;

    private List<SelectCardListItem> _spawnedItems = new List<SelectCardListItem>();


    public void Show(List<CardModel> cards)
    {
        _canvas.enabled = true;
        foreach(var card in cards)
        {
            var item = Instantiate(_itemPrefab);
            _spawnedItems.Add(item);
            item.transform.SetParent(_contentRoot.transform);
            item.transform.localScale = Vector3.one;
            item.Init(card.CardId);
            item.Clicked += ItemClicked;
        }
    }

    private void ItemClicked(int cardId)
    {
        _gameModel.MoveFromGraveyardToHand(cardId);
        Hide();
    }

    public void Hide()
    {
        foreach(var spawnedItem in _spawnedItems)
        {
            GameObject.Destroy(spawnedItem);
            spawnedItem.Clicked -= ItemClicked;
        }
        _spawnedItems.Clear();
    }

    void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

}
