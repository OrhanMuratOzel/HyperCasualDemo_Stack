using UnityEngine;
using System.Collections.Generic;
namespace GameOne
{
    public class GridManager : MonoBehaviour
    {
        private GridPool pool;
        private UIManager uiManager;
        private int gridSize = 0;
        [SerializeField] private List<Grid> activeGrids;
        [SerializeField] private GridPositionParameters gridPositionParameters;
        public void Init(GridPool pool,UIManager uiManager, int gridSize)
        {
            this.pool = pool;
            this.uiManager = uiManager;
            this.gridSize = gridSize;
            activeGrids = pool.GetActiveStaticGrids();
        }

        public void ReBuildGrid(int size)
        {
            var newGridSize = size * size;
            Debug.Log(activeGrids.Count + " "+ newGridSize);
            if (activeGrids.Count<newGridSize)
            {
                //-listeyi arttýr
                var poolList = pool.GetWantedSizeGrid(activeGrids.Count, newGridSize);
                Debug.Log(poolList.Count);
                activeGrids.AddRange(poolList);
            }
            else
            {
                Debug.Log("Deactive ");
                DeactivateUnUsedGrids(newGridSize);
                //- boyut þu an iyi
            }
            ClearGrids();
            PositionGrid();
        }
        private void PositionGrid()
        {
            var tmpVector = Vector3.zero;
            float offsetX = (gridSize - 1) * 1 / 2;
            float offsetY = (gridSize - 1) * 1 / 2;
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                   // tmpVector.x = gridPositionParameters.xOffSet * x;
                   // tmpVector.z = gridPositionParameters.zOffset * i;
                    Vector3 cellPosition = new Vector3(x * 1 - offsetX,
                        0, y * 1 - offsetY) + transform.position;
                   activeGrids[y*gridSize+ x].transform.position =cellPosition;


                    //gridPositionParameters
                }
            }
        }
        private void DeactivateUnUsedGrids(int from)
        {
            for (int i = from; i < activeGrids.Count; i++)
            {
                pool.BackToPool(activeGrids[i]);
            }
            activeGrids.RemoveRange(from, activeGrids.Count - from);
        }
        private void ClearGrids()
        {
            for (int i = 0; i < activeGrids.Count; i++)
            {
                activeGrids[i].ClearGrid();
            }
        }
        public bool CheckGrid()
        {
            return false;
        }
        private void OnMatch()
        {

        }


#if UNITY_EDITOR

        #region Editor
        public void EditorBuildGrid()
        {
            var newGridSize = gridSize * gridSize;
            Debug.Log(activeGrids.Count + " " + newGridSize);
            if (activeGrids.Count < newGridSize)
            {
                activeGrids.AddRange(pool.EditorGetWantedSizeGrid(activeGrids.Count, newGridSize));
            }
            else
            {
                Debug.Log("Deactive ");
                EditorDeactivateUnUsedGrids(newGridSize);
            }
            ClearGrids();
            PositionGrid();
        }
        private void EditorDeactivateUnUsedGrids(int from)
        {
            for (int i = from; i < activeGrids.Count; i++)
            {
                activeGrids[i].gameObject.SetActive(false);
            }
            activeGrids.RemoveRange(from, activeGrids.Count - from);
        }
        #endregion
#endif
        [System.Serializable] 
        class GridPositionParameters
        {
            public float xOffSet;
            public float zOffset;
        }
    }
}
