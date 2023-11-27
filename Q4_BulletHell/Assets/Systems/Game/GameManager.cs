using BH.MenusUI;
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
        }

        public void PlayerDie()
        {
            //Active UI
            m_defeatUI.SetActive(true);

            m_victoryUI.SetActive(false);
            m_pauseUI.SetActive(false);
            m_startingUI.gameObject.SetActive(false);
        }

        public void PauseGame()
        {
            //Active UI
            m_pauseUI.SetActive(true);

            m_victoryUI.SetActive(false);
            m_defeatUI.SetActive(false);
            m_startingUI.gameObject.SetActive(false);

            //pause game
            Time.timeScale = 0f;
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

            Time.timeScale = 1f;

            //Remove UI
            m_startingUI.gameObject.SetActive(false);
        }
    }
}
