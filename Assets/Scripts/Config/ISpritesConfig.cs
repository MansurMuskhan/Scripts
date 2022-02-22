using Goons.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goons.Config
{
    public interface ISpritesConfig
    {
        Sprite GetElementSprite(CardElements cardElement);
        Sprite GetElementSprite(string cardElement);
    }
}