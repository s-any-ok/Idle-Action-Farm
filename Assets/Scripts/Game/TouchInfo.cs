using UnityEngine;

namespace Game
{
    public struct TouchInfo
    {
        public Vector3 StartPos {get; set;}
        public Vector3 StartPosWorld {get; set;}
        public Vector3 Direction {get; set;}
        public Vector3 DirectionWorld {get; set;}
        public bool IsInteractableUI {get; set;}
        public TouchPhase Phase {get; set;}
        
        public TouchInfo(Vector3 startPos, Vector3 direction, bool isInteractableUI, TouchPhase phase)
        {
            StartPos = startPos;
            StartPosWorld = startPos;
            Direction = direction;
            DirectionWorld = direction;
            IsInteractableUI = isInteractableUI;
            Phase = phase;
        }
    }
}
