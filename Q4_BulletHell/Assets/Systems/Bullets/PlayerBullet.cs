using UnityEngine;

namespace BH.Bullets
{
    public class PlayerBullet : Bullet
    {
        private bool m_isCollidWithEnemy;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision == null) { return; }

            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                m_isCollidWithEnemy = true;
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
