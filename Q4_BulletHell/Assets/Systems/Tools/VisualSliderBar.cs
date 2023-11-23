using UnityEngine;

namespace BH.Tools
{
    public class VisualSliderBar : MonoBehaviour
    {
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
            Debug.Log(m_ratio * m_anchorMax.x + " , " + m_anchorMax.y);
            m_fillingTrs.anchorMax = new Vector2(m_ratio * m_anchorMax.x, m_anchorMax.y);
        }
    }
}
