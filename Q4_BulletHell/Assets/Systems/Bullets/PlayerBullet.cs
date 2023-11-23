using UnityEngine;
using BH.Enemies;

namespace BH.Bullets
{
    public class PlayerBullet : Bullet
    {
        [SerializeField] private int m_dammage;
        private bool m_isCollidWithEnemy;

        /*-------------------------------------------------------------------*/

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision == null) { return; }

            //collid with enemy
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                m_isCollidWithEnemy = true;

                //enemy takes dmg
                if (collision.gameObject.TryGetComponent(out EnemyController enemyController))
                {
                    enemyController.m_life.TakeDamage(m_dammage);
                }
            }
        }

        /*-------------------------------------------------------------------*/

        public override void Init(Transform origin, Vector3 direction)
        {
            gameObject.SetActive(true);

            m_initialDir = direction;
            transform.position = origin.position;
            transform.rotation = Quaternion.Euler(Vector3.forward * Vector2.SignedAngle(Vector2.up, direction));

            m_isCollidWithEnemy = false;
        }

        public override void Move()
        {
            transform.position += m_speed * m_initialDir * Time.deltaTime;
        }

        public override bool IsNeedToBeRemove()
        {
            if (m_isCollidWithEnemy)
            {
                return true;
            }

            return base.IsNeedToBeRemove();
        }
    }
}
