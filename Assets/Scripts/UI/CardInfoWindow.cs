using Goons.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Goons.UI
{
    public class CardInfoWindow : MonoBehaviour, IHidableWindow
    {
        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();

        }

        public void Show(CardModel cardModel)
        {
            _canvas.enabled = true;
        }

        public void Hide()
        {
            _canvas.enabled = false;
        }

        public void ButtonCloseClicked()
        {
            Hide();
        }
    }
}