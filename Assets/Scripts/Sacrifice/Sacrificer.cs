using UnityEngine;

namespace Sacrifice
{
    public class Sacrificer : MonoBehaviour
    {
        public void TogglePanel(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void TogglePanelOn()
        {
            gameObject.SetActive(true);
        }
    }
}