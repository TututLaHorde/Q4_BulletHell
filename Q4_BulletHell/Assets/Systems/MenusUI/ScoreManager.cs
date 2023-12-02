using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BH.MenusUI
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager instance;

        [Header("UI")]
        [SerializeField] private TMP_Text m_txtBossSurvive;
        [SerializeField] private TMP_Text m_txtStats;
        [SerializeField] private List<GameObject> m_threeStars = new();

        [Header("Score Params")]
        [SerializeField][Min(1f)] private float m_gameAverageTime = 1f;
        [SerializeField][Range(0f, 0.8f)] private float m_enemyScoreRelativeToBoss;
        private int m_bossKillValue;
        private int m_enemyKillValue;

        private readonly float m_timeMultiplicator = 0.1f;
        private readonly int m_idealBossScoreValue = 200000; //Max 450 000

        //own stats
        private float m_timeFromStart = 0;
        private float m_score = 0f;

        //external stats
        private string m_lastBossName;
        private float m_lastBossLifeRatio;

        private int m_nbEnemiesDead; // without boss
        private int m_nbBossDead;

        /*-------------------------------------------------------------------*/

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                Debug.Log("There is two ScoreManager");
                Destroy(this);
                return;
            }

            InitScoreValues();
        }

        private void Update()
        {
            m_timeFromStart += Time.deltaTime;
        }

        /*-------------------------------------------------------------------*/

        public void OnDefeat()
        {
            //draw boss remaining hp
            if (m_txtBossSurvive != null)
            {
                m_txtBossSurvive.text = "The " + m_lastBossName + " survived with ";

                int lifePercent = Mathf.CeilToInt(m_lastBossLifeRatio * 100f);
                m_txtBossSurvive.text += lifePercent + " % of his HP";
            }
        }

        public void OnVictory()
        {
            //time in minutes
            string time = SecondToChrono();

            //calculate score
            string score = GetScoreText();

            //Stars
            ActiveStars();

            //draw
            if (m_txtStats != null)
            {
                m_txtStats.text  = score + "\n";
                m_txtStats.text += time + "\n";
                m_txtStats.text += (m_nbEnemiesDead + m_nbBossDead).ToString();
            }
        }

        public void EnemyDeath()
        {
            m_nbEnemiesDead++;
        }

        public void BossDeath()
        {
            m_nbBossDead++;
        }

        public void BossChangeHP(float hpRatio, string bossName)
        {
            m_lastBossName = bossName;
            m_lastBossLifeRatio = hpRatio;
        }

        /*-------------------------------------------------------------------*/

        private void InitScoreValues()
        {
            m_bossKillValue = Mathf.RoundToInt(m_gameAverageTime * m_timeMultiplicator * m_idealBossScoreValue);
            m_enemyKillValue = Mathf.RoundToInt(m_enemyScoreRelativeToBoss * m_bossKillValue);
        }

        private string SecondToChrono()
        {
            string time = "";

            //calcul
            int minutes = Mathf.FloorToInt(m_timeFromStart / 60f);
            float seconds = (int)(m_timeFromStart % 60f * 1000f) / 1000f;

            //minutes
            if (minutes < 10)
            {
                time += "0";
            }
            time += minutes + " : ";

            //second and millisecond
            if (seconds < 10f)
            {
                time += "0";
            }
            time += seconds;

            return time;
        }

        private string GetScoreText()
        {
            //calculate score
            m_score = (m_nbEnemiesDead * m_enemyKillValue + m_nbBossDead * m_bossKillValue) / (m_timeFromStart * m_timeMultiplicator);
            m_score = Mathf.Round(m_score);

            //limit
            m_score = Mathf.Clamp(m_score, 0, 999999f);

            //set string
            int thousand = Mathf.FloorToInt((m_score % 1000000f) / 1000f);
            string score = "";
            if (thousand > 0)
            {
                score = thousand + " ";
            }

            int rest = Mathf.RoundToInt(m_score % 1000f);
            if (rest < 10)
            {
                score += "00";
            }
            else if (rest < 100)
            {
                score += "0";
            }
            score += rest;

            return score;
        }

        private void ActiveStars()
        {
            if (m_threeStars.Count == 3)
            {
                //first star
                if (m_score >= m_idealBossScoreValue / 2f)
                {
                    m_threeStars[0].SetActive(true);
                }

                //second star
                if (m_score >= m_idealBossScoreValue)
                {
                    m_threeStars[1].SetActive(true);
                }

                //third star
                if (m_score >= m_idealBossScoreValue * 1.5f)
                {
                    m_threeStars[2].SetActive(true);
                }
            }
        }
    }
}
