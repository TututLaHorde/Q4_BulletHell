using UnityEngine;

namespace BH.Music
{
    public class SfxManager : MonoBehaviour
    {
        public static SfxManager instance;

        [SerializeField] private AudioSource m_source;

        private void Awake()
        {
            //singelton
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.Log("There is two SfxManager");
                Destroy(this);
                return;
            }
        }

        public void PlaySfx(AudioClip clip)
        {
            if (clip != null)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.clip = clip;
                source.Play();
                Destroy(source, clip.length);
            }
        }

        public void PlayMultipleSfx(AudioClip clip, float volume = 1f)
        {
            if (clip != null && m_source != null)
            {
                m_source.PlayOneShot(clip, volume);
            }
        }
    }
}

