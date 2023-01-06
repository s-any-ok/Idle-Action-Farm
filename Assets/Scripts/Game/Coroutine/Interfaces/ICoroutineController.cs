#nullable enable
using System.Collections;

namespace Game.Coroutine.Interfaces
{
    public interface ICoroutineController
    {
        void StartCoroutine(IEnumerator coroutine);
    }
}