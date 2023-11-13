using UnityEngine;

namespace GameTwo
{
    public class Stack : MonoBehaviour, IPoolObject
    {
        [SerializeField] private MeshRenderer meshRenderer;
        private MaterialPropertyBlock mpb;
        public void Init()
        {
            mpb = new MaterialPropertyBlock();
        }

        public void Reset()
        {
            gameObject.SetActive(true);
        }
        public void OpenStack(ref Color color)
        {
            Reset();
            mpb.SetColor(StackManager.ColorID,color);
            meshRenderer.SetPropertyBlock(mpb);
        }

    }
}
