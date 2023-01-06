using Game.Coroutine.Controllers;
using Game.Coroutine.Views;
using UnityEngine;
using Zenject;

namespace Game.Coroutine.Installers
{
    public class CoroutineInstaller : MonoInstaller
    {
        [SerializeField] private CoroutineView _coroutineView;
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<CoroutineController>().AsSingle().WithArguments(_coroutineView);
        }
    }
}