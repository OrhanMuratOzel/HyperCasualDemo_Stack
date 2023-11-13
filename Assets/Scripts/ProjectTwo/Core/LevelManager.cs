using System;
using UnityEngine;
using System.Collections.Generic;
namespace GameTwo
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private LevelDataScriptable[] levelData;
        private StackManager stackManager;
        private int currentLevel = 0;

        private List<Action> resetActions;


        public void Init(StackManager stackManager,Action UIControllerResetAction, Action PlayerControllerResetAction)
        {
            this.stackManager = stackManager;
            resetActions = new();
            resetActions.Add(UIControllerResetAction);
            resetActions.Add(PlayerControllerResetAction);
        }


        public void Reset()
        {
            foreach (var item in resetActions)
            {
                item?.Invoke();
            }
        }
        public void LevelStart()
        {
            stackManager.OnLevelStart(levelData[currentLevel].stackCount);
            GameManager.instance.LevelStart();
        }

        public void OnLevelSucces()
        {

        }
        public void OnLevelFailed()
        {
            GameManager.instance.LevelFailed();
            Debug.Log("Level Failed");
        }

    }
}
