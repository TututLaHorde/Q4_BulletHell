using UnityEngine;
using BH.Enemies;

namespace BH.Player
{
    [RequireComponent(typeof(PlayerMovement), typeof(PlayerShoot))]

    public class PlayerController : MonoBehaviour
    {
        [Header("Enemies")]
        [SerializeField] private EnemiesManager m_enemiesManager;
        public Transform m_enemyTrs { get; private set; }

        [Header("Explosion")]
        [SerializeField] private GameObject m_explosionParticule;
        [SerializeField] private float m_explosionTime;

        //own component
        private PlayerMovement m_playerMovement;
        private Transform m_ownTrs;

        public bool m_isCanAct { get; private set; } = true;

    /*-------------------------------------------------------------------*/

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

        /*-------------------------------------------------------------------*/

        public void MoveTargetChange(Vector3 targetPos)
        {
            m_playerMovement.m_targetPos = targetPos;                
        }

        public float DeathExplosion()
        {
            m_isCanAct = false;
            m_explosionParticule.SetActive(true);

            return m_explosionTime;
        }

        /*-------------------------------------------------------------------*/

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

