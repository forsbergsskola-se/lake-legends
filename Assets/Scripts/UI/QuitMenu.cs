using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class QuitMenu : MonoBehaviour
    {
        public Button confirm;
        public Button cancel;
        public void SetUp(BackButton backButton)
        {
            confirm.onClick.AddListener(backButton.Quit);
            cancel.onClick.AddListener(backButton.Cancel);
        }
    }
}