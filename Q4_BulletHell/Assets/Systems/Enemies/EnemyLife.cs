using BH.Tools;

namespace BH.Enemies
{
    public class EnemyLife
    {
        public float m_lifeRatio {  get; private set; }

        private int m_currentHp;
        private int m_maxHp;
        private VisualSliderBar m_bossHpBar;
        private EnemyController m_enemy;

        public EnemyLife(int maxHp, VisualSliderBar hpBar, EnemyController enemy)
        { 
            m_bossHpBar = hpBar;
            m_enemy = enemy;
            SetMaxHp(maxHp);
            UpdtLifeBar();
        }

        /*-------------------------------------------------------------------*/

        public void TakeDamage(int damage)
        {
            m_currentHp -= damage;

            if (m_currentHp < 0)
            {
                m_currentHp = 0;
                m_enemy.GetComponentInParent<EnemiesManager>().AnEnemyDie(m_enemy);
            }

            UpdtLifeBar();
        }

        /*-------------------------------------------------------------------*/

        private void SetMaxHp(int hp)
        {
            m_maxHp = hp;
            m_currentHp = hp;
        }

        private void UpdtLifeBar()
        {
            m_lifeRatio = (float)m_currentHp / m_maxHp;

            if (m_bossHpBar != null)
            {
                //Debug.Log(m_lifeRatio);
                m_bossHpBar.SetRatio(m_lifeRatio);
            }
        }
    }
}
