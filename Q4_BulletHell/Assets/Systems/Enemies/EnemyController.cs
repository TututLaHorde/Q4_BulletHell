using UnityEngine;
using System.Collections.Generic;
using System.Collections;

using BH.Tools;
using BH.Patterns;
using BH.Bullets;
using BH.Game;
using BH.Player;
using BH.Music;

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

        [Header("Explosion")]
        [SerializeField] private GameObject m_explosionParticule;
        [SerializeField] private float m_explosionTime;
        [SerializeField] private float m_shakeAmount;

        [Header("Audio")]
        [SerializeField] private AudioClip m_clipExplosion;
        [SerializeField] private AudioClip m_clipImpact;
        [SerializeField][Range(0f, 1f)] private float m_impactVolume;

        private bool m_isAlive = true;

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

            //collid with player bullet
            if (m_isAlive &&
                collision.gameObject.layer == LayerMask.NameToLayer("Player") &&
                PlayerController.instance.m_isAlive && collision.gameObject.TryGetComponent(out PlayerBullet playerBullet))
            {
                //enemy takes dmg
                m_life.TakeDamage(playerBullet.m_damage);
                SfxManager.instance.PlayMultipleSfx(m_clipImpact, m_impactVolume);

                //for bullet pooling
                playerBullet.m_isCollidWithEnemy = true;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            //player die
            if (m_isAlive && collision.gameObject.TryGetComponent(out PlayerCollision player))
            {
                GameManager.instance.PlayerDie();
            }
        }

        /*-------------------------------------------------------------------*/

        public void FinishAnAtkPattern()
        {
            StartCoroutine(WaitBetweenPattern());
        }

        public void Die()
        {
            if (m_isAlive)
            {
                StopAllCoroutines();
                StartCoroutine(DeathExplosion());
            }
        }

        /*-------------------------------------------------------------------*/

        private void StartNextAtkPattern()
        {
            if (!m_isAlive)
            {  return; }

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

        private IEnumerator DeathExplosion()
        {
            m_isAlive = false;

            //sfx, animation, screen shake
            SfxManager.instance.PlaySfx(m_clipExplosion);
            m_explosionParticule.SetActive(true);
            ScreenShake.instance.m_amount += m_shakeAmount;

            yield return new WaitForSeconds(m_explosionTime);

            GetComponentInParent<EnemiesManager>().AnEnemyDie(this);
        }
    }
}
