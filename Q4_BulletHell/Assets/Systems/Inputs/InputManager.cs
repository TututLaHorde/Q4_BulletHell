using UnityEngine;
using UnityEngine.InputSystem;
using BH.Player;

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
            Vector3 targetPos = m_cam.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
            m_playerController.MoveTargetChange(targetPos);
        }
    }
}
