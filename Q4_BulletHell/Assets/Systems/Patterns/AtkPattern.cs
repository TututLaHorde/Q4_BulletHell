using BH.Bullets;
using BH.Enemies;
using BH.Player;

using UnityEngine;
using UnityEngine.Assertions;

namespace BH.Patterns
{
    public abstract class AtkPattern : MonoBehaviour
    {
        [Header("Global")]
        [SerializeField] private GameObject m_pbBulletManager;
        public float m_timeBeforeNextPattern;
        protected BulletManager m_bulletManager;

        protected Transform m_shooterTrs;
        protected Transform m_playerTrs;

        [Header("Bursts")]
        [SerializeField] protected int m_burstNb;
        [SerializeField] protected float m_timeBetweenBurst;

        /*-------------------------------------------------------------------*/

        private void Start()
        {
            //get player trs
            m_playerTrs = PlayerController.instance.transform;
            Assert.IsNotNull(m_playerTrs);

            //get bullet manager
            LinkWithBulletManager();
        }

        /*-------------------------------------------------------------------*/

        public abstract void StartAtkPattern(EnemyController enemy);
        protected abstract void DoOneBurst(int burstIndex);

        /*-------------------------------------------------------------------*/

        private void LinkWithBulletManager()
        {
            if (m_pbBulletManager != null)
            {
                //try find an instance of the bullet Manager prefab
                GameObject pbBulletManager = GameObject.Find(m_pbBulletManager.name);
                if (pbBulletManager != null && pbBulletManager.TryGetComponent(out BulletManager bulletManager))
                {
                    //pattern can shoot
                    m_bulletManager = bulletManager;
                }
            }
        }
    }
}
