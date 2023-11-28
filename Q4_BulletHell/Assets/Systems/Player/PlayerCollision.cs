using BH.Bullets;
using BH.Game;
using UnityEngine;

namespace BH.Player
{
    public class PlayerCollision : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision == null) { return; }

            //collid with enemy
            if (collision.gameObject.layer == LayerMask.NameToLayer("Bullets"))
            {
                //enemy takes dmg
                if (collision.transform.TryGetComponent(out Bullet bullet))
                {
                    GameManager.instance.PlayerDie();
                }
            }
        }
    }
}

