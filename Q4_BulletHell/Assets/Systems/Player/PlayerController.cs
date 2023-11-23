using UnityEngine;

namespace BH.Player
{
    [RequireComponent(typeof(PlayerMovement), typeof(PlayerShoot))]

    public class PlayerController : MonoBehaviour
    {
        public Transform m_enemyTrs;

        private PlayerMovement m_playerMovement;
        private PlayerShoot m_playerShoot;

        private void Start()
        {
            m_playerMovement = GetComponent<PlayerMovement>();
            m_playerShoot = GetComponent<PlayerShoot>();
        }

        public void MoveTargetChange(Vector3 targetPos)
        {
            m_playerMovement.m_targetPos = targetPos;
        }
    }
}

