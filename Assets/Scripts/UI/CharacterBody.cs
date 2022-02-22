using Goons.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Goons.UI
{
    public class CharacterBody : MonoBehaviour
    {
        public event Action Initialized;

        [SerializeField]
        private Image _leftHand;

        [SerializeField]
        private Image _rightHand;


        [SerializeField]
        private Image _bodyImage;

        [SerializeField]
        private Image _shoesImage;

        [SerializeField]
        private Image _beltImage;

        [SerializeField]
        private Image _pantsImage;

        [SerializeField]
        private Image _acessoryImage;

        [SerializeField]
        private Image _weaponImage;

        [Inject]
        private IPrematch _prematch;

        [Inject]
        private StartWindow _startWindow;

        [Inject]
        private CharacterWindow _characterWindow;

        private Canvas _canvas;

        public async void Init(int id)
        {
            var bodySprite = await _prematch.GetFullBody(id);
            _bodyImage.sprite = bodySprite;
        }

        public void SetTraits(BodyTraits traits)
        {
            if(traits.Shoes!= null)
            {
                _shoesImage.gameObject.SetActive(true);
                _shoesImage.sprite = traits.Shoes;
            }
            else
            {
                _shoesImage.gameObject.SetActive(false);
            }

            if (traits.Belt != null)
            {
                _beltImage.gameObject.SetActive(true);
                _beltImage.sprite = traits.Belt;
            }
            else
            {
                _beltImage.gameObject.SetActive(false);
            }

            if (traits.Weapon != null)
            {
                _weaponImage.gameObject.SetActive(true);
                _weaponImage.sprite = traits.Weapon;
                _leftHand.gameObject.SetActive(false);
            }
            else
            {
                _weaponImage.gameObject.SetActive(false);
                _leftHand.gameObject.SetActive(true);
            }

            if (traits.Acessory != null)
            {
                _acessoryImage.gameObject.SetActive(true);
                _acessoryImage.sprite = traits.Acessory;
                _rightHand.gameObject.SetActive(false);
            }
            else
            {
                _acessoryImage.gameObject.SetActive(false);
                _rightHand.gameObject.SetActive(true);
            }

            if (traits.Pants != null)
            {
                _pantsImage.gameObject.SetActive(true);
                _pantsImage.sprite = traits.Pants;
            }
            else
            {
                _pantsImage.gameObject.SetActive(false);
            }
        }

        private void CharacterWindowVisibilityChanged()
        {
            if (_characterWindow.IsViwible || _startWindow.IsViwible)
                _canvas.enabled = true;
            else
                _canvas.enabled = false;
        }

        private void OnDestroy()
        {
            _startWindow.VisibilityChanged -= CharacterWindowVisibilityChanged;
            _characterWindow.VisibilityChanged -= CharacterWindowVisibilityChanged;
        }

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        async void Start()
        {
            _startWindow.VisibilityChanged += CharacterWindowVisibilityChanged;
            _characterWindow.VisibilityChanged += CharacterWindowVisibilityChanged;

            var avatarsIds = await _prematch.GetUserAvatarIds();
            var bodySprite = await _prematch.GetFullBody(avatarsIds[0]);
            _leftHand.gameObject.SetActive(true);
            _rightHand.gameObject.SetActive(true);
            _bodyImage.sprite = bodySprite;

            Initialized?.Invoke();
        }

    }
}