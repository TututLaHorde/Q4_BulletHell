using System.Collections.Generic;
using UnityEngine;

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
