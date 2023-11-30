using BH.Bullets;
using System.Collections;
using UnityEngine;

namespace BH.Player
{
    public class PlayerShoot : MonoBehaviour
    {
        [SerializeField] private float m_bulletIntervale;
        [SerializeField][Range(0f, 0.8f)] private float m_bulletSpread;

        [SerializeField] private BulletManager m_bulletManager;
        [SerializeField] private Transform m_shootOrigin;

        private PlayerController m_playerController;

        private void Start()
        {
            m_playerController = GetComponent<PlayerController>();

            StartCoroutine(AutoShoot());
        }

        private IEnumerator AutoShoot()
        {
            while (true)
            {
                yield return new WaitForSeconds(m_bulletIntervale);

                if (m_playerController != null && m_playerController.m_isAlive) 
                {
                    Vector3 dir = m_playerController.m_enemyTrs.position - m_shootOrigin.position;
                    //Vector3 dir = Vector3.right;
                    dir = (dir.normalized + (Vector3)RandomVector2() * m_bulletSpread).normalized;

                    m_bulletManager.Shoot(m_shootOrigin.position, dir);
                }
            }
        }

        private Vector2 RandomVector2()
        {
            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-1f, 1f);

            return new Vector2(x, y);
        }
    }
}
