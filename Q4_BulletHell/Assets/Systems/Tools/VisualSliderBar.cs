using UnityEngine;

namespace BH.Tools
{
    public class VisualSliderBar : MonoBehaviour
    {
        //[SerializeField][Range(0f, 1f)] float m_value = 0f;
        public float m_ratio { get; private set; }

        [SerializeField] private RectTransform m_fillingTrs;

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
            m_fillingTrs.localScale = new Vector2(m_ratio, 1f);
            //m_fillingTrs.localScale = new Vector2(m_value, 1f);
        }
    }
}
