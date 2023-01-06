#nullable enable
using Game.Player.Interfaces;
using UnityEngine;

namespace Game.Player.Scriptable
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Database/PlayerDatabase", fileName = "PlayerDatabase")]
    public class PlayerDatabase : ScriptableObject, IPlayerDatabase
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _turnSpeed;
        [SerializeField] private float _sellTime;
        [SerializeField] private float _coinTime;
        [SerializeField] private int _wheatValue;
        [SerializeField] private int _maxStacked;

        public float MoveSpeed => _moveSpeed;
        public float TurnSpeed => _turnSpeed;
        public float SellTime => _sellTime;
        public float CoinTime => _coinTime;
        public int WheatValue => _wheatValue;
        public int MaxStacked => _maxStacked;
    }
}
