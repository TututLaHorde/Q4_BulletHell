using TMPro;
using UnityEngine;

namespace BH.MenusUI
{
    public class StartingGameCount : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_countText;

        public void SetStartingCount(float count)
        {
            count = Mathf.Ceil(count);

            m_countText.text = count.ToString();
        }
    }
}

