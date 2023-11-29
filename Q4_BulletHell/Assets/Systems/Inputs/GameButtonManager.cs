using UnityEngine;
using UnityEngine.SceneManagement;
using BH.Game;

namespace BH.Inputs
{
    public class GameButtonManager : MonoBehaviour
    {
        public void OnRestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void OnBackToMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void OnContinueGame()
        {
            GameManager.instance.PlayGame();
        }
    }
}