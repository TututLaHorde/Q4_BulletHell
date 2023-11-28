using UnityEngine;

namespace BH.Bullets
{
    public class SlowBullet : Bullet
    {
        public override void Init(Vector3 origin, Vector3 direction)
        {
            gameObject.SetActive(true);

            m_initialDir = direction;
            transform.position = origin;
            transform.rotation = Quaternion.Euler(Vector3.forward * Vector2.SignedAngle(Vector2.up, direction));
        }

        public override void Move()
        {
            transform.position += m_speed * m_initialDir * Time.deltaTime;
        }
    }
}
