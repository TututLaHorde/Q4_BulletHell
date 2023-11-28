using UnityEngine;
using BH.Enemies;
using BH.Game;
using BH.Music;

namespace BH.Player
{
    [RequireComponent(typeof(PlayerMovement), typeof(PlayerShoot))]

    public class PlayerController : MonoBehaviour
    {
        public static PlayerController instance;

        [Header("Enemies")]
        [SerializeField] private EnemiesManager m_enemiesManager;
        public Transform m_enemyTrs { get; private set; }

        [Header("Explosion")]
        [SerializeField] private GameObject m_explosionParticule;
        [SerializeField] private float m_explosionTime;
        [SerializeField] private AudioClip m_explosionSound;
        [SerializeField] private float m_shakeAmount;

        //own component
        private PlayerMovement m_playerMovement;
        private Transform m_ownTrs;

        public bool m_isAlive { get; private set; } = true;

        /*-------------------------------------------------------------------*/

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.Log("There is two PlayerController");
                Destroy(this);
                return;
            }
        }

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
            m_isAlive = false;

            SfxManager.instance.PlaySfx(m_explosionSound);
            m_explosionParticule.SetActive(true);
            ScreenShake.instance.m_amount += m_shakeAmount;

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

