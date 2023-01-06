using Game.Player.Controllers;
using Game.Player.Interfaces;
using Game.Player.Scriptable;
using Game.Player.Views;
using UnityEngine;
using Zenject;

namespace Game.Player.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private PlayerDatabase _playerDatabase;
        public override void InstallBindings()
        {
            Container.Bind<IPlayerDatabase>().FromInstance(_playerDatabase);
            Container.BindInterfacesTo<PlayerController>().AsSingle().WithArguments(_playerView);
        }
    }
}