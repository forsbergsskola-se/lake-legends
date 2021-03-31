using UnityEngine;

namespace UI
{
    public class MenuPanel : MonoBehaviour
    {
        public static bool AllAreClosed => _numberOfOpenPanels == 0;
        private static int _numberOfOpenPanels;
        private void OnEnable()
        {
            _numberOfOpenPanels += 1;
        }

        private void OnDisable()
        {
            _numberOfOpenPanels -= 1;
        }

        private void DebugState()
        {
            Debug.Log($"Number Of Panels Open Is {_numberOfOpenPanels}\n All Closed Is Now {AllAreClosed}");
        }
    }
}