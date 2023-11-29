using UnityEngine;
using UnityEngine.SceneManagement;

namespace BH.Inputs
{
    public class MainMenuBtnManager : MonoBehaviour
    {
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
            Debug.Log("Credits");
        }
    }
}
