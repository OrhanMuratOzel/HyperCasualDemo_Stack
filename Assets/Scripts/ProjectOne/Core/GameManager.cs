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
            gridManager.Init(pool, cameraController, uiController,gridSize);
            uiController.Init(gridManager);
            cameraController.Init();
        }
        private void OnValidate()
        {
            if (Application.isPlaying)
                return;
            gridManager.Init(pool, cameraController, uiController,gridSize);
            cameraController.Init();
            gridManager.EditorBuildGrid();
        }
    }
}