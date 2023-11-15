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
        void Update()
        {
            if (GameManager.instance.GetGameState == GameStates.Idle)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    LevelManager.instance.LevelStart();
                }
                return;
            }
            if (GameManager.instance.GetGameState != GameStates.Playing)
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
