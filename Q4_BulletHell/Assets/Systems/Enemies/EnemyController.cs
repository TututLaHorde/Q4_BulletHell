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
using TMPro;

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
        [HideInInspector] public EnemyLife m_life;

        [Header("Boss params")]
        [SerializeField] private VisualSliderBar m_bossHpBar;
        public TMP_Text m_txtBossName;
        public string m_bossName = "BOSS";


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
        [SerializeField] private GameObject m_sprites;

        [Header("Audio")]
        [SerializeField] private AudioClip m_clipExplosion;
        [SerializeField] private AudioClip m_clipImpact;
        [SerializeField] private AudioClip m_clipRegen;
        [SerializeField][Range(0f, 1f)] private float m_impactVolume;

        [Header("Absorbtion")]
        [SerializeField] private float m_absorbtionTime = 1f;
        [SerializeField] private AnimationCurve m_absorbtionCurve;
        [SerializeField][Min(0f)] private float m_regenMultiplier = 2f;

        //other component
        private CameraFollow m_camFollow;
        private EnemiesManager m_enemiesManager;
        private Vector3 m_originalScale;

        //vars
        public int m_poolingIndex { get; private set; }
        public bool m_isAlive { get; private set; } = true;


        /*-------------------------------------------------------------------*/
        private void Awake()
        {
            m_originalScale = transform.localScale;

            //init boss name
            if (m_txtBossName != null)
            {
                m_txtBossName.text = m_bossName;
            }
        }

        private void Start()
        {
            //init life
            m_life = new EnemyLife(m_maxHp, m_bossHpBar, this);

            //init atk pattern
            FirstAtkPattern();

            //get components
            m_camFollow = Camera.main.GetComponentInParent<CameraFollow>();
            m_enemiesManager = GetComponentInParent<EnemiesManager>();

            Assert.IsNotNull(m_camFollow);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision == null) { return; }

            //enemy takes dmg
            if (m_isAlive && collision.gameObject.layer == LayerMask.NameToLayer("Player") &&
                PlayerController.instance.m_isAlive && collision.gameObject.TryGetComponent(out PlayerBullet playerBullet))
            {
                m_life.TakeDamage(playerBullet.m_damage);
                SfxManager.instance.PlayMultipleSfx(m_clipImpact, m_impactVolume);

                playerBullet.m_isCollidWithEnemy = true;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision == null) { return; }

            if (m_isAlive)
            {
                //player die
                if (collision.gameObject.TryGetComponent(out PlayerCollision player))
                {
                    GameManager.instance.PlayerDie();
                }
                //enemy collid with boss
                else if (!IsBossEnemy() && 
                        collision.gameObject.TryGetComponent(out EnemyController enemy) && 
                        enemy.IsBossEnemy())
                {
                    StartCoroutine(DeathByAbsorbtion());

                    //regen boss
                    int regenHp = Mathf.RoundToInt(m_life.GetCurrentHp() * m_regenMultiplier);
                    enemy.m_life.RegenLife(regenHp);

                    SfxManager.instance.PlaySfx(m_clipRegen);
                }
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

            //reactive sprites
            if (m_sprites != null)
            {
                m_sprites.SetActive(true);
            }
            transform.localScale = m_originalScale;

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
            return (m_bossHpBar != null && m_txtBossName != null);
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

            //sfx, animation, screen shake
            SfxManager.instance.PlaySfx(m_clipExplosion);
            m_explosionParticule.SetActive(true);
            ScreenShake.instance.m_amount += m_shakeAmount;

            //sprites disapear
            if (m_sprites != null)
            {
                m_sprites.SetActive(false);
            }

            //Zoom or continue playing
            bool isLastEnemy = m_enemiesManager.IsLastBoss(this);
            if (isLastEnemy)
            {
                //zoom on the last enemy death
                m_camFollow.trsTarget = transform;
                GameManager.instance.m_playerIsImmortal = true;

                //wait for the explosion anim and reset cam
                yield return new WaitForSeconds(m_explosionTime);
                m_camFollow.ResetTarget();
            }
            else
            {
                //wait for the explosion anim
                yield return new WaitForSeconds(m_explosionTime);
            }


            m_enemiesManager.AnEnemyDie(this, isLastEnemy);
        }

        private IEnumerator DeathByAbsorbtion()
        {
            //can't fonction
            m_isAlive = false;

            //absorbtion anim
            float t = m_absorbtionTime;
            while (t > 0)
            {
                t -= Time.deltaTime;

                //reducing scale
                transform.localScale = m_originalScale * m_absorbtionCurve.Evaluate(t / m_absorbtionTime);
                yield return null;
            }

            //death
            bool isLastEnemy = m_enemiesManager.IsLastBoss(this);
            m_enemiesManager.AnEnemyDie(this, isLastEnemy);
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
