using System;
using UnityEngine;
namespace Goons.UI
{
    public class BaseWindow : MonoBehaviour, IHidableWindow
    {
        private Canvas _canvas;

        public bool IsViwible => _canvas.enabled;
        protected virtual void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }
        public virtual void Hide()
        {
            _canvas.enabled = false;
            VisibilityChanged?.Invoke();
        }
        public virtual void Show()
        {
            _canvas.enabled = true;
            VisibilityChanged?.Invoke();
        }

        public event Action VisibilityChanged;
    }
}