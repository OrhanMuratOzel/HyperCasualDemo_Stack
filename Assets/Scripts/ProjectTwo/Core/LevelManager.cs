using System;
using UnityEngine;
using System.Collections.Generic;
namespace GameTwo
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private LevelDataScriptable[] levelData;
        [SerializeField] private GameObject[] cinemachineCameras;
        private StackManager stackManager;
        private int currentLevel = 0;
        private List<Action> resetActions;
        private List<Action> levelSuccess;


        public void Init(StackManager stackManager, List<Action> resetActions, List<Action> levelSuccess)
        {
            this.stackManager = stackManager;
            this.resetActions = resetActions;
            this.levelSuccess = levelSuccess;
        }


        public void Reset()
        {
            foreach (var item in resetActions)
            {
                item?.Invoke();
            }
            cinemachineCameras[0].SetActive(true);
            cinemachineCameras[1].SetActive(false);
        }
        public void LevelStart()
        {
            stackManager.OnLevelStart(levelData[currentLevel].stackCount);
            GameManager.instance.LevelStart();
        }

        public void LevelSucces()
        {
            GameManager.instance.LevelSuccess();
            foreach (var item in levelSuccess)
            {
                item?.Invoke();
            }
        }
        public void PlayerOnPlatform()
        {
            cinemachineCameras[0].SetActive(false);
            cinemachineCameras[1].SetActive(true);
        }
        public void LevelFailed()
        {
            GameManager.instance.LevelFailed();
            Debug.Log("Level Failed");
        }

    }
}
