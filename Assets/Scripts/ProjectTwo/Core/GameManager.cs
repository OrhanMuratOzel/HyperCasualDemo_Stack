using UnityEngine;
using System.Collections;
namespace GameTwo
{
    public class GameManager : Singleton<GameManager>
    {
        public static WaitForSeconds droppableCloseDuration = new WaitForSeconds(3);
        public static WaitForSeconds gameRestartDuration = new WaitForSeconds(3);
        private GameStates gameState;
        [Header("Main Managers")]
        [SerializeField] private InputManager inputManager;
        [SerializeField] private UIController uiController;

        [Header("Sub Managers")]
        [SerializeField] private StackManager stackManager;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private PoolManager poolManager;
        #region Get|Set
        public GameStates GetGameStates => gameState;

        #endregion
        private void Start()
        {
            ChangeGameState(GameStates.Idle);
            SoundManager.instance.Init();
            inputManager.Init(stackManager);

            playerController.Init();
            poolManager.Init();
            stackManager.Init(poolManager, playerController.MovePlayer, playerController.PlayerFall);

            LevelManager.instance.Init(stackManager, uiController.Reset,playerController.Reset);
            Reset();
        }
        private void Reset()
        {
            ChangeGameState(GameStates.Idle);
            LevelManager.instance.Reset();
            SoundManager.instance.Reset();

            inputManager.Reset();

            playerController.Reset();
            stackManager.Reset();
        }
        IEnumerator GameRestartDelay()
        {
            yield return gameRestartDuration;
            Reset();
        }
        public void LevelStart()
        {
            ChangeGameState(GameStates.Playing);
            uiController.LevelStart();
        }
        public void LevelFailed()
        {
            ChangeGameState(GameStates.Fail);
            StartCoroutine(GameRestartDelay());
        }
       
        private void ChangeGameState(GameStates toState)
        {
            gameState = toState;
        }

    }
    public enum GameStates
    {
        Idle,
        Playing,
        Success,
        Fail
    }
}
