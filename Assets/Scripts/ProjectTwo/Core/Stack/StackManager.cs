using DG.Tweening;
using UnityEngine;
using System;
using System.Collections.Generic;
namespace GameTwo
{
    public class StackManager : MonoBehaviour
    {
        #region Color
        public static readonly int ColorID = Shader.PropertyToID("_Color");
        [SerializeField] private Color[] colors;
        [SerializeField] private float stackZDistance;
        [SerializeField] private float minStackScale;
        private int colorIndex = 0;
        private int levelStackCount;
        private int currentStackCount;
        #endregion
        private PoolManager poolManager;
        private Stack currentStack;
        private List<Stack> usedStackList;
        [SerializeField] private StackMovement stackMovement;
        private float previousScale;
        private float previousXPosition;
        private Action<Vector3> correctTapAction;
        private Action<Vector3> playerFallAction;
        public void Init(PoolManager poolManager, Action<Vector3> correctTapAction, Action<Vector3> playerFallAction)
        {
            this.poolManager = poolManager;
            this.correctTapAction = correctTapAction;
            this.playerFallAction = playerFallAction;
            usedStackList = new();
        }
        public void Reset()
        {
            colorIndex = 0;
            currentStackCount = 0;
            stackMovement.currentStackTransform = stackMovement.startStack;
            previousScale = stackMovement.startStack.localScale.x;
            foreach (var item in usedStackList)
            {
                poolManager.BackToPoolStack(item);
            }
            usedStackList.Clear();
        }
        public void OnLevelStart(int maxStackCount)
        {
            levelStackCount = maxStackCount;
            SetStackTransform();
            currentStack.OpenStack(ref GetNextColor());
            StartMovingStack();
        }

        #region Input Controller | Stack Functions
        public void StopStackObject()
        {
            currentStackCount++;

            StopCurrentStack();
            
            if (GameManager.instance.GetGameState != GameStates.Playing)
                return;
            previousScale = stackMovement.currentStackTransform.localScale.x;
            previousXPosition = stackMovement.currentStackTransform.localPosition.x;
            if (currentStackCount >= levelStackCount)
            {
                SuccessLevel();
                return;
            }
            stackMovement.isMovingRight = !stackMovement.isMovingRight;
            SetStackTransform();
            currentStack.OpenStack(ref GetNextColor());
            StartMovingStack();
        
        }
        private void SetStackTransform()
        {
            stackMovement.stackPosition = stackMovement.currentStackTransform.position;
            stackMovement.stackPosition.z += stackZDistance;
            stackMovement.stackPosition.x = stackMovement.isMovingRight? stackMovement.xRightPosition: stackMovement.xLeftPosition;
            stackMovement.stackScale = stackMovement.currentStackTransform.transform.localScale;
            if (currentStack != null)
                usedStackList.Add(currentStack);
            currentStack = poolManager.GetStack();
            currentStack.transform.position = stackMovement.stackPosition;
            currentStack.transform.localScale= stackMovement.stackScale;

            stackMovement.currentStackTransform = currentStack.transform;
        }
        private void StartMovingStack()
        {
            if(stackMovement.isMovingRight)
            {
                stackMovement.movementTween = stackMovement.currentStackTransform
                    .DOLocalMoveX(stackMovement.xLeftPosition, stackMovement.motionDuration)
                    .SetEase(stackMovement.movementEase)
                    .OnComplete(() =>
                {
                    stackMovement.isMovingRight = false;
                    StartMovingStack();
                });
            }
            else
            {
                stackMovement.movementTween = stackMovement.currentStackTransform
                    .DOLocalMoveX(stackMovement.xRightPosition, stackMovement.motionDuration)
                    .SetEase(stackMovement.movementEase)
                    .OnComplete(() =>
                {
                    stackMovement.isMovingRight = true;
                    StartMovingStack();
                }); ;
            }
        }
        public ref Color GetNextColor()
        {
            return ref colors[colorIndex++% colors.Length];
        }
        public ref Color GetCurrentColor()
        {
            return ref colors[(colorIndex-1) % colors.Length];
        }
        public void StopCurrentStack()
        {
            if(stackMovement.movementTween is not null)
                stackMovement.movementTween.Kill();

            if (Mathf.Abs(stackMovement.currentStackTransform.localPosition.x) > previousScale+ Mathf.Abs(previousXPosition))
            {
                LevelFailed();
                return;
            }
            if(Mathf.Abs(stackMovement.currentStackTransform.localPosition.x - previousXPosition) <=previousScale*stackMovement.perfectScoreOffSet)
            {
                PerfectTap();
                return;
            }
            NormalTap();
        }
        private void PerfectTap()
        {
            var perfectPosition = stackMovement.currentStackTransform.localPosition;
            perfectPosition.x = previousXPosition;
            stackMovement.currentStackTransform.localPosition = perfectPosition;
            SoundManager.instance.PerfectTap();
            CorrectTapMovePlayer(perfectPosition);
        }
        private void NormalTap()
        {
            var newPivot = stackMovement.currentStackTransform.localPosition;
            var tmpScale = stackMovement.currentStackTransform.localScale;
            if (stackMovement.currentStackTransform.localPosition.x > previousXPosition)
            {
                var newScale = stackMovement.currentStackTransform.localScale.x - Mathf.Abs(stackMovement.currentStackTransform.localPosition.x - previousXPosition);
                tmpScale.x = newScale;
                if (tmpScale.x < minStackScale)
                {
                    LevelFailed();
                    return;
                }
                var cutPoint = previousXPosition + previousScale / 2;
                newPivot.x = cutPoint - newScale / 2;
                SetDropableObject(newScale, cutPoint, newPivot, false);
            }
            else
            {
                var newScale = stackMovement.currentStackTransform.localScale.x - Mathf.Abs(stackMovement.currentStackTransform.localPosition.x - previousXPosition);
                tmpScale.x = newScale;
                if (tmpScale.x < minStackScale)
                {
                    LevelFailed();
                    return;
                }
                var cutPoint = previousXPosition - previousScale / 2;
                newPivot.x = cutPoint + newScale / 2;
                SetDropableObject(newScale, cutPoint, newPivot, true);
            }
            SoundManager.instance.NormalTap();
            stackMovement.currentStackTransform.localScale = tmpScale;
            stackMovement.currentStackTransform.localPosition = newPivot;
            CorrectTapMovePlayer(newPivot);
        }
        #endregion
        #region Game Stage Changes
        private void LevelFailed()
        {
            PlayerFall(stackMovement.currentStackTransform.localPosition);
            FailDroppable(currentStack.transform.localScale, currentStack.transform.position);
            poolManager.BackToPoolStack(currentStack);
            LevelManager.instance.LevelFailed();
        }
        private void SuccessLevel()
        {
            LevelManager.instance.LevelSucces();
        }
        #endregion

        #region Player Actions
        private void CorrectTapMovePlayer(Vector3 toPos)
        {
            correctTapAction(toPos);
        }
        private void PlayerFall(Vector3 toPos)
        {
            playerFallAction(toPos);
        }
        #endregion
        #region Droppable
        private void FailDroppable(Vector3 scale, Vector3 position)
        {
            var dropObject = poolManager.GetDroppable();
            dropObject.ActivatePiece(ref scale, ref position, ref GetCurrentColor());
            poolManager.BackToPoolDroppable(dropObject);
        }
        private void SetDropableObject(float newScale,float cutPointX,Vector3 newPos,bool isLeftSide)
        {
            var dropObject = poolManager.GetDroppable();

            var dropObjectScale = dropObject.transform.localScale;
            dropObjectScale.x = previousScale - newScale;
            dropObject.transform.localScale = dropObjectScale;

            if(isLeftSide)
                newPos.x = cutPointX - dropObjectScale.x / 2;
            else
                newPos.x = cutPointX + dropObjectScale.x / 2;

            dropObject.ActivatePiece(ref dropObjectScale,ref newPos, ref GetCurrentColor());
            poolManager.BackToPoolDroppable(dropObject);
        }
        #endregion
        [System.Serializable]
        class StackMovement
        {
            public float perfectScoreOffSet;
            public Tween movementTween;
            public Ease movementEase;
            public Transform startStack;
            [HideInInspector] public Transform currentStackTransform;
            [HideInInspector] public Vector3 stackPosition;
            [HideInInspector] public Vector3 stackScale;
            public float xLeftPosition;
            public float xRightPosition;
            public float motionDuration;
            [HideInInspector] public bool isMovingRight = false;
        }
    }
}