using UnityEngine;

namespace BH.Game
{
    public class GameArea : MonoBehaviour
    {
        public static GameArea instance;

        [SerializeField] private float m_radiusPlayerArea;
        [SerializeField] private float m_radiusBulletArea;

        /*-------------------------------------------------------------------*/

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.Log("There is two game area");
                Destroy(gameObject);
                return;
            }
        }

        /*-------------------------------------------------------------------*/

        public Vector2 KeepPositionInPlayerArea(Vector2 position)
        {
            Vector2 dir = position - (Vector2)transform.position;
            position = dir.normalized * m_radiusPlayerArea;
            return position;
        }

        public bool IsInPlayerGameArea(Vector2 position)
        {
            Vector2 ownPos = transform.position;
            float dist = Vector2.Distance(position, ownPos);
            if (dist <= m_radiusPlayerArea)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsInBulletGameArea(Vector2 position)
        {
            Vector2 ownPos = transform.position;
            float dist = Vector2.Distance(position, ownPos);
            if (dist <= m_radiusBulletArea)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public float GetBulletRadius()
        {
            return m_radiusBulletArea;
        }
    }
}

