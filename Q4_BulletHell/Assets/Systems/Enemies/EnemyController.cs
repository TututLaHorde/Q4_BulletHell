using UnityEngine;
using BH.Tools;

namespace BH.Enemies
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private int m_maxHp;
        [SerializeField] private VisualSliderBar m_bossHpBar;

        [HideInInspector] public EnemyLife m_life;

        private void Start()
        {
            m_life = new(m_maxHp, m_bossHpBar);
        }
    }
}
