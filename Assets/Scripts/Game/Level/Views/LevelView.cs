using System;
using System.Collections;
using System.Collections.Generic;
using Game.Level.Enum;
using TMPro;
using UnityEngine;

namespace Game.Level.Views
{
    public class LevelView : MonoBehaviour
    {
        public event Action<Vector3> OnMove;
 
        [SerializeField] private TextMeshProUGUI _coinsTMP;
        [SerializeField] private TextMeshProUGUI _wheatTMP;
        [SerializeField] private GameObject _coinImg;
        [SerializeField] private float _txtScaleTime;
        [SerializeField] private Vector3 _txtScaleMax;
        [SerializeField] private List<GameObject> _disableIngame;

        private ELevelState _levelState;
        private Joystick _joystick;
        private InputSystem _inputSystem;
        
        public TextMeshProUGUI CoinsTMP => _coinsTMP;
        public TextMeshProUGUI WheatTMP => _wheatTMP;

        public InputSystem InputSystem => _inputSystem;

        void Awake()
        {
            _levelState = ELevelState.WaitingTap;
            _inputSystem = new InputSystem();
            _joystick = new Joystick();
        }

        public void LevelStateManager()
        {
            switch (_levelState)
            {
                case ELevelState.WaitingTap:
                    if (_inputSystem.TouchInfo.Phase == TouchPhase.Ended)
                    {
                        ChangeLevelState(ELevelState.Ingame);
                    }
                    break;
                case ELevelState.Ingame:
                    if (_inputSystem.TouchInfo.Phase == TouchPhase.Began)
                    {
                        _joystick.ShowJoystick(true, _inputSystem.TouchInfo.StartPos);
                                
                    }
                    else if ((_inputSystem.TouchInfo.Phase == TouchPhase.Moved || _inputSystem.TouchInfo.Phase == TouchPhase.Stationary))
                    {
                        OnMove?.Invoke(_joystick.MoveJoystick(_inputSystem.TouchInfo.Direction));
                    }
                    else if (_inputSystem.TouchInfo.Phase == TouchPhase.Ended)
                    {
                        _joystick.ShowJoystick(false, _inputSystem.TouchInfo.StartPos);
                    }               
                    break;
            }
        }

        public void ChangeLevelState(ELevelState newLevelState)
        {
            //check old level state and based on it clean up some things
            switch (_levelState)
            {
                case ELevelState.WaitingTap:
                    if (newLevelState == ELevelState.Ingame)
                    {
                        foreach(GameObject gameObject in _disableIngame)
                        {
                            gameObject.SetActive(false);
                        }
                    }
                    break;
                case ELevelState.Ingame:
                    break;
            }
            _levelState = newLevelState;
        }

        public Coin SpawnCoin(Vector3 startPosition)
        {
            GameObject gameObject = Instantiate(_coinImg, _coinsTMP.transform.parent);
            Coin coin = gameObject.GetComponent<Coin>();
            coin.Move(Camera.main.WorldToScreenPoint(startPosition));
            return coin;
        }

        public void UpdateTXT(string value, TextMeshProUGUI txtObj)
        {
            txtObj.text = value;
            StartCoroutine(ScaleTXT(_txtScaleTime, txtObj));
        }

        IEnumerator ScaleTXT(float time, TextMeshProUGUI txtObj)
        {
            txtObj.transform.localScale = _txtScaleMax;

            yield return new WaitForSeconds(time);

            txtObj.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
