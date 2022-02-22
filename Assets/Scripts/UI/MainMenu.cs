using Goons.ScenesManagement;
using Goons.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Goons.UI
{
    public class MainMenu : MonoBehaviour
    {
        [Inject]
        private IUiSystem _uiSystem;
        [Inject]
        private IScenesManager _scenesManager;

        [SerializeField]
        private Animator _animator;

        public void CollectionButtonClicked()
        {
            _uiSystem.ShowDeckBuilder();
            _animator.SetTrigger("collection");
        }

        public void HomeButtonClicked()
        {
            _uiSystem.ShowStartWindow();
            _animator.SetTrigger("home");
        }

        public void ShowCharacterWindow()
        {
            _uiSystem.ShowCharacterWindow();
            _animator.SetTrigger("character");
        }

    }
}