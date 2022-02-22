using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundButton : SoundUiElement
{
    [SerializeField]
    private AudioClip _sound;
    private Button _button;
    protected override void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(PlaySoundOnClick);
    }

    protected override void OnDestroy()
    {
        _button.onClick.RemoveListener(PlaySoundOnClick);
    }
    private void PlaySoundOnClick()
    {
        _audioSource.PlayOneShot(_sound);
    }
}

public class SoundHover : SoundUiElement
{
    private Selectable _hoveredElement;

    protected override void OnDestroy()
    {
    }

    protected override void Start()
    {
        _hoveredElement = GetComponent<Selectable>();
    }
}

public abstract class SoundUiElement : MonoBehaviour
{




    protected AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = FindObjectOfType<AudioSource>();
    }



    protected abstract void Start();

    protected abstract void OnDestroy();
}