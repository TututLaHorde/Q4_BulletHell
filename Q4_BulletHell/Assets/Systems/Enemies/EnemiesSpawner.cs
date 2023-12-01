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
        [SerializeField][Min(0f)] private float m_timeToMaxDifficulty;
        private float m_intervalTime = 6f;
        private float m_currentTime = 6f;

        private List<PoolingManager<EnemyController>> m_pooling = new();
        private EnemiesManager m_manager;

        /*-------------------------------------------------------------------*/

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
            if (m_pbEnemies.Count > 0 && m_nbEnemiesMax > m_manager.GetNbEnemyAlive())
            {
                if (m_currentTime <= 0)
                {
                    m_currentTime = m_intervalTime;
                    SpawnRandomEnemy();
                }
                else
                {
                    m_currentTime -= Time.deltaTime;
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
            Vector3 pos = new Vector3(4f, 4f, 0f);

            //spawn at the right position
            EnemyController m_newEnemy = m_pooling[enemyIndex].UseNew();
            m_newEnemy.transform.position = pos;
               
            //Init the enemy
            m_manager.AnEnemySpawn(m_newEnemy);
            m_newEnemy.Init(enemyIndex);
        }
    }
}
