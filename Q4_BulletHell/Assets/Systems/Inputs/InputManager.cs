using UnityEngine;
using UnityEngine.InputSystem;
using BH.Player;
using BH.Game;

namespace BH.Inputs
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerController m_playerController;
        private Camera m_cam;

        /*-------------------------------------------------------------------*/

        private void Start()
        {
            m_cam = Camera.main;
        }

        /*-------------------------------------------------------------------*/

        public void OnMoveTargetChange(InputAction.CallbackContext ctx)
        {
            m_playerController.MoveTargetChange(ctx.ReadValue<Vector2>());
        }

        public void OnPauseGame(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                GameManager.instance.PauseGame();
            }
        }
    }
}
