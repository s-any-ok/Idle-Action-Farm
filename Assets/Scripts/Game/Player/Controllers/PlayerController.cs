using System;
using System.Collections;
using System.Collections.Generic;
using Game.Coroutine.Interfaces;
using Game.Level.Interfaces;
using Game.Player.Enum;
using Game.Player.Interfaces;
using Game.Player.Views;
using UnityEngine;
using Zenject;

namespace Game.Player.Controllers
{
    public class PlayerController: IPlayerController, IInitializable, ITickable, IDisposable
    {
        private readonly PlayerView _view;
        private readonly IPlayerDatabase _playerDatabase;
        private readonly ILevelController _levelController;
        private readonly ICoroutineController _coroutineController;
        
        private Stack<Transform> _stackedBlocks = new();

        private readonly float _moveSpeed;
        private readonly float _turnSpeed;
        private readonly float _sellTime;
        private readonly float _coinTime;
        private bool _isSellBlocks;
        private int _coinsToSpawn;
        
        public PlayerView View => _view;

        public void Initialize() {}
        
        public PlayerController(PlayerView view, IPlayerDatabase playerDatabase, ILevelController levelController,ICoroutineController coroutineController)
        {
            _view = view;
            _playerDatabase = playerDatabase;
            _levelController = levelController;
            _coroutineController = coroutineController;

            _moveSpeed = _playerDatabase.MoveSpeed;
            _turnSpeed = _playerDatabase.TurnSpeed;
            _sellTime = _playerDatabase.SellTime;
            _coinTime = _playerDatabase.CoinTime;

            _view.OnTriggerEnterE += OnTriggerEnter;
            _view.OnTriggerExitE += OnTriggerExit;

            _view.OnAddWheat += OnAddWheat;
            _view.OnSpawnCoin += OnSpawnCoin;

            _levelController.View.OnMove += OnMove;
        }

        private void OnMove(Vector3 val)
        {
            _view.Move(val);
        }

        private void OnSpawnCoin(Vector3 val)
        {
            _levelController.SpawnCoin(val);
        }

        private void OnAddWheat(int val)
        {
            _levelController.AddWheat(val);
        }

        public void Tick()
        {
            if (_view.IsMoving)
            {
                _view.Rigidbody.velocity = _view.MovePosition * _moveSpeed;
                _view.Model.rotation = Quaternion.Slerp(_view.Model.rotation, Quaternion.LookRotation(_view.MovePosition), _turnSpeed * Time.fixedDeltaTime);
                _view.IsMoving = false;
            }
            else
            {
                if (_view.PlayerState != EPlayerState.Idle) _view.StopAnim();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (other.tag)
            {
                case "Field":
                    _view.AttackAnim(true);
                    break;
                case "Wheat":
                    other.GetComponent<Wheat>().Cut();
                    break;
                case "Block":
                    CollectBlock(other.gameObject);
                    break;
                case "Sell":
                    SellBlocks(other.gameObject, _sellTime, _coinTime);
                    break;
                default:
                    Debug.Log("Trigger enter not implemented: " + other.tag);
                    break;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            switch (other.tag)
            {
                case "Field":
                    _view.AttackAnim(false);
                    break;
                case "Sell":
                    _isSellBlocks = false;
                    break;
                default:
                    Debug.Log("Trigger exit not implemented: " + other.tag);
                    break;
            }
        }

        private void CollectBlock(GameObject block)
        {
            if (_stackedBlocks.Count < _playerDatabase.MaxStacked)
            {
                _levelController.AddWheat(1);
                _stackedBlocks.Push(block.transform);
                block.GetComponent<Block>().Stack(_view.StackBlocksPoint, _stackedBlocks.Count);
                _view.SetCameraDistance(_stackedBlocks.Count);
            }
            else
            {
                block.GetComponent<Block>().SetLimitTexture();
            }
        }
        
        private void SellBlocks(GameObject block, float sellTime, float coinTime)
        {
            _isSellBlocks = true;
            _coroutineController.StartCoroutine(SellBlock(sellTime, coinTime, block.GetComponentsInChildren<Transform>()[1]));
        }

        private IEnumerator SellBlock(float sellTime, float coinTime, Transform block)
        {
            _coinsToSpawn = 0;
            while (_isSellBlocks)
            {
                if (_stackedBlocks.Count > 0)
                {
                    _coinsToSpawn++;
                    _levelController.AddWheat(-1);
                    _stackedBlocks.Pop().GetComponent<Block>().Unstack(block);
                    _view.SetCameraDistance(_stackedBlocks.Count);
                }
                else
                {
                    _isSellBlocks = false;
                }
                yield return new WaitForSeconds(sellTime);
            }
            if (_coinsToSpawn > 0)
            {
                _coroutineController.StartCoroutine(SpawnCoins(coinTime, block.position));
            }
        }

        private IEnumerator SpawnCoins(float time, Vector3 startPoint)
        {
            yield return new WaitForSeconds(time);
            while (_coinsToSpawn > 0)
            {
                _coinsToSpawn--;
                _levelController.SpawnCoin(startPoint);
                yield return new WaitForSeconds(time);
            }
        }
        
        public void Dispose()
        {
            _view.OnTriggerEnterE -= OnTriggerEnter;
            _view.OnTriggerExitE -= OnTriggerExit;
            
            _view.OnAddWheat -= OnAddWheat;
            _view.OnSpawnCoin -= OnSpawnCoin;

            _levelController.View.OnMove -= OnMove;
        }
    }
}