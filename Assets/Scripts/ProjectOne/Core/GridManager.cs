using UnityEngine;
using System.Collections.Generic;
namespace GameOne
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private List<Grid> activeGrids;
        [SerializeField] private float evenOffSet;

        private GridPool pool;
        private UIController uiController;
        private CameraController camController;
        private int gridSize = 0;
        private int gridLength = 0;

        public void Init(GridPool pool, CameraController camController, UIController uiController, int gridSize)
        {
            this.pool = pool;
            this.uiController = uiController;
            this.gridSize = gridSize;
            this.camController = camController;
            gridLength = gridSize * gridSize;
            activeGrids = pool.GetWantedSizeGrid(gridLength);
        }
        public void ReBuildGrid(int size)
        {
            if (size < 2)
                return;
            gridSize = size;
            gridLength = gridSize * gridSize;
            var wantedGridSize = gridLength - activeGrids.Count;
            if (wantedGridSize>0)
            {
                activeGrids.AddRange(pool.GetWantedSizeGrid(wantedGridSize));
            }
            else
            {
                DeactivateUnUsedGrids(gridLength);
            }
            ClearGrids();
            PositionGrid();
            camController.GridSizeChanged(size);
        }
        private void PositionGrid()
        {
            var tmpVector = Vector3.zero;
            var isEven = gridSize % 2 == 0;
            var offsetX = (gridSize - 1) * .5f;
            var offsetY = (gridSize - 1) * .5f;

            for (var x = 0; x < gridSize; x++)
            {
                for (var y = 0; y < gridSize; y++)
                {

                    tmpVector.x = x * 1 - offsetX;
                    tmpVector.z = y * 1 - offsetY;
                    if (isEven)
                        tmpVector.x += evenOffSet;

                    activeGrids[y * gridSize + x].transform.position = tmpVector;
                    activeGrids[y * gridSize + x].SetGridPosition(y * gridSize + x);
                }
            }
        }
        private void DeactivateUnUsedGrids(int from)
        {
            for (var i = from; i < activeGrids.Count; i++)
            {
                pool.BackToPool(activeGrids[i]);
            }
            activeGrids.RemoveRange(from, activeGrids.Count - from);
        }
        private void ClearGrids()
        {
            for (var i = 0; i < activeGrids.Count; i++)
            {
                activeGrids[i].ClearGrid();
            }
        }


        #region Match Checking
        public void CheckGrid(Grid grid)
        {
            if (!HasAdjescent(grid.GetIndex))
                return;

            var adjescentGrids = new List<Grid>();
            adjescentGrids = FindAdjescentGrids(grid.GetIndex);
            if (adjescentGrids.Count >=3)
                OnMatch(adjescentGrids);
            else
            {
                Debug.Log(adjescentGrids.Count);
                foreach (var item in adjescentGrids)
                {
                    item.IsInSearch = false;
                }
            }

        }
        private bool HasAdjescent(int index)
        {
            var returnVal = false;
            var gridLength = gridSize * gridSize;

            if (index%gridSize!=0)
            {
              //  Debug.Log("Sol gidebilir");
                returnVal = true;
              //  returnVal = activeGrids[index - 1].IsOccupied;
            }

            if (!returnVal && (index+1) % gridSize != 0)
            {
              //  Debug.Log("sað gidebilir");
                returnVal = true;
                //returnVal = activeGrids[index +1].IsOccupied;
            }

            if (!returnVal && index+gridSize < gridLength)
            {
             //   Debug.Log("yukarý gidebilir");
                returnVal = true;
               // returnVal = activeGrids[index + gridSize].IsOccupied;
            }

            if (!returnVal && index-gridSize >= 0)
            {
              //  Debug.Log("aþaðý gidebilir");
                returnVal = true;
              //  returnVal = activeGrids[index - gridSize].IsOccupied;
            }

            return returnVal;
        }
        private List<Grid> FindAdjescentGrids(int index)
        {
            var returnGrid = new List<Grid>();
            activeGrids[index].IsInSearch = true;
            returnGrid.Add(activeGrids[index]);

            if (index % gridSize != 0 && activeGrids[index - 1].IsOccupied && !activeGrids[index - 1].IsInSearch)
            {
                returnGrid.AddRange(FindAdjescentGrids(index-1));
            }
            if ((index + 1) % gridSize != 0 && activeGrids[index + 1].IsOccupied && !activeGrids[index + 1].IsInSearch)
            {
                returnGrid.AddRange(FindAdjescentGrids(index +1));
            }

            if (index + gridSize < gridLength && activeGrids[index + gridSize].IsOccupied && !activeGrids[index + gridSize].IsInSearch)
            {
                returnGrid.AddRange(FindAdjescentGrids(index + gridSize));
            }

            if (index - gridSize >= 0&& activeGrids[index - gridSize].IsOccupied && !activeGrids[index - gridSize].IsInSearch)
            {
                returnGrid.AddRange(FindAdjescentGrids(index - gridSize));
            }
            return returnGrid;
        }
        private void OnMatch(List<Grid> grid)
        {
            Debug.Log(grid.Count);
            foreach (var item in grid)
            {
                item.ClearGrid();
            }
            uiController.IncreaseMatchCount();
        }
        #endregion


#if UNITY_EDITOR

        #region Editor
        public void EditorBuildGrid()
        {
            if (gridSize < 2)
                return;
            gridLength = gridSize * gridSize;
            if (activeGrids.Count < gridLength)
            {
                activeGrids.AddRange(pool.EditorGetWantedSizeGrid(activeGrids.Count, gridLength));
            }
            else
            {
                EditorDeactivateUnUsedGrids(gridLength);
            }
            ClearGrids();
            PositionGrid();
            camController.GridSizeChanged(gridSize);
        }
        private void EditorDeactivateUnUsedGrids(int from)
        {
            for (var i = from; i < activeGrids.Count; i++)
            {
                activeGrids[i].gameObject.SetActive(false);
            }
            activeGrids.RemoveRange(from, activeGrids.Count - from);
        }
        #endregion
#endif
    }
}
