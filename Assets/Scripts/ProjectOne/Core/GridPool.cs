using System.Collections.Generic;
using UnityEngine;
namespace GameOne
{
    public class GridPool : MonoBehaviour
    {
        private Queue<Grid> dynamicPool;
        private GridManager gridManager;
        [SerializeField] private List<Grid> staticPoolList;
        [SerializeField] private Grid gridPrefab;

        #region Initiliaze
        public void Init(GridManager gridManager)
        {
            this.gridManager = gridManager;
            dynamicPool = new Queue<Grid>();
            AddStaticListToQueue();
        }
        private void AddStaticListToQueue()
        {
            for (var i = 0; i < staticPoolList.Count; i++)
            {
                staticPoolList[i].Init(gridManager);
                BackToPool(staticPoolList[i]);
            }
        }
        #endregion

        #region Get From Pool
        public Grid GetFromPool()
        {
            return dynamicPool.Count > 0 ? dynamicPool.Dequeue() : CreateGrid();
        }
        public List<Grid> GetWantedSizeGrid(int size)
        {
            var returnList = new List<Grid>();
            for (int i = 0; i < size; i++)
            {
                var gridRefHolder = GetFromPool();
                Debug.Log(dynamicPool.Count);
                gridRefHolder.gameObject.SetActive(true);
                returnList.Add(gridRefHolder);
            }
            return returnList;
        }
        private Grid CreateGrid()
        {
            var grid = Instantiate(gridPrefab, transform);
            grid.Init(gridManager);
            return grid;
        }
        #endregion

        #region Back To Pool
        public void BackToPool(Grid grid)
        {
            grid.ClearGrid();
            grid.gameObject.SetActive(false);
            dynamicPool.Enqueue(grid);
        }
        #endregion

#if UNITY_EDITOR
        #region Editor
        public List<Grid> EditorGetWantedSizeGrid(int capacity, int size)
        {
            var returnList = new List<Grid>();
            var wantedAmount = size - staticPoolList.Count;
            if (wantedAmount <= 0)
            {
                for (int i = capacity; i < size; i++)
                {
                    returnList.Add(staticPoolList[i]);
                    staticPoolList[i].gameObject.SetActive(true);
                }
                return returnList;
            }

            for (int i = 0; i < wantedAmount; i++)
            {
                var gridObj = Instantiate(gridPrefab,transform);
                gridObj.gameObject.SetActive(true);
                staticPoolList.Add(gridObj);
                returnList.Add(gridObj);
            }

            return returnList;
        }
        #endregion
#endif
    }
}
