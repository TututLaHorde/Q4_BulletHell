using BH.Tools;

namespace BH.Enemies
{
    public class EnemyLife
    {
        private int m_currentHp;
        private int m_maxHp;
        private VisualSliderBar m_bossHpBar;

        public EnemyLife(int maxHp, VisualSliderBar hpBar)
        { 
            m_bossHpBar = hpBar;
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
            if (m_bossHpBar != null)
            {
                //Debug.Log((float)m_currentHp / m_maxHp);
                m_bossHpBar.SetRatio((float)m_currentHp / m_maxHp);
            }
        }
    }
}
