using Auth;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EULA : MonoBehaviour
    {
        public Button confirm;
        public Button decline;

        public void SetUp(InitializeHandler initializeHandler)
        {
            confirm.onClick.AddListener(() => initializeHandler.HasAcceptedEula = true);
            decline.onClick.AddListener(Quit);
        }

        private void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}