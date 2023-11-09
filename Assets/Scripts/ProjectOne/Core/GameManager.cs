using UnityEngine;
namespace GameOne
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GridManager gridManager;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private GridPool pool;
        [SerializeField][Range(0, 10)] private int gridSize;
        void Start()
        {
            gridManager.Init(pool, uiManager,gridSize);
            uiManager.Init(gridManager);
            pool.Init(gridManager);
        }
        private void OnValidate()
        {
            gridManager.Init(pool, uiManager,gridSize);
            gridManager.EditorBuildGrid();

        }
    }

}