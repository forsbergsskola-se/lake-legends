using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialRecap : MonoBehaviour
    {
        public Text content;
        public Text title;
        public Button prefabButton;
        public Transform buttonAnchor;
        public Message[] tutorialMessages;
        private List<Button> buttons = new List<Button>();

        private void OnEnable()
        {
            foreach (var message in tutorialMessages)
            {
                if (!message.WasTriggered)
                    continue;
                var button = Instantiate(prefabButton, buttonAnchor);
                button.onClick.AddListener(() => Show(message));
                buttons.Add(button);
                button.GetComponentInChildren<Text>().text = message.name;
            }
        }
        
        private void OnDisable()
        {
            foreach (var button in buttons)
            {
                Destroy(button.gameObject);
            }
            buttons.Clear();
        }

        private void Show(Message message)
        {
            content.text = message.GetMessage();
            title.text = message.name;
        }
    }
}
