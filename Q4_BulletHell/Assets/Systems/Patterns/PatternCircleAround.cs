using BH.Enemies;
using System.Collections;
using UnityEngine;

namespace BH.Patterns
{
    public class PatternCircleAround : AtkPattern
    {
        /*-------------------------------------------------------------------*/

        public override void StartAtkPattern(EnemyController enemy)
        {
            m_shooterTrs = enemy.transform;

            StopAllCoroutines();
            StartCoroutine(BurstManagement(enemy));
        }

        protected override void DoOneBurst(int burstIndex)
        {
            if (m_bulletManager != null)
            {
                m_bulletManager.Shoot(m_shooterTrs, Vector2.up);            
            }
            else
            {
                Debug.Log("no bullet manager");
            }
        }

        /*-------------------------------------------------------------------*/

        private IEnumerator BurstManagement(EnemyController enemy)
        {
            for (int i = 0; i < m_burstNb; i++)
            {
                DoOneBurst(i);

                yield return new WaitForSeconds(m_timeBetweenBurst);
            }

            enemy.FinishAnAtkPattern();
        }
    }
}
