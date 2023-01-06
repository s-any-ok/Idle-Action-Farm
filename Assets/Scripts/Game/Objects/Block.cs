using System.Collections;
using UnityEngine;

namespace Game
{
    public class Block : MonoBehaviour
    {
        [SerializeField] private float _destroyTime;
        [SerializeField] private float _speed;
        [SerializeField] private Vector3 _anchorPoint1;
        [SerializeField] private Vector3 _anchorPoint2;
        [SerializeField] private Material _stackMaterial;
        [SerializeField] private Texture _limitTexture;
        [SerializeField] private float _textureTime;
        [SerializeField] private BoxCollider _collider;
        [SerializeField] private Animator _animator;
        
        private Material _origMaterial;
        private Texture _origTexture;
        private bool _isMoving = false;
     
        private Transform _endPoint;
        private Vector3 _offset;
        private Transform _startPoint;

        private float _tParam = 0;
        private Vector3 _objectPosition;
        private bool _isDestroy = true;
        private bool _couRunning = false;

        void Awake()
        {
            _origMaterial = GetComponentInChildren<MeshRenderer>().sharedMaterial;
            _origTexture = _origMaterial.GetTexture("_MainTex");
        }

        void Start()
        {
            if (_isDestroy) StartCoroutine(DestroyOnTime(_destroyTime));
        }

        public void Stack(Transform position, int blockOrder)
        {
            _isDestroy = false;
            _isMoving = true;
            _collider.enabled = false;
            _endPoint = position;
            _offset = new Vector3 (0, blockOrder * _collider.size.y, 0);
            transform.SetParent(_endPoint.parent);
            _startPoint = transform;
            _animator.Play("block_bounce", 0, 1);
            GetComponentInChildren<MeshRenderer>().sharedMaterial = _stackMaterial;
        }

        public void Unstack(Transform position)
        {
            _isDestroy = true;
            _isMoving = true;
            _startPoint = transform;
            _endPoint = position;
            transform.SetParent(_endPoint.parent);
            _offset = Vector3.zero;
            _tParam = 0;
            GetComponentInChildren<MeshRenderer>().sharedMaterial = _origMaterial;
        }

        public void SetLimitTexture()
        {
            if (!_couRunning) StartCoroutine(ChangeTexture(_textureTime, _limitTexture));
            _couRunning = true;
        }

        private IEnumerator ChangeTexture(float time, Texture texture)
        {
            _stackMaterial.SetTexture("_MainTex", texture);

            yield return new WaitForSeconds(time);

            _stackMaterial.SetTexture("_MainTex", _origTexture);
            _couRunning = false;
        }

        private IEnumerator DestroyOnTime(float time)
        {
            yield return new WaitForSeconds(time);
            if (_isDestroy) Destroy(this.gameObject);
        }

        private void Update()
        {
            if (_isMoving)
            {
                if(_tParam < 1)
                {
                    _tParam += Time.deltaTime * _speed;

                    _objectPosition = Mathf.Pow(1 - _tParam, 3) * _startPoint.localPosition 
                                      + 3 * Mathf.Pow(1 - _tParam, 2) * _tParam * (_startPoint.localPosition + _anchorPoint1) 
                                      + 3 * (1 - _tParam) * Mathf.Pow(_tParam, 2) * (_endPoint.localPosition + _anchorPoint2 + _offset)
                                      + Mathf.Pow(_tParam, 3) * (_endPoint.localPosition + _offset);

                    transform.localPosition = _objectPosition;

                    transform.localRotation = Quaternion.Slerp(transform.localRotation, _endPoint.localRotation, _speed * 100 * Time.deltaTime);
                }
                else
                {
                    _isMoving = false;
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition, _endPoint.localPosition + _offset, 100);
                    transform.localRotation =  _endPoint.localRotation;
                    transform.SetParent(_endPoint);
                    if (_isDestroy) StartCoroutine(DestroyOnTime(_destroyTime));
                }
            }
        }
    }
}
