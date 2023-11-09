using UnityEngine;

namespace GameOne
{
    public class Grid : MonoBehaviour
    {
        private bool isOccupied;
        [SerializeField] private GameObject xObject;
        private GridManager gridManager;

        #region Set | Get
        public bool IsOccupied => isOccupied;
        #endregion

        #region Init | Close 
        public void Init(GridManager gridManager)
        {
            this.gridManager = gridManager;
        }
        public void ClearGrid()
        {
            xObject.SetActive(false);
        }
        #endregion

        private void CheckGrid()
        {
            gridManager.CheckGrid();
        }


        public void OnMouseDown()
        {
            xObject.SetActive(true);
            CheckGrid();
        }
    }
}