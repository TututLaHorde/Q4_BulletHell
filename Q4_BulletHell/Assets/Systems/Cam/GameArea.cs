using UnityEngine;

namespace BH.Cam
{
    public class GameArea : MonoBehaviour
    {
        public static GameArea instance;

        [SerializeField] private Transform m_topRightTrs;
        [SerializeField] private Transform m_bottomLeftTrs;

        private Camera m_cam;
        private Rect m_areaRect = new();

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

        private void Start()
        {
            m_cam = GetComponent<Camera>();
            ChangeGameArea();
        }

        //private void OnDrawGizmos()
        //{
        //    Gizmos.DrawCube(m_areaRect.center, m_areaRect.size);
        //}

        /*-------------------------------------------------------------------*/

        public void ChangeGameArea()
        {
            //change 
            float x = m_bottomLeftTrs.position.x;
            float y = m_bottomLeftTrs.position.y;
            float width = m_topRightTrs.position.x - m_bottomLeftTrs.position.x;
            float height = m_topRightTrs.position.y - m_bottomLeftTrs.position.y;
            m_areaRect.size = m_cam.sensorSize;
            m_areaRect.Set(x, y ,width, height);
        }

        public Vector2 KeepPositionInArea(Vector2 position)
        {
            position.x = Mathf.Clamp(position.x, m_areaRect.x, m_areaRect.x + m_areaRect.width);
            position.y = Mathf.Clamp(position.y, m_areaRect.y, m_areaRect.y + m_areaRect.height);
            return position;
        }

        public bool IsInGameArea(Vector2 position)
        {
            if (m_areaRect.Contains(position))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

