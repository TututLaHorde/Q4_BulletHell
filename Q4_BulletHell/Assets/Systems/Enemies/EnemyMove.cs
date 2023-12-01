using UnityEngine;

namespace BH.Enemies
{
    public class EnemyMove : MonoBehaviour
    {
        [SerializeField] private float m_speed;
        private bool m_isClockwise;

        private Transform m_target;
        private Transform m_ownTransform;

        private EnemyController m_controller;
        private EnemyLife m_enemyLife;

        /*-------------------------------------------------------------------*/

        private void OnEnable()
        {
            //set random clockwise
            if (Random.value <= 0.5f)
            {
                m_isClockwise = false;
            }
            else
            {
                m_isClockwise = true;
            }
        }

        private void Start()
        {
            //Get components
            m_target = GetComponentInParent<EnemiesManager>().GetRandomBoss();
            m_ownTransform = transform;

            m_controller = GetComponent<EnemyController>();
            m_enemyLife = m_controller.m_life;
        }

        private void FixedUpdate()
        {
            if (m_controller.m_isAlive)
            {
                MoveAroundTarget();
            }
        }

        /*-------------------------------------------------------------------*/

        private void MoveAroundTarget()
        {
            //calculate the direction
            Vector3 targetDir = m_target.position - m_ownTransform.position;
            Vector3 dir = targetDir * 0.25f;

            //go clockwise or not
            if (m_isClockwise)
            {
                dir.x -= targetDir.y;
                dir.y += targetDir.x;
            }
            else
            {
                dir.x += targetDir.y;
                dir.y -= targetDir.x;
            }

            //speed multiplyer depending on life
            float multiplier = 1f;
            if (m_enemyLife.m_lifeRatio <= 0.5f)
            {
                multiplier = 2f;
            }

            //apply movement
            m_ownTransform.position += dir.normalized * m_speed * multiplier * Time.deltaTime;
        }
    }
}

