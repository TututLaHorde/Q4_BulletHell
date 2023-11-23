using System.Collections.Generic;
using UnityEngine;

namespace BH.Tools
{
    public class PoolingManager<T> where T : Component
    {
        private readonly GameObject m_prefab;
        private readonly Transform m_parentTrs;

        private Stack<T> m_pools;

        /*-------------------------------------------------------------------*/

        public PoolingManager(GameObject prefab, Transform componentsParentTrs, int nbPreAllocateObj = 0)
        {
            m_prefab = prefab;
            m_parentTrs = componentsParentTrs;

            m_pools = new Stack<T>(nbPreAllocateObj);
            for (int i = 0; i < nbPreAllocateObj; i++)
            {
                SpawnComponent();
            }
        }

        public T UseNew()
        {
            //there isn't a disponible T
            if (m_pools.Count == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    SpawnComponent();
                }
            }

            //Need to reset params
            return m_pools.Pop();
        }

        public void StopUsing(T component)
        {
            component.gameObject.SetActive(false);
            m_pools.Push(component);
        }

        /*-------------------------------------------------------------------*/

        private void SpawnComponent()
        {
            T component = GameObject.Instantiate(m_prefab, m_parentTrs).GetComponent<T>();
            component.gameObject.SetActive(false);

            m_pools.Push(component);
        }
    }
}

