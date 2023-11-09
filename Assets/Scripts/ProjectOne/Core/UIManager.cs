using UnityEngine;
using TMPro;
namespace GameOne
{
    public class UIManager : MonoBehaviour
    {
        private GridManager gridManager;
        private int matchCount;
        [SerializeField] private TextMeshProUGUI matchCountText;
        public void Init(GridManager gridManager)
        {
            this.gridManager = gridManager;
            matchCount = 0;
        }

        public void IncreaseMatchCount()
        {
            matchCount++;
            matchCountText.SetText(matchCount.ToString());
        }
        public void OnRebuild(int size)
        {
            gridManager.ReBuildGrid(size);
        }


    }
}
