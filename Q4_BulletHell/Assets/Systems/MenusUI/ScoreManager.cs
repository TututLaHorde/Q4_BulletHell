using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace BH.MenusUI
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager instance;

        [Header("UI")]
        [SerializeField] private TMP_Text m_txtBossSurvive;
        [SerializeField] private TMP_Text m_txtBossLowestHP;
        [SerializeField] private TMP_Text m_txtStats;
        [SerializeField] private TMP_Text m_txtHighScore;

        [SerializeField] private GameObject m_highScoreTrophy;
        [SerializeField] private GameObject m_lowestHPTrophy;
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

        //player prefs key
        private readonly string m_highScoreKey = "highscore";
        private readonly string m_lowestBossHpKey = "LowestBossHp";

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

            Assert.IsNotNull(m_txtBossSurvive);
            Assert.IsNotNull(m_txtBossLowestHP);
            Assert.IsNotNull(m_txtStats);
            Assert.IsNotNull(m_txtHighScore);

            Assert.IsNotNull(m_highScoreTrophy);
            Assert.IsNotNull(m_lowestHPTrophy);
        }

        private void Update()
        {
            m_timeFromStart += Time.deltaTime;
        }

        /*-------------------------------------------------------------------*/

        public void OnDefeat()
        {
            //draw boss remaining hp
            m_txtBossSurvive.text = "The " + m_lastBossName + " survived with ";

            int lifePercent = Mathf.CeilToInt(m_lastBossLifeRatio * 100f);
            m_txtBossSurvive.text += lifePercent + "% of his HP";

            //set lowest HP
            if (PlayerPrefs.GetInt(m_lowestBossHpKey) >= lifePercent)
            {
                PlayerPrefs.SetInt(m_lowestBossHpKey, lifePercent);
                m_lowestHPTrophy.SetActive(true);
            }

            m_txtBossLowestHP.text = "Best : " + PlayerPrefs.GetInt(m_lowestBossHpKey) + "%";
        }

        public void OnVictory()
        {
            //time in minutes
            string time = SecondToChrono();

            //calculate score
            CalculateScore();
            string score = GetScoreText(m_score);
            string highScore = GetScoreText(PlayerPrefs.GetFloat(m_highScoreKey));

            //Stars
            ActiveStars();

            //draw current stats
            m_txtStats.text  = score + "\n";
            m_txtStats.text += time + "\n";
            m_txtStats.text += (m_nbEnemiesDead + m_nbBossDead).ToString() + "\n";

            //draw high score
            m_txtHighScore.text = "HIGHSCORE : " + highScore;
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

            //first high score
            if (!PlayerPrefs.HasKey(m_highScoreKey))
            {
                PlayerPrefs.SetFloat(m_highScoreKey, 0f);
            }

            if(!PlayerPrefs.HasKey(m_lowestBossHpKey))
            {
                PlayerPrefs.SetInt(m_lowestBossHpKey, 100);
            }
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

        private string GetScoreText(float score)
        {
            string scoretxt = "";

            //set string
            int thousand = Mathf.FloorToInt((score % 1000000f) / 1000f);
            if (thousand > 0)
            {
                scoretxt = thousand + " ";
            }

            int rest = Mathf.RoundToInt(score % 1000f);
            if (rest < 10)
            {
                scoretxt += "00";
            }
            else if (rest < 100)
            {
                scoretxt += "0";
            }
            scoretxt += rest;

            return scoretxt;
        }

        private void CalculateScore()
        {
            //calculate score
            m_score = (m_nbEnemiesDead * m_enemyKillValue + m_nbBossDead * m_bossKillValue) / (m_timeFromStart * m_timeMultiplicator);
            m_score = Mathf.Round(m_score);

            //limit
            m_score = Mathf.Clamp(m_score, 0, 999999f);

            //Set Highscore
            if (PlayerPrefs.GetFloat(m_highScoreKey) <= m_score)
            {
                PlayerPrefs.SetFloat(m_highScoreKey, m_score);
                m_highScoreTrophy.SetActive(true);
            }

            //Set Lowest Boss HP to 0%
            PlayerPrefs.SetInt(m_lowestBossHpKey, 0);
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
