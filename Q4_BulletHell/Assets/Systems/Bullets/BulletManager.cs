using System.Collections.Generic;
using UnityEngine;

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

        public void Shoot(Transform origin, Vector3 direction)
        {
            Bullet m_newBullet = m_pooling.UseNew();
            m_activeBullet.Add(m_newBullet);
        }

        /*-------------------------------------------------------------------*/

        private void MoveBullets()
        {
            foreach (Bullet bullet in m_activeBullet)
            {
                bullet.Move();
            }
        }
    }
}
