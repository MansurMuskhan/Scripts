using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCardListItem : MonoBehaviour
{
    public event Action<int> Clicked;

    private int _cardId;

    public void Init(int cardId)
    {
        _cardId = cardId;
    }
    

    public void Click()
    {
        Clicked?.Invoke(_cardId);
    }
}
