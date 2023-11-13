using UnityEngine;
using TMPro;
namespace GameTwo
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tapText;
        public void Reset()
        {
            tapText.gameObject.SetActive(true);
        }
        public void LevelStart()
        {
            tapText.gameObject.SetActive(false);
        }
    }
}
