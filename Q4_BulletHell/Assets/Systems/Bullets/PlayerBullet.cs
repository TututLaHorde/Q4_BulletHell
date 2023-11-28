using UnityEngine;
using BH.Enemies;

namespace BH.Bullets
{
    public class PlayerBullet : Bullet
    {
        public int m_damage;
        public bool m_isCollidWithEnemy;

        /*-------------------------------------------------------------------*/

        public override void Init(Vector3 origin, Vector3 direction)
        {
            gameObject.SetActive(true);

            m_initialDir = direction;
            transform.position = origin;
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
