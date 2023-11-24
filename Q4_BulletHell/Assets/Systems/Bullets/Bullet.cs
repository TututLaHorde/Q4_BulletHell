using BH.Cam;
using UnityEngine;

namespace BH.Bullets
{
    public abstract class Bullet : MonoBehaviour
    {
        [SerializeField] protected float m_speed;

        protected Vector3 m_initialDir = Vector3.zero;

        /*-------------------------------------------------------------------*/

        public abstract void Init(Vector3 origin, Vector3 direction);
        public abstract void Move();

        public virtual bool IsNeedToBeRemove()
        {
            if (GameArea.instance.IsInGameArea(transform.position))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
