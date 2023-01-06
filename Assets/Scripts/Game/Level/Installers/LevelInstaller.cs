using Game.Level.Controllers;
using Game.Level.Views;
using UnityEngine;
using Zenject;

namespace Game.Level.Installers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private LevelView _levelView;
        [SerializeField] private GameData _gameData;
        public override void InstallBindings()
        {
            Container.BindInstance(_gameData).AsSingle();
            Container.BindInterfacesTo<LevelController>().AsSingle().WithArguments(_levelView);
        }
    }
}