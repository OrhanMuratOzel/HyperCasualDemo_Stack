using UnityEngine;
using TMPro;
namespace GameOne
{
    public class UIController : MonoBehaviour
    {
        private static int MAXRebuildSize=15;
        private GridManager gridManager;
        private int matchCount;
        private int rebuildSize;
        [SerializeField] private TMP_InputField rebuildText;
        [SerializeField] private TextMeshProUGUI matchCountText;
        public void Init(GridManager gridManager)
        {
            this.gridManager = gridManager;
            matchCount = 0;
            rebuildSize = int.Parse(rebuildText.text);
        }

        public void IncreaseMatchCount()
        {
            matchCount++;
            matchCountText.SetText(matchCount.ToString());
        }
        public void OnEndEdit(string text)
        {
            int.TryParse(text, out rebuildSize);
            if(rebuildSize> MAXRebuildSize)
                rebuildSize = MAXRebuildSize;
            rebuildText.text =rebuildSize.ToString();
        }
        public void OnRebuild() 
        { 
            gridManager.ReBuildGrid(rebuildSize);
        }


    }
}
