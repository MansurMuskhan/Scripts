using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class CharacterCustomizationItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject _selectedMark;
    public event Action<CharacterCustomizationItem> Clicked;
    public int Id { get; private set; }
    public void Init(int id)
    {
        Id = id;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Clicked?.Invoke(this);
    }

    public void SetImage(Sprite sprite)
    {
        var img = GetComponent<Image>();
        img.sprite = sprite;
    }

    public void SetSelected(bool isSelected)
    {
        _selectedMark.SetActive(isSelected);
    }

    public class Pool : MemoryPool<CharacterCustomizationItem>
    {

    }
}
