using BH.Game;
using System.Collections.Generic;
using UnityEngine;

namespace BH.Enemies
{
    [RequireComponent (typeof (EnemiesSpawner))]

    public class EnemiesManager : MonoBehaviour
    {
        private List<EnemyController> m_enemies = new();
        private EnemiesSpawner m_spawner;

        /*-------------------------------------------------------------------*/

        private void Start()
        {
            //get all spawned enemies
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).TryGetComponent(out EnemyController enemy))
                {
                    m_enemies.Add(enemy);
                }
            }

            //get own components
            m_spawner = GetComponent<EnemiesSpawner>();
        }

        /*-------------------------------------------------------------------*/

        public void AnEnemyDie(EnemyController enemy, bool isLastBoss)
        {
            if (m_enemies.Contains(enemy))
            {
                //player win if the last enemy die
                if (isLastBoss)
                {
                    GameManager.instance.LastEnemyDie();
                }
                //kill boss
                else if (enemy.IsBossEnemy())
                {
                    enemy.gameObject.SetActive(false);
                }
                //kill enemy
                else
                {
                    m_spawner.EnemyDie(enemy);
                    m_enemies.Remove(enemy);
                }
            }
        }

        public void AnEnemySpawn(EnemyController enemy)
        {
            m_enemies.Add(enemy);
        }

        public int GetNbEnemyAlive()
        {
            return m_enemies.Count;
        }

        public bool IsLastBoss(EnemyController enemy)
        {
            if (m_enemies.Contains(enemy) && enemy.IsBossEnemy())
            {
                //count the alive enemies
                int nbBossAlive = 0;
                foreach (var en in m_enemies)
                {
                    if (en.gameObject.activeSelf && en.IsBossEnemy())
                    {
                        nbBossAlive++;
                    }
                }

                //last enemy die
                if (nbBossAlive == 1)
                {
                    return true;
                }
            }

            return false;
        }

        public EnemyController GetClosestEnemy(Vector3 position)
        {
            EnemyController enemy = null;
            float distance = float.MaxValue;

            foreach (var en in m_enemies)
            {
                //ignore desactive and dead enemies
                if (!en.gameObject.activeSelf || !en.m_isAlive)
                {
                    continue;
                }

                //get the closest
                float dist = Vector3.Distance(en.transform.position, position);
                if (dist <= distance)
                {
                    enemy = en;
                    distance = dist;
                }
            }

            return enemy;
        }

        public Transform GetRandomBoss()
        {
            List<EnemyController> bosses = new();

            //get a only boss
            for (int i = 0; i < m_enemies.Count; i++)
            {
                if (m_enemies[i].IsBossEnemy())
                {
                    bosses.Add(m_enemies[i]);
                }
            }

            //return random bosses
            int index = Random.Range(0, bosses.Count);
            return bosses[index].transform;
        }
    }
}
