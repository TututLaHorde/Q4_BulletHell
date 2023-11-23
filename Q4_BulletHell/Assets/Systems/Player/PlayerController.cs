using UnityEngine;

namespace BH.Player
{
    [RequireComponent(typeof(PlayerMovement), typeof(PlayerShoot))]

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private EnemiesManager m_enemiesManager;
        public Transform m_enemyTrs { get; private set; }

        private PlayerMovement m_playerMovement;
        private Transform m_ownTrs;

        private void Start()
        {
            m_playerMovement = GetComponent<PlayerMovement>();
            m_ownTrs = transform;

            SetClosestEnemyTrs();
        }

        private void FixedUpdate()
        {
            SetClosestEnemyTrs();
        }

        public void MoveTargetChange(Vector3 targetPos)
        {
            m_playerMovement.m_targetPos = targetPos;
        }

        private void SetClosestEnemyTrs()
        {
            EnemyController enemy = m_enemiesManager.GetClosestEnemy(m_ownTrs.position);
            if (enemy != null)
            {
                m_enemyTrs = enemy.transform;
            }
            else
            {
                m_enemyTrs = m_ownTrs;
            }
        }
    }
}

