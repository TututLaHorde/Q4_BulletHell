using UnityEngine;

namespace BH.Tools
{
    public class VisualSliderBar : MonoBehaviour
    {
        //[SerializeField]
        //[Range(0f, 1f)] float m_value = 0f;
        public float m_ratio { get; private set; }

        [SerializeField] private RectTransform m_fillingTrs;
        private Vector2 m_anchorMax;

        private void Start()
        {
            m_anchorMax = m_fillingTrs.anchorMax;
        }

        private void Update()
        {
            SetFillingSize();
        }

        public void SetRatio(float ratio)
        {
            m_ratio = Mathf.Clamp01(ratio);
        }

        private void SetFillingSize()
        {
            m_fillingTrs.anchorMax = new Vector2(m_ratio * m_anchorMax.x, m_anchorMax.y);
            //m_fillingTrs.anchorMax = new Vector2(m_value * m_anchorMax.x, m_anchorMax.y);
        }
    }
}
