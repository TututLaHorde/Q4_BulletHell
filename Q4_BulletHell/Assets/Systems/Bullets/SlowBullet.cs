using BH.Player;
using UnityEngine;

namespace BH.Bullets
{
    public class SlowBullet : Bullet
    {
        private bool m_isCollidWithPlayer;

        /*-------------------------------------------------------------------*/

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision == null) { return; }

            //collid with enemy
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                m_isCollidWithPlayer = true;

                //enemy takes dmg
                if (collision.transform.parent.TryGetComponent(out PlayerController playerController))
                {
                    Debug.Log("Player is dead : " + playerController);
                }
            }
        }

        /*-------------------------------------------------------------------*/

        public override void Init(Vector3 origin, Vector3 direction)
        {
            gameObject.SetActive(true);

            m_initialDir = direction;
            transform.position = origin;
            transform.rotation = Quaternion.Euler(Vector3.forward * Vector2.SignedAngle(Vector2.up, direction));

            m_isCollidWithPlayer = false;
        }

        public override void Move()
        {
            transform.position += m_speed * m_initialDir * Time.deltaTime;
        }
    }
}
