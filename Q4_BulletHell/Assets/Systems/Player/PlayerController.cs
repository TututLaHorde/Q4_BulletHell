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
        [SerializeField] private float m_shakeAmount;

        [SerializeField] private AudioClip m_clipExplosion;

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
            //get own component
            m_playerMovement = GetComponent<PlayerMovement>();
            m_ownTrs = transform;

            //find the closest enemy
            SetClosestEnemyTrs();
        }

        private void FixedUpdate()
        {
            //find the closest enemy
            SetClosestEnemyTrs();
        }

        /*-------------------------------------------------------------------*/

        public void MoveTargetChange(Vector2 targetPos)
        {
            m_playerMovement.m_mousePos = targetPos;                
        }

        public float DeathExplosion()
        {
            m_isAlive = false;

            //sfx, animation, screen shake
            SfxManager.instance.PlaySfx(m_clipExplosion);
            m_explosionParticule.SetActive(true);
            //ScreenShake.instance.m_amount += m_shakeAmount;

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

