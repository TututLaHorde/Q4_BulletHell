using System.Collections.Generic;
using UnityEngine;
using BH.Game;

namespace BH.Enemies
{
    public class EnemiesManager : MonoBehaviour
    {
        List<EnemyController> m_enemies = new();

        private void Start()
        {
            //get all enemies
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent(out EnemyController enemy))
                {
                    m_enemies.Add(enemy);
                }
            }
        }

        public void AnEnemyDie(EnemyController enemy)
        {
            if (m_enemies.Contains(enemy))
            {
                //count the alive enemies
                int nbAliveEnemies = 0;
                foreach (var en in m_enemies)
                {
                    if (en.gameObject.activeSelf)
                    {
                        nbAliveEnemies++;
                    }
                }

                //player win if the last enemy die
                if (nbAliveEnemies == 1)
                {
                    GameManager.instance.LastEnemyDie();
                }
                else
                {
                    //kill the enemy
                    enemy.gameObject.SetActive(false);
                }
            }
        }

        public EnemyController GetClosestEnemy(Vector3 position)
        {
            EnemyController enemy = null;
            float distance = float.MaxValue;

            foreach (var en in m_enemies)
            {
                //ignore desactive enemies
                if (!en.gameObject.activeSelf)
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
    }
}
