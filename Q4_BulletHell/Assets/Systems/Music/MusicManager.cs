using System.Collections.Generic;
using UnityEngine;

namespace BH.Music
{
    [RequireComponent (typeof (AudioSource))]

    public class MusicManager : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> m_musics = new();
        private int m_musicIndex;
        private AudioSource m_source;

        private void Start()
        {
            m_source = GetComponent<AudioSource>();

            //init music
            m_musicIndex = Random.Range(0, m_musics.Count);
            m_source.clip = m_musics[m_musicIndex];
            m_source.Play();
        }

        private void Update()
        {
            if (!m_source.isPlaying)
            {
                ChangeMusic();
            }
        }

        private void ChangeMusic()
        {
            //loop the same music
            if (m_musics.Count == 1)
            {
                m_source.clip = m_musics[0];
                m_source.Play();
            }
            //choose rando music
            else if (m_musics.Count > 1)
            {
                Debug.Log("yep");
                int rnd = Random.Range(0, m_musics.Count - 1);
                m_musicIndex = rnd >= m_musicIndex ? rnd + 1 : rnd;

                m_source.clip = m_musics[m_musicIndex];
                m_source.Play();
            }
        }
    }
}
