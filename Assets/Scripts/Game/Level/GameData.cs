using UnityEngine;

namespace Game.Level
{
    [CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 2)]
    public class GameData : ScriptableObject
    {
        [SerializeField] private int _coins;
        public int Coins 
        {
            get => _coins;
            set => _coins = value;
        }

        private int _levelNum;

        private int _wheat = 0;
        public int Wheat
        {
            get => _wheat;
            set => _wheat = value;
        }
    }
}
