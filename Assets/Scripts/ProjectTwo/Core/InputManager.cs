using UnityEngine;
namespace GameTwo
{
    public class InputManager : MonoBehaviour
    {
        private StackManager stackManager;
        public void Init(StackManager stackManager)
        {
            this.stackManager = stackManager;
        }
        public void Reset()
        {

        }
        public void OnLevelStart()
        {

        }
        void Update()
        {
            if (GameManager.instance.GetGameStates == GameStates.Idle)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    LevelManager.instance.LevelStart();
                }
                return;
            }
            if (GameManager.instance.GetGameStates != GameStates.Playing)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                OnMouseButtonDown();
            }
        }

        private void OnMouseButtonDown()
        {
            stackManager.StopStackObject();
        }

    }
}
