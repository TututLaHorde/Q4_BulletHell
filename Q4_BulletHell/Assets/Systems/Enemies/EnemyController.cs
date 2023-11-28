using UnityEngine;
using System.Collections.Generic;
using System.Collections;

using BH.Tools;
using BH.Patterns;
using BH.Bullets;

namespace BH.Enemies
{
    public class EnemyController : MonoBehaviour
    {
        [Header("Life")]
        [SerializeField] private int m_maxHp;
        [SerializeField] private VisualSliderBar m_bossHpBar;

        [HideInInspector] public EnemyLife m_life;

        [Header("Atk Patterns")]
        [SerializeField] private List<AtkPattern> m_atkPatterns = new();
        private int m_patternIndex;

        /*-------------------------------------------------------------------*/

        private void Start()
        {
            //init life
            m_life = new EnemyLife(m_maxHp, m_bossHpBar, this);

            //first pattern
            StartCoroutine(WaitBetweenPattern());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision == null) { return; }

            //collid with enemy
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                //enemy takes dmg
                if (collision.gameObject.TryGetComponent(out PlayerBullet playerBullet))
                {
                    m_life.TakeDamage(playerBullet.m_damage);
                    playerBullet.m_isCollidWithEnemy = true;
                }
            }
        }

        /*-------------------------------------------------------------------*/

        public void FinishAnAtkPattern()
        {
            StartCoroutine(WaitBetweenPattern());
        }

        /*-------------------------------------------------------------------*/

        private void StartNextAtkPattern()
        {
            //next atk pattern
            if (m_patternIndex < m_atkPatterns.Count)
            {
                if (m_atkPatterns[m_patternIndex] != null)
                {
                    m_atkPatterns[m_patternIndex].StartAtkPattern(this);
                    m_patternIndex++;
                }
            }
            //loop when reach the end
            else if (m_atkPatterns.Count != 0)
            {
                m_patternIndex = 0;
                StartNextAtkPattern();
            }
        }

        private IEnumerator WaitBetweenPattern()
        {
            int nextPattern;
            int previousPattern;

            //set next pattern
            if (m_patternIndex < m_atkPatterns.Count)
            {
                nextPattern = m_patternIndex;
            }
            else
            {
                nextPattern = 0;
            }

            //set previous pattern
            if (nextPattern == 0)
            {
                previousPattern = m_atkPatterns.Count - 1;
            }
            else
            {
                previousPattern = m_patternIndex - 1;
            }

            //Calcul waiting time
            float waitingTime = (m_atkPatterns[previousPattern].m_refreshTime + m_atkPatterns[previousPattern].m_chargeTime);
            waitingTime *= m_life.m_lifeRatio;

            //Wait before doing next pattern
            yield return new WaitForSeconds(waitingTime);

            StartNextAtkPattern();
        }
    }
}
