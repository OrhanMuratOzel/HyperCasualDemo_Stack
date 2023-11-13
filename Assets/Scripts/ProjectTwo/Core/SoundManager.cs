using UnityEngine;
namespace GameTwo
{
    public class SoundManager : Singleton<SoundManager>
    {
        [SerializeField] private AudioClip selectedSound;
        [SerializeField] private float pitchIncreaseAmount = 0.1f;
        [SerializeField] private float maxPitch;
        [SerializeField] private float initialPitch;
        private AudioSource audioSource;
        public void Init()
        {
            audioSource = GetComponent<AudioSource>();
            initialPitch = audioSource.pitch;
        }
        public void Reset()
        {
            audioSource.pitch = initialPitch;
        }
        public void PerfectTap()
        {
            audioSource.clip = selectedSound;
            if(audioSource.pitch < maxPitch)
                audioSource.pitch += pitchIncreaseAmount;
            audioSource.Play();
        }
        public void NormalTap()
        {
            StrikeBroke();
            audioSource.clip = selectedSound;
            audioSource.Play();
        }
        public void StrikeBroke()
        {
            audioSource.pitch = initialPitch;
        }
    }
}