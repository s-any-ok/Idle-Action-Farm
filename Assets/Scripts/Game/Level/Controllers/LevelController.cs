using System;
using Game.Level.Interfaces;
using Game.Level.Views;
using Game.Player.Interfaces;
using UnityEngine;
using Zenject;

namespace Game.Level.Controllers
{
    public class LevelController: ILevelController, IInitializable, ITickable, IDisposable
    {
        private readonly LevelView _view;
        private readonly IPlayerDatabase _playerDatabase;
        private readonly GameData _gameData;
        private readonly IPlayerController _playerController;
        
        public LevelView View => _view;
        public void Initialize() {}
        
        public LevelController(LevelView view, IPlayerDatabase playerDatabase, GameData gameData)
        {
            _view = view;
            _playerDatabase = playerDatabase;
            _gameData = gameData;
            
            _gameData.Wheat = 0;
            _gameData.Coins = 0;
            _view.CoinsTMP.text = _gameData.Coins.ToString();
            _view.WheatTMP.text = _gameData.Wheat.ToString("00") + "/" + _playerDatabase.MaxStacked;
        }
        
        public void Tick()
        {
            _view.InputSystem.ReadInput();
            _view.LevelStateManager();
        }
        
        public void SpawnCoin(Vector3 startPosition)
        {
            var coin = _view.SpawnCoin(startPosition);
            coin.SetLevelController(this);
        }

        public void AddCoins(int value)
        {
            _gameData.Coins += (value * _playerDatabase.WheatValue);
            if (_gameData.Coins > 9999) _gameData.Coins = 9999;
            _view.UpdateTXT(_gameData.Coins.ToString(), _view.CoinsTMP);
        }

        public void AddWheat(int value)
        {
            _gameData.Wheat += value;
            _view.UpdateTXT(_gameData.Wheat.ToString("00") + "/" + _playerDatabase.MaxStacked, _view.WheatTMP);
        }

        public void Dispose()
        {
        
        }
    }
}