using UnityEngine;

namespace Goons.Config
{
    public interface IRarityGradientsConfig
    {
        Color GetColor(Rarity rarity);
    }
    [CreateAssetMenu(fileName = "GradientsConfig", menuName = "Config/Gradients Config")]
    public class ScriptableObjectGradientsConfig : ScriptableObject, IRarityGradientsConfig
    {
        [SerializeField]
        private Color _common;        
        
        [SerializeField]
        private Color _rare;        
        
        [SerializeField]
        private Color _epic;        
        
        [SerializeField]
        private Color _legendary;

        public Color GetColor(Rarity rarity)
        {
            switch (rarity)
            {
                default:
                case Rarity.Common:
                    return _common;
                case Rarity.Rare:
                    return _rare;
                case Rarity.Epic:
                    return _epic;
                case Rarity.Legendary:
                    return _legendary;
            }
        }
    }
}