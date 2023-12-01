using BH.MenusUI;
using BH.Music;
using BH.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;

namespace BH.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        [Header("Game UI")]
        [SerializeField] private GameObject m_victoryUI;
        [SerializeField] private GameObject m_defeatUI;
        [SerializeField] private GameObject m_pauseUI;
        [SerializeField] private StartingGameCount m_startingUI;

        [Header("Pause")]
        [SerializeField] private float m_timeAfterPause;

        [Header("Others")]
        [SerializeField] private PlayerController m_player;

        [Header("Audio")]
        [SerializeField] private float m_musicStopTime = 1.5f;
        [SerializeField] private AudioClip m_clipWin;
        [SerializeField] private AudioClip m_clipLose;

        /*-------------------------------------------------------------------*/

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                Debug.Log("There is two GameManager");
                Destroy(this);
                return;
            }

            Assert.IsNotNull(m_victoryUI);
            Assert.IsNotNull(m_defeatUI);
            Assert.IsNotNull(m_startingUI);
            Assert.IsNotNull(m_pauseUI);
        }

        private void Start()
        {
            PauseGame();
            PlayGame();
        }

        /*-------------------------------------------------------------------*/

        public void LastEnemyDie()
        {
            //Active UI
            m_victoryUI.SetActive(true);

            m_defeatUI.SetActive(false);
            m_pauseUI.SetActive(false);
            m_startingUI.gameObject.SetActive(false);

            //play sound
            SfxManager.instance.PlaySfx(m_clipWin);
            MusicManager.instance.SmoothStopMusic(0.2f);

            //pause game
            SetPause(true);
        }

        public void PlayerDie()
        {
            //if (m_player.m_isAlive)
            //{
            //    StartCoroutine(PlayerDeath());
            //}
        }

        public void PauseGame()
        {
            //Active UI
            if (Time.timeScale != 0f)
            {
                m_pauseUI.SetActive(true);

                //pause game
                SetPause(true);
            }
        }

        public void PlayGame()
        {
            //Active UI
            m_startingUI.gameObject.SetActive(true);

            m_victoryUI.SetActive(false);
            m_defeatUI.SetActive(false);
            m_pauseUI.SetActive(false);

            //Stop pause
            StartCoroutine(WaitBeforePlaying());
        }

        /*-------------------------------------------------------------------*/

        private IEnumerator WaitBeforePlaying()
        {
            float currentTime = m_timeAfterPause;

            while (currentTime > 0)
            {
                currentTime -= Time.unscaledDeltaTime;
                m_startingUI.SetStartingCount(currentTime);

                yield return null;
            }

            //play game
            SetPause(false);

            //Remove UI
            m_startingUI.gameObject.SetActive(false);
        }

        private IEnumerator PlayerDeath()
        {
            yield return new WaitForSeconds(m_player.DeathExplosion());

            //Active UI
            m_defeatUI.SetActive(true);

            m_victoryUI.SetActive(false);
            m_pauseUI.SetActive(false);
            m_startingUI.gameObject.SetActive(false);

            //stop music
            MusicManager.instance.SmoothStopMusic(m_musicStopTime);

            //pause game
            SetPause(true);

            //play sound
            yield return new WaitForSecondsRealtime(m_musicStopTime);
            SfxManager.instance.PlaySfx(m_clipLose);
        }

        private void SetPause(bool pauseGame)
        {
            if (pauseGame)
            {
                Time.timeScale = 0f;
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1f;
                Cursor.visible = false;
            }
        }
    }
}
