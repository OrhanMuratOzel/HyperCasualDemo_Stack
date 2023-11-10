using UnityEngine;
namespace GameOne
{
    public class CameraController : MonoBehaviour
    {
        private Camera cam;
        [SerializeField] private float baseFow;
        [SerializeField] private float increaseAmount;
        public void Init()
        {
            cam = Camera.main;
        }
        public void GridSizeChanged(int size)
        {
            cam.fieldOfView = baseFow + (size*increaseAmount);
        }
    }
}