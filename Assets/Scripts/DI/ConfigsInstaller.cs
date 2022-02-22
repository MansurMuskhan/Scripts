using UnityEngine;
using Zenject;
using Goons.Config;

[CreateAssetMenu(fileName = "ConfigsInstaller", menuName = "Installers/ConfigsInstaller")]
public class ConfigsInstaller : ScriptableObjectInstaller<ConfigsInstaller>
{
    [SerializeField]
    private ScriptableObjectSpritesConfig _spritesConfig;
    [SerializeField]
    private ScriptableObjectGradientsConfig _gradientsConfig;
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<NetworkCardsConfig>().AsSingle();
        Container.BindInterfacesTo<ScriptableObjectSpritesConfig>().FromInstance(_spritesConfig);
        Container.BindInterfacesTo<ScriptableObjectGradientsConfig>().FromInstance(_gradientsConfig);
        Container.BindInterfacesTo<CustomizationImagesConfig>().AsSingle();
        
    }
}