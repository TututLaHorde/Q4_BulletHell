using BH.Enemies;
using System;
using System.Collections;
using UnityEngine;

namespace BH.Patterns
{
    public class PatternCircleAround : AtkPattern
    {
        [Header("Shoot Origin")]
        [SerializeField][Range(0f, 360f)] private float m_startAngle;
        [SerializeField][Range(0f, 360f)] private float m_angleOffset;
        [SerializeField] private float m_distFromCenter;

        [Header("One Burst Params")]
        [SerializeField] private float m_rotationEachBurst;
        [SerializeField] private int m_nbBullet;

        private EnemyController m_enemy;

        /*-------------------------------------------------------------------*/

        public override void StartAtkPattern(EnemyController enemy)
        {
            m_shooterTrs = enemy.transform;
            m_enemy = enemy;

            StopAllCoroutines();
            StartCoroutine(BurstManagement());
        }

        protected override void DoOneBurst(int burstIndex)
        {
            //secu
            if (m_bulletManager == null)
            {
                Debug.Log("no bullet manager");
                return;
            }


            //shoots
            float angleBetweenBullet = m_angleOffset;

            if (m_nbBullet > 1)
            {
                float angleOffset = Mathf.Clamp(m_angleOffset, 0, 360f - (360f / m_nbBullet));
                angleBetweenBullet = angleOffset / (m_nbBullet - 1);
            }

            for (int i = 0; i < m_nbBullet; i++)
            {
                float radians = (m_shooterTrs.rotation.eulerAngles.z + m_startAngle + angleBetweenBullet * i) * Mathf.Deg2Rad;
                Vector3 dir = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians));


                Vector3 origin = m_shooterTrs.position + dir * m_distFromCenter;

                m_bulletManager.Shoot(origin, dir);
            }


            //changements after shoot
            Vector3 newRot = m_shooterTrs.rotation.eulerAngles + Vector3.forward * m_rotationEachBurst;
            m_shooterTrs.rotation = Quaternion.Euler(newRot);
        }

        /*-------------------------------------------------------------------*/

        private IEnumerator BurstManagement()
        {
            for (int i = 0; i < m_burstNb; i++)
            {
                DoOneBurst(i);

                yield return new WaitForSeconds(m_timeBetweenBurst);
            }

            m_enemy.FinishAnAtkPattern();
        }
    }
}
