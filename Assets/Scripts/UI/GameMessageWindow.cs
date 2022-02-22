using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameMessageWindow : MonoBehaviour
{
    private Canvas _canvas;
    [SerializeField]
    private TMP_Text _text;
    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }
    public void Show(string message)
    {
        _text.text = message;
        _canvas.enabled = true;
    }

    public void ButtonOkClicked()
    {
        _canvas.enabled = false;
    }
}
