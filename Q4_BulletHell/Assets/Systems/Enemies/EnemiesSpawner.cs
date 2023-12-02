using BH.Game;
using BH.Tools;

using System.Collections.Generic;
using UnityEngine;

namespace BH.Enemies
{
    public class EnemiesSpawner : MonoBehaviour
    {
        [Header("Enemies")]
        [SerializeField] private List<GameObject> m_pbEnemies = new();
        [SerializeField][Min(0f)] private int m_nbEnemiesMax;

        [Header("Spawn interval")]
        [SerializeField][Min(0f)] private float m_minIntervalTime;
        [SerializeField][Min(0f)] private float m_maxIntervalTime;
        [SerializeField][Min(0f)] private float m_timeToMinInterval;
        [SerializeField] private AnimationCurve m_intervalCurve;
        private float m_currIntervalTime;
        private float m_timeFromPrevious;

        private List<PoolingManager<EnemyController>> m_pooling = new();
        private EnemiesManager m_manager;

        /*-------------------------------------------------------------------*/

        private void Awake()
        {
            m_currIntervalTime = m_maxIntervalTime;

            if (m_minIntervalTime > m_maxIntervalTime)
            {
                m_minIntervalTime = m_maxIntervalTime;
                Debug.Log("Min intervale time must be smaller than max");
            }
        }

        private void Start()
        {
            //preallocate enemies with pooling system
            for (int i = 0;  i < m_pbEnemies.Count; i++)
            {
                int preAllocNb = Mathf.CeilToInt((float)m_nbEnemiesMax / m_pbEnemies.Count);
                m_pooling.Add(new PoolingManager<EnemyController>(m_pbEnemies[i], transform, preAllocNb));
            }

            //get own components
            m_manager = GetComponent<EnemiesManager>();
        }

        private void Update()
        {
            //spawn there is place for new enemies
            if (m_pbEnemies.Count > 0 && m_nbEnemiesMax > m_manager.GetNbEnemyAlive())
            {
                //spawn interval
                if (m_currIntervalTime <= 0)
                {
                    SetIntervalTime();
                    SpawnRandomEnemy();
                }
                else
                {
                    m_currIntervalTime -= Time.deltaTime;
                }

                //for interval difficulty 
                if (m_timeFromPrevious < m_timeToMinInterval)
                {
                    m_timeFromPrevious += Time.deltaTime;
                    m_timeFromPrevious = Mathf.Clamp(m_timeFromPrevious, 0, m_timeToMinInterval);
                }
            }
        }

        /*-------------------------------------------------------------------*/

        public void EnemyDie(EnemyController enemy)
        {
            m_pooling[enemy.m_poolingIndex].StopUsing(enemy);
        }

        /*-------------------------------------------------------------------*/

        private void SpawnRandomEnemy()
        {
            //choose random enemies to spawn
            int enemyIndex = Random.Range(0, m_pbEnemies.Count);

            //choose random point around the border of the map
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
            pos *= GameArea.instance.GetBulletRadius();

            //spawn at the right position
            EnemyController m_newEnemy = m_pooling[enemyIndex].UseNew();
            m_newEnemy.transform.position = pos;
               
            //Init the enemy
            m_manager.AnEnemySpawn(m_newEnemy);
            m_newEnemy.Init(enemyIndex);
        }

        private void SetIntervalTime()
        {
            //lerp with the difficulty curve
            float lerpValue = m_timeFromPrevious / m_timeToMinInterval;
            lerpValue = m_intervalCurve.Evaluate(lerpValue);

            m_currIntervalTime = Mathf.Lerp(m_maxIntervalTime, m_minIntervalTime, lerpValue);
        }
    }
}
