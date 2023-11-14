using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

namespace GameTwo
{
    public class GameManager : Singleton<GameManager>
    {
        public static WaitForSeconds droppableCloseDuration = new WaitForSeconds(3);
        public static WaitForSeconds gameRestartDurationFail = new WaitForSeconds(3);
        public static WaitForSeconds gameRestartDurationSuccess = new WaitForSeconds(4);
        private GameStates gameState;
        [Header("Main Managers")]
        [SerializeField] private InputManager inputManager;
        [SerializeField] private UIController uiController;

        [Header("Sub Managers")]
        [SerializeField] private StackManager stackManager;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private PoolManager poolManager;

        private List<Action> resetActions;
        private List<Action> levelSuccess;

        #region Get|Set
        public GameStates GetGameState => gameState;

        #endregion
        private void Start()
        {
            ChangeGameState(GameStates.Idle);
            SoundManager.instance.Init();
            inputManager.Init(stackManager);

            playerController.Init();
            poolManager.Init();
            stackManager.Init(poolManager, playerController.MovePlayer, playerController.PlayerFall);
            SetActions();
            LevelManager.instance.Init(stackManager,resetActions,levelSuccess);
            Reset();
        }
        private void SetActions()
        {
            resetActions = new();
            levelSuccess = new();

            resetActions.Add(uiController.Reset);
            resetActions.Add(playerController.Reset);
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
        private IEnumerator GameRestartSucces()
        {
            yield return gameRestartDurationSuccess;
            Reset();
        }
        private IEnumerator GameRestartFail()
        {
            yield return gameRestartDurationFail;
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
            StartCoroutine(GameRestartFail());
        }
        public void LevelSuccess()
        {
            ChangeGameState(GameStates.Success);
            StartCoroutine(GameRestartSucces());
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
