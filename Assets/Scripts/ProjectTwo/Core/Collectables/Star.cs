using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameTwo
{
    public class Star : MonoBehaviour, ICollectable, IPoolObject
    {
        [SerializeField] private ParticleSystem particleSystem;
        private Collider collider;
        public void Collected()
        {
            particleSystem.Play();
            collider.enabled = false;
            CloseFunctions();
        }
        public void Init()
        {
            collider = GetComponent<Collider>();
        }
        public void Reset()
        {
            particleSystem.Stop();
            collider.enabled = true;
        }
        private IEnumerator CloseFunctions()
        {
            yield return null;
            gameObject.SetActive(false);
        }
        private void OnTriggerEnter(Collider other)
        {
            Collected();
        }
    }
}
