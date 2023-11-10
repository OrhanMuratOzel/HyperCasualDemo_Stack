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
            //- take start size than arrange it
        }
        public void GridSizeChanged(int size)
        {
            cam.fieldOfView = baseFow + (size*increaseAmount);
        }
    }
}