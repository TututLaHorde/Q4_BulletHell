using UnityEngine;

namespace BH.Music
{
    public class SfxManager : MonoBehaviour
    {
        public static SfxManager instance;

        private void Awake()
        {
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
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.Play();
            Destroy(source, clip.length);
        }
    }
}

