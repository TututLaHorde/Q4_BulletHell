using UnityEngine;
using UnityEngine.SceneManagement;

namespace BH.Inputs
{
    public class MainMenuBtnManager : MonoBehaviour
    {
        [SerializeField] private Animator m_AnimMainMenu;

        private void Awake()
        {
            Time.timeScale = 1f;
        }

        public void OnPlayGame()
        {
            SceneManager.LoadScene("SampleScene");
        }

        public void OnQuit()
        {
            Application.Quit();
            Debug.Log("Quit");
        }

        public void OnCredits()
        {
            m_AnimMainMenu.SetTrigger("MainToCredit");
        }

        public void OnBack()
        {
            m_AnimMainMenu.SetTrigger("CreditToMain");
        }
    }
}
