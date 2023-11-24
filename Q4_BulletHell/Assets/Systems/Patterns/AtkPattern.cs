using BH.Bullets;
using BH.Enemies;
using UnityEngine;

namespace BH.Patterns
{
    public abstract class AtkPattern : MonoBehaviour
    {
        [Header("Global")]
        [SerializeField] protected BulletManager m_bulletManager;
        [HideInInspector] public float m_refreshTime = 0.3f;
        public float m_chargeTime;

        protected Transform m_shooterTrs;

        [Header("Burst")]
        [SerializeField] protected int m_burstNb;
        [SerializeField] protected float m_timeBetweenBurst;

        /*-------------------------------------------------------------------*/

        public abstract void StartAtkPattern(EnemyController enemy);
        protected abstract void DoOneBurst(int burstIndex);
    }
}
