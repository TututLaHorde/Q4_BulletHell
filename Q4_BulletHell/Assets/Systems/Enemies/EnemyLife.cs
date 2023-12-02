using BH.MenusUI;
using BH.Tools;
using TMPro;

namespace BH.Enemies
{
    public class EnemyLife
    {
        public float m_lifeRatio {get; private set;}

        private int m_currentHp;
        private int m_maxHp;

        private VisualSliderBar m_bossHpBar;
        private TMP_Text m_textName;
        private string m_name;

        private EnemyController m_enemy;

        public EnemyLife(int maxHp, VisualSliderBar hpBar, EnemyController enemy)
        { 
            m_bossHpBar = hpBar;
            m_enemy = enemy;

            m_textName = m_enemy.m_txtBossName;
            m_name = m_enemy.m_bossName;

            SetMaxHp(maxHp);
            UpdtLifeBar();
        }

        /*-------------------------------------------------------------------*/

        public void TakeDamage(int damage)
        {
            m_currentHp -= damage;

            //Death
            if (m_currentHp < 0)
            {
                m_currentHp = 0;
                m_enemy.Die();
                ScoreDeathCount();
            }

            UpdtLifeBar();
        }

        public void RegenLife(int hp)
        {
            m_currentHp += hp;
            UpdtLifeBar();
        }

        public int GetCurrentHp()
        { return m_currentHp; }

        /*-------------------------------------------------------------------*/

        private void SetMaxHp(int hp)
        {
            m_maxHp = hp;
            m_currentHp = hp;
        }

        private void UpdtLifeBar()
        {
            m_lifeRatio = (float)m_currentHp / m_maxHp;

            //set life bar and boss name
            if (m_bossHpBar != null && m_textName != null)
            {
                m_bossHpBar.SetRatio(m_lifeRatio);
                m_textName.text = m_name;

                ScoreManager.instance.BossChangeHP(m_lifeRatio, m_name);
            }
        }

        private void ScoreDeathCount()
        {
            if (m_enemy.IsBossEnemy())
            {
                ScoreManager.instance.BossDeath();
            }
            else
            {
                ScoreManager.instance.EnemyDeath();
            }
        }
    }
}
