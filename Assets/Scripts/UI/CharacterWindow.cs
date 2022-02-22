using Goons.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Goons.UI
{
    public class CharacterWindow : BaseWindow
    {
        [Inject]
        private CharacterCustomizationItem.Pool _characterCustomizationItemsPool;
        [Inject]
        private IPrematch _prematch;

        [Inject]
        private ICustomizationImagesConfig _customizationImagesConfig;

        [Inject]
        private CharacterBody _characterBody;

        [SerializeField]
        private Animation _animation;

        [SerializeField]
        private GameObject _uiLoadingGif;  

        [SerializeField]
        private Transform _headsRoot;

        [SerializeField]
        private Transform _bodiesRoot;

        public event Action CustomizationItemsLoaded;

        private bool _loaded;

        private Dictionary<int, CharacterCustomizationItem> _heads = new Dictionary<int, CharacterCustomizationItem>();
        private Dictionary<int, CharacterCustomizationItem> _bodies = new Dictionary<int, CharacterCustomizationItem>();

        private CancellationTokenSource _cancelTokenSource;

        public override void Show()
        {
            base.Show();
            _animation.Play();
        }

        private void Start()
        {
            if (!_loaded)
            {
                _cancelTokenSource = new CancellationTokenSource();
                LOadCutromizationItems(_cancelTokenSource.Token);
            }
        }



        private async void LOadCutromizationItems(CancellationToken token)
        {
            if (_loaded || token.IsCancellationRequested)
                return;
            _loaded = true;
            _uiLoadingGif.SetActive(true);
            
            var heads = await _prematch.GetHeads();
            foreach (var id in heads.Keys)
            {
                var headPlaceholder = _characterCustomizationItemsPool.Spawn();
                headPlaceholder.Clicked += HeadPlaceholder_Clicked;
                headPlaceholder.transform.SetParent(_headsRoot);
                headPlaceholder.Init(id);
                headPlaceholder.transform.localScale = Vector3.one;
                headPlaceholder.SetImage(heads[id]);

                //don't deselect first element
                if(_heads.Count > 0)
                    headPlaceholder.SetSelected(false);
                _heads.Add(id, headPlaceholder);
            }
            CustomizationItemsLoaded?.Invoke();

            var bodies = await _prematch.GetBodies();
            foreach (var id in bodies.Keys)
            {
                var bodyPlaceholder = _characterCustomizationItemsPool.Spawn();
                bodyPlaceholder.Clicked += Bodylaceholder_Clicked;
                bodyPlaceholder.transform.SetParent(_bodiesRoot);
                bodyPlaceholder.Init(id);
                bodyPlaceholder.transform.localScale = Vector3.one;
                bodyPlaceholder.SetImage(bodies[id].BodyPreview);

                //don't deselect first element
                if (_bodies.Count > 0)
                    bodyPlaceholder.SetSelected(false);
                _bodies.Add(id, bodyPlaceholder);
            }

            _uiLoadingGif.SetActive(false);
        }

        private void OnDestroy()
        {
            _cancelTokenSource?.Cancel();
        }

        private void DeselectAllHeads()
        {
            foreach(var head in _heads.Values)
            {
                head.SetSelected(false);
            }
        }

        private async void Bodylaceholder_Clicked(CharacterCustomizationItem sender)
        {
            _characterBody.SetTraits(await _prematch.GetBodyTraits(sender.Id));

            foreach (var body in _bodies.Values)
            {
                body.SetSelected(false);
            }
            sender.SetSelected(true);
        }

        private void HeadPlaceholder_Clicked(CharacterCustomizationItem sender)
        {
            _characterBody.Init(sender.Id);

            DeselectAllHeads();
            sender.SetSelected(true);
        }
    }
}
