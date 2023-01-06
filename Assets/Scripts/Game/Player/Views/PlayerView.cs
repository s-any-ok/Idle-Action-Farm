using System;
using Cinemachine;
using Game.Player.Enum;
using UnityEngine;

namespace Game.Player.Views
{
    public class PlayerView : MonoBehaviour
    {
        public event Action<Collider> OnTriggerEnterE;
        public event Action<Collider> OnTriggerExitE;
        public event Action<int> OnAddWheat;
        public event Action<Vector3> OnSpawnCoin;
        
        [SerializeField] private GameObject _weaponAttack;
        [SerializeField] private GameObject _weaponIdle;
        [SerializeField] private Transform _stackBlocksPoint;
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private float _cameraLimitDistance;
        [SerializeField] private float _cameraStep;
        [SerializeField] private Rigidbody _ridigbody;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _model;
        
        private CinemachineFramingTransposer _cameraSettings;
        private bool _isMoving;
        private Vector3 _movePosition;
        
        public bool IsMoving
        {
            get => _isMoving;
            set => _isMoving = value;
        }

        public Transform Model => _model;
        public Rigidbody Rigidbody => _ridigbody;
        public EPlayerState PlayerState => _state;
        public Transform StackBlocksPoint => _stackBlocksPoint;
        public Vector3 MovePosition => _movePosition;
      
        private EPlayerState _state = EPlayerState.Idle;

        private void Awake()
        {
            _cameraSettings = _camera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        public void Move(Vector3 newPosition)
        { 
            //lock Y from changes
            _movePosition = new Vector3(newPosition.x, 0, newPosition.y);
            if (!Mathf.Approximately(_movePosition.sqrMagnitude, 0))
            {
                MoveAnim(_movePosition.sqrMagnitude);
                _isMoving = true;
            }
            //Debug.Log("Move pos: " + _movePosition);     
        }

        private void MoveAnim(float strength)
        {
            _state = EPlayerState.Moving;
            if (Mathf.Approximately(strength, 1) && !_animator.GetBool("IsAttacking"))
            {
                _animator.SetBool("isWalking", false);
                _animator.SetBool("isRunning", true);
            }
            else
            {
                _animator.SetBool("isWalking", true);
                _animator.SetBool("isRunning", false);
            }
        }

        public void AttackAnim(bool value)
        {
            _animator.SetBool("IsAttacking", value);
            _weaponAttack.SetActive(value);
            _weaponIdle.SetActive(!value);
        }

        public void StopAnim()
        {
            _state = EPlayerState.Idle;
            _animator.SetBool("isWalking", false);
            _animator.SetBool("isRunning", false);
        }

        private void OnTriggerEnter(Collider other) 
        {
            OnTriggerEnterE?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTriggerExitE?.Invoke(other);
        }

        public void SetCameraDistance(int blocksCount)
        {
            float newCameraDistance = blocksCount / _cameraLimitDistance + _cameraStep;
            if (newCameraDistance > _cameraLimitDistance) _cameraSettings.m_CameraDistance = newCameraDistance;
            else _cameraSettings.m_CameraDistance = _cameraLimitDistance;
        }
    }
}
