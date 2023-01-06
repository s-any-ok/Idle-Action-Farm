#nullable enable
using Game.Level.Views;
using UnityEngine;

namespace Game.Level.Interfaces
{
    public interface ILevelController
    {
        LevelView View { get; }
        void SpawnCoin(Vector3 startPosition);
        void AddCoins(int value);
        void AddWheat(int value);
    }
}