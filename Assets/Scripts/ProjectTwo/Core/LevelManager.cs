using System;
using UnityEngine;
using System.Collections.Generic;
namespace GameTwo
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private LevelDesigner levelDesigner;
        [SerializeField] private LevelDataScriptable[] levelData;
        [SerializeField] private GameObject[] cinemachineCameras;
        private StackManager stackManager;
        private int currentLevel = 0;
        private List<Action> resetActions;

        public void Init(StackManager stackManager, List<Action> resetActions)
        {
            this.stackManager = stackManager;
            this.resetActions = resetActions;
        }
        public void Reset()
        {
            foreach (var item in resetActions)
            {
                item?.Invoke();
            }
            cinemachineCameras[0].SetActive(true);
            cinemachineCameras[1].SetActive(false);
            var currentLevelData = levelData[currentLevel % levelData.Length];
            levelDesigner.LoadLevel(currentLevelData);
        }
        public void LevelStart()
        {
            var currentLevelData = levelData[currentLevel % levelData.Length];
            stackManager.OnLevelStart(currentLevelData.stackCount);
            GameManager.instance.LevelStart();
        }
        public void LevelSucces()
        {
            GameManager.instance.LevelSuccess();
            currentLevel++;
        }
        public void PlayerOnPlatform()
        {
            cinemachineCameras[0].SetActive(false);
            cinemachineCameras[1].SetActive(true);
        }
        public void LevelFailed()
        {
            GameManager.instance.LevelFailed();
        }
    }
}
