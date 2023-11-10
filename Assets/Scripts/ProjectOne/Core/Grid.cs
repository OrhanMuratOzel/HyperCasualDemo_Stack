using UnityEngine;

namespace GameOne
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] private bool isOccupied;
        [SerializeField] private GameObject xObject;
        private GridManager gridManager;
        private int gridIndex;
        private bool isInSearch;
        public int GetIndex => gridIndex;

        #region Set | Get
        public bool IsOccupied => isOccupied;
        public bool IsInSearch
        {
            get
            {
                return isInSearch;
            }
            set
            {
                isInSearch = value;
            }
        }
        #endregion

        #region Init | Close 
        public void Init(GridManager gridManager)
        {
            this.gridManager = gridManager;
        }
        public void SetGridPosition(int index) =>gridIndex = index;
        public void ClearGrid()
        {
            isOccupied = false;
            isInSearch= false;
            xObject.SetActive(false);
        }
        #endregion
        private void CheckGrid()
        {
            gridManager.CheckGrid(this);
        }
        public void OnMouseDown()
        {
            if (isOccupied)
                return;
            isOccupied = true;
            xObject.SetActive(true);
            CheckGrid();
        }
    }
}