using System.Collections;
using Game.Coroutine.Interfaces;
using Game.Coroutine.Views;

namespace Game.Coroutine.Controllers
{
    public class CoroutineController: ICoroutineController
    {
        private readonly CoroutineView _view;

        public CoroutineController(CoroutineView view)
        {
            _view = view;
        }
        
        public void StartCoroutine(IEnumerator coroutine)
        {
            _view.StartCoroutine(coroutine);
        }
    }
}