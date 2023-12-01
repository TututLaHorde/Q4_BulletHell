using UnityEngine;
using System.Collections.Generic;
using System.Collections;

using BH.Tools;
using BH.Patterns;
using BH.Bullets;
using BH.Game;
using BH.Player;
using BH.Music;
using UnityEngine.Assertions;

namespace BH.Enemies
{
    public enum AtkPatternState
    {
        pattern1, pattern2, pattern3,
    }

    public class EnemyController : MonoBehaviour
    {
        [Header("Life")]
        [SerializeField] private int m_maxHp;
        [SerializeField] private VisualSliderBar m_bossHpBar;

        [HideInInspector] public EnemyLife m_life;

        [Header("Atk Patterns")]
        [SerializeField] private List<AtkPattern> m_atkPatterns1 = new();
        [SerializeField] private List<AtkPattern> m_atkPatterns2 = new();
        [SerializeField] private List<AtkPattern> m_atkPatterns3 = new();
        private List<AtkPattern> m_atkPatterns;
        private AtkPatternState m_atkState;
        private int m_patternIndex = 0;

        [Header("Explosion")]
        [SerializeField] private GameObject m_explosionParticule;
        [SerializeField] private float m_explosionTime;
        [SerializeField] private float m_shakeAmount;

        [Header("Audio")]
        [SerializeField] private AudioClip m_clipExplosion;
        [SerializeField] private AudioClip m_clipImpact;
        [SerializeField][Range(0f, 1f)] private float m_impactVolume;

        public int m_poolingIndex { get; private set; }
        private bool m_isAlive = true;

        private CameraFollow m_camFollow;

        /*-------------------------------------------------------------------*/

        private void Start()
        {
            //init life
            m_life = new EnemyLife(m_maxHp, m_bossHpBar, this);

            FirstAtkPattern();

            m_camFollow = Camera.main.GetComponentInParent<CameraFollow>();
            Assert.IsNotNull(m_camFollow);
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

        public void Init(int poolingindex)
        {
            //for pooling
            m_poolingIndex = poolingindex;

            //init life
            m_life = new EnemyLife(m_maxHp, m_bossHpBar, this);
            m_isAlive = true;

            //active but not explosion fx
            transform.GetChild(0).gameObject.SetActive(false);
            gameObject.SetActive(true);

            FirstAtkPattern();
        }

        public void FinishAnAtkPattern()
        {
            ChangeAtkPatternState();
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

        public bool IsBossEnemy()
        {
            return m_bossHpBar != null;
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
            int previousPattern;

            //get previous pattern
            if (m_patternIndex == 0)
            {
                previousPattern = m_atkPatterns.Count - 1;
            }
            else
            {
                previousPattern = m_patternIndex - 1;
            }

            //Wait before doing next pattern
            yield return new WaitForSeconds(m_atkPatterns[previousPattern].m_timeBeforeNextPattern);

            StartNextAtkPattern();
        }

        private IEnumerator DeathExplosion()
        {
            m_isAlive = false;
            EnemiesManager manager = GetComponentInParent<EnemiesManager>();

            //sfx, animation, screen shake
            SfxManager.instance.PlaySfx(m_clipExplosion);
            m_explosionParticule.SetActive(true);
            ScreenShake.instance.m_amount += m_shakeAmount;

            bool isLastEnemy = manager.IsLastBoss(this);
            if (isLastEnemy)
            {
                //zoom on the last enemy death
                m_camFollow.trsTarget = transform;

                //wait for the explosion anim
                yield return new WaitForSeconds(m_explosionTime);

                //zoom on the last enemy death
                m_camFollow.ResetTarget();
            }
            else
            {
                //wait for the explosion anim
                yield return new WaitForSeconds(m_explosionTime);
            }


            manager.AnEnemyDie(this, isLastEnemy);
        }

        private void ChangeAtkPatternState()
        {
            if (m_atkState == AtkPatternState.pattern1 && m_life.m_lifeRatio <= 0.75f)
            {
                m_atkPatterns = m_atkPatterns2;
                m_atkState = AtkPatternState.pattern2;
                m_patternIndex = 0;
            }
            else if (m_atkState == AtkPatternState.pattern2 && m_life.m_lifeRatio <= 0.4f)
            {
                m_atkPatterns = m_atkPatterns3;
                m_atkState = AtkPatternState.pattern3;
                m_patternIndex = 0;
            }
        }

        private void FirstAtkPattern()
        {
            m_atkPatterns = m_atkPatterns1;
            m_atkState = AtkPatternState.pattern1;
            StartNextAtkPattern();
        }
    }
}
