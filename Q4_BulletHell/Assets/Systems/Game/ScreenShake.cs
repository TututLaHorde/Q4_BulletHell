using UnityEngine;

namespace BH.Game
{
    public class ScreenShake : MonoBehaviour
    {
        public static ScreenShake instance;

        public float m_amount;
        [SerializeField] private float m_amountReduction;
        [SerializeField] private float m_amountMax;
        [SerializeField] private float m_shakeSpeed;

        /*-------------------------------------------------------------------*/

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.Log("There is two ScreenShake scripts");
                Destroy(this);
                return;
            }
        }

        void Update()
        {
            //limit amount
            m_amount = Mathf.Clamp(m_amount, 0, m_amountMax);

            if (m_shakeSpeed > 0 && m_amount > 0)
            {
                //random shake
                float x = Mathf.PerlinNoise(Time.time * m_shakeSpeed, 0f);
                float y = Mathf.PerlinNoise(Time.time * m_shakeSpeed, 0.5f);

                //calculate shake
                Vector3 offset = new Vector3(x - 0.5f, y - 0.5f, 0f);
                offset *= m_amount;
                offset.z = transform.position.z;

                //apply shake
                transform.position = offset;

                //reduce amount
                m_amount -= m_amountReduction * Time.deltaTime;
            }
        }
    }
}
