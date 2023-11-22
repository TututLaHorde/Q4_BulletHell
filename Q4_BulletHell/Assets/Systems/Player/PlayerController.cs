using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;

namespace BH.Player
{
    public class PlayerController : MonoBehaviour
    {
        //stats
        [SerializeField] private float m_speed;

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
                m_rb.velocity = Vector2.zero;
                return;
            }

            //move
            direction = direction.normalized;
            m_rb.velocity = direction * speed;
        }
    }
}

