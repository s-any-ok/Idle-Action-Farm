using Game.Level.Controllers;
using UnityEngine;

namespace Game
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private float _scaleSpeed;
        [SerializeField] private Vector3 _minScale;
        [SerializeField] private Vector3 _maxScale;
        [SerializeField] private RectTransform _transform;
        
        private LevelController _levelController;
        private Vector3 _startPoint;
  
        private Vector3 _origPos;
        private float _delta = 0.001f;
        private bool _isMoving = false;

        void Awake()
        {
            _origPos = _transform.position;
        }

        public void SetLevelController(LevelController levelController)
        {
            _levelController = levelController;
        }

        public void Move(Vector3 startPoint)
        {
            _transform.position = startPoint;
            _isMoving = true;
            _transform.localScale = _minScale;
        }
        void Update()
        {
            if (_isMoving && _levelController != null)
            {
                _transform.position = Vector3.MoveTowards(_transform.position, _origPos, Time.deltaTime * Screen.width);
                _transform.localScale = Vector3.MoveTowards(_transform.localScale, _maxScale, _scaleSpeed * Time.deltaTime);
                if (Vector3.Distance(_transform.position, _origPos) < _delta)
                {
                    _levelController.AddCoins(1);
                    Destroy(gameObject);
                }
            }
        }
    }
}
