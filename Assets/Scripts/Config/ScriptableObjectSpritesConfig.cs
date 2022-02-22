using Goons.Model;
using UnityEngine;


namespace Goons.Config {
    [CreateAssetMenu(fileName = "SpritesConfig", menuName = "Config/SpritesConfig")]
    public class ScriptableObjectSpritesConfig : ScriptableObject, ISpritesConfig
    {

        [SerializeField]
        private Sprite _fire, _earth, _electric, _neutral, _water;

        public Sprite GetElementSprite(string cardElement)
        {
            switch (cardElement)
            {
                default:
                case "fire":
                    return _fire;
                case "earth":
                    return _earth;
                case "electric":
                    return _electric;
                case "water":
                    return _water;
                case "neutral":
                    return _neutral;
            }
        }

        public Sprite GetElementSprite(CardElements cardElement)
        {
            switch (cardElement)
            {
                default:
                case CardElements.Fire:
                    return _fire;
                case CardElements.Earth:
                    return _earth;
                case CardElements.Electric:
                    return _electric;
                case CardElements.Water:
                    return _water;
                case CardElements.Neutral:
                    return _neutral;
            }
        }
    }
}