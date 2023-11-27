using System.Collections.Generic;
using UnityEngine;
using BH.Tools;

namespace BH.Bullets
{
    public class BulletManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_prefab;
        [SerializeField] private int m_nbPreAllocated;

        private PoolingManager<Bullet> m_pooling;
        private List<Bullet> m_activeBullet = new();

        /*-------------------------------------------------------------------*/

        private void Awake()
        {
            m_nbPreAllocated = Mathf.Abs(m_nbPreAllocated);
            m_pooling = new(m_prefab, transform, m_nbPreAllocated);
        }

        private void FixedUpdate()
        {
            MoveBullets();
        }

        /*-------------------------------------------------------------------*/

        public void Shoot(Vector3 origin, Vector3 direction)
        {
            Bullet m_newBullet = m_pooling.UseNew();
            m_activeBullet.Add(m_newBullet);

            m_newBullet.Init(origin, direction);
        }

        /*-------------------------------------------------------------------*/

        private void MoveBullets()
        {
            for (int i = 0;  i < m_activeBullet.Count; i++)
            {
                m_activeBullet[i].Move();
                if (m_activeBullet[i].IsNeedToBeRemove())
                {
                    m_pooling.StopUsing(m_activeBullet[i]);
                    m_activeBullet.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
