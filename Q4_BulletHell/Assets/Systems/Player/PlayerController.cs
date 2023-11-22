using UnityEngine;
using UnityEngine.Assertions;
using BH.Cam;

namespace BH.Player
{
    public class PlayerController : MonoBehaviour
    {
        //stats
        [SerializeField] private float m_speed;
        [SerializeField] private Transform m_enemyTrs;

        //own components
        private Rigidbody2D m_rb;
        private Transform m_ownTrs;

        //others
        private Vector3 m_targetPos;

        /*-------------------------------------------------------------------*/

        private void Start()
        {
            m_ownTrs = GetComponent<Transform>();
            m_rb = GetComponent<Rigidbody2D>();

            Assert.IsNotNull(m_rb);
        }

        private void FixedUpdate()
        {
            LookToTarget();
            MoveToTarget();
        }

        /*-------------------------------------------------------------------*/

        public void MoveTargetChange(Vector3 targetPos)
        {
            m_targetPos = targetPos;
        }

        /*-------------------------------------------------------------------*/

        private void MoveToTarget()
        {
            float speed = m_speed;
            Vector2 direction = m_targetPos - m_ownTrs.position;

            //too close of the target
            if (direction.magnitude < speed * Time.deltaTime)
            {
                m_ownTrs.position = new Vector3(m_targetPos.x, m_targetPos.y, 0f);
                speed = 0f;
            }

            direction = direction.normalized;
            Vector2 potentialPos = (Vector2)m_ownTrs.position + direction * speed * Time.deltaTime;
            if (GameArea.instance.IsInGameArea(potentialPos))
            {
                //move to target
                m_rb.velocity = direction * speed;
            }
            else
            {
                //move to border of the game area
                m_ownTrs.position = (Vector3)GameArea.instance.KeepPositionInArea(potentialPos);
                m_rb.velocity = Vector2.zero;
            }
        }

        private void LookToTarget()
        {
            Vector2 direction = m_enemyTrs.position - m_ownTrs.position;
            if (direction.magnitude > 0)
            {
                direction = direction.normalized;
                float angle = Vector2.SignedAngle(Vector2.up, direction);
                m_ownTrs.rotation = Quaternion.Euler(Vector3.forward * angle);
            }
        }
    }
}

