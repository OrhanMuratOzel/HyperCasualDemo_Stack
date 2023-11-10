using UnityEngine;
namespace GameOne
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GridManager gridManager;
        [SerializeField] private UIController uiController;
        [SerializeField] private GridPool pool;
        [SerializeField] private CameraController cameraController;
        [SerializeField][Range(0, 10)] private int gridSize;
        void Start()
        {
            pool.Init(gridManager);
            cameraController.Init();
            gridManager.Init(pool, cameraController, uiController,gridSize);
            uiController.Init(gridManager);
        }
        private void OnValidate()
        {
            if (Application.isPlaying)
                return;
            cameraController.Init();
            gridManager.EditorBuildGrid(pool, cameraController, gridSize);
        }
    }
}