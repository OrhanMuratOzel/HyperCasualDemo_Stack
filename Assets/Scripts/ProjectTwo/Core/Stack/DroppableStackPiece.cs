using UnityEngine;
namespace GameTwo
{
    public class DroppableStackPiece : MonoBehaviour, IPoolObject
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Rigidbody rb;
        private MaterialPropertyBlock mpb;

        public void Init()
        {
            mpb = new MaterialPropertyBlock();
        }

        public void Reset()
        {
            rb.velocity= Vector3.zero;
            rb.useGravity = false;
            gameObject.SetActive(true);
        }
        public void ActivatePiece(ref Vector3 scale,ref Vector3 spawnPoint, ref Color color)
        {
            Reset();

            mpb.SetColor(StackManager.ColorID, color);
            meshRenderer.SetPropertyBlock(mpb);
            transform.localScale = scale;
            transform.localPosition= spawnPoint;
            rb.useGravity = true;
        }
    }
}
