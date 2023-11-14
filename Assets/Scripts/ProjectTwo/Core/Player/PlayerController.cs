using UnityEngine;
using DG.Tweening;
namespace GameTwo
{

    public class PlayerController : MonoBehaviour
    {
        #region Animation Hashes
        private static readonly int PlayerIdleAnimation = Animator.StringToHash("Idle");
        private static readonly int PlayerRunAnimation = Animator.StringToHash("Run");
        private static readonly int PlayerDanceAnimation = Animator.StringToHash("Dance");
        #endregion
        [SerializeField] private Animator playerAnimator;
        private Transform modelTransform;
        private Tween moveTween;
        private Tween modelTween;
        public void Init()
        {
            modelTransform = transform.GetChild(0);
        }
        public void Reset()
        {
            if (modelTween is not null)
                modelTween.Kill();
            modelTransform.localRotation = Quaternion.identity;
            modelTransform.localPosition = Vector3.zero;
            transform.position = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            StopPlayer();
        }
        private void StopPlayer()
        {
            if (GameManager.instance.GetGameState == GameStates.Success)
            {
               
                PlayerToFinishLine();
               
            }
            else
                playerAnimator.Play(PlayerIdleAnimation);
        }
        private void PlayerToFinishLine()
        {
            moveTween = transform.DOMoveZ(2.5f, .5f).SetRelative(true).OnComplete(() =>
            {
                PlayerDance();
                LevelManager.instance.PlayerOnPlatform();
            });
        }
        private void PlayerDance()
        {
            playerAnimator.Play(PlayerDanceAnimation);
        }

        #region Player Movement
        public void PlayerFall(Vector3 toPos)
        {
            if (moveTween != null)
                moveTween.Kill();
            toPos.y = transform.position.y;
            moveTween = modelTransform.DOMove(toPos, .5f).OnComplete(() =>
            {
                StopPlayer();
                modelTween = modelTransform.DOMoveY(-20, 4).SetRelative(true);
                modelTransform.DORotate(new Vector3(90, 0, 0), .2f);
            });

            playerAnimator.Play(PlayerRunAnimation);
        }
        public void MovePlayer(Vector3 toPos)
        {
            if (moveTween != null)
                moveTween.Kill();
            toPos.y = transform.position.y;
            modelTransform.LookAt(toPos);
            moveTween = transform.DOMove(toPos, .5f).OnComplete(() =>
            {
                StopPlayer();
            });
            playerAnimator.Play(PlayerRunAnimation);
        }
        #endregion
    }
}