using BH.Cam;
using UnityEngine;

namespace BH.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        //stats
        [SerializeField] private float m_speed;

        //own components
        private Transform m_ownTrs;

        //others
        private PlayerController m_playerController;
        [HideInInspector] public Vector3 m_targetPos;

        /*-------------------------------------------------------------------*/

        private void Start()
        {
            m_ownTrs = GetComponent<Transform>();
            m_playerController = GetComponent<PlayerController>();
        }

        private void FixedUpdate()
        {
            LookToTarget();
            MoveToTarget();
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
                m_ownTrs.position = potentialPos;
            }
            else
            {
                //move to border of the game area
                m_ownTrs.position = (Vector3)GameArea.instance.KeepPositionInArea(potentialPos);
            }
        }

        private void LookToTarget()
        {
            Vector2 direction = m_playerController.m_enemyTrs.position - m_ownTrs.position;

            direction = direction.normalized;
            float angle = Vector2.SignedAngle(Vector2.up, direction);
            m_ownTrs.rotation = Quaternion.Euler(Vector3.forward * angle);
        }
    }
}
