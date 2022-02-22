using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Goons.UI
{
    public class LoadingScreen : BaseWindow
    {
        [Inject]
        private CharacterBody _characterBody;

        [Inject]
        private CharacterWindow _characterWindow;

        private void Start()
        {
            //_characterBody.Initialized += Hide;
            _characterWindow.CustomizationItemsLoaded += Hide;
        }
    }
}
