using UnityEngine;
using System.Collections;
namespace GameTwo
{
    public class Diamond : MonoBehaviour, ICollectable, IPoolObject
    {
        [SerializeField] private new ParticleSystem particleSystem;
        [SerializeField] private GameObject model;
        private new Collider collider;
        public void Collected()
        {
            particleSystem.Play();
            collider.enabled = false;
            model.SetActive(false);
        }
        public void Init()
        {
            collider = GetComponent<Collider>();
        }
        public void Reset()
        {
            particleSystem.Stop();
            model.SetActive(true);
            collider.enabled = true;
        }
        private void OnTriggerEnter(Collider other)
        {
            Collected();
        }
    }
}