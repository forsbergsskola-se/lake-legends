using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialPopup : MonoBehaviour
    {
        [SerializeField] private Text textArea;
        [SerializeField] private Transform buttonArea;
        [SerializeField] private Button buttonPrefab;

        public void Setup(Message message)
        {
            textArea.text = message.GetMessage();
            var buttons = message.GetButtons();
            foreach (var buttonDefinition in buttons)
            {
                var button = Instantiate(buttonPrefab, buttonArea);
                button.GetComponentInChildren<Text>().text = buttonDefinition.buttonText;
                button.onClick.AddListener(() => buttonDefinition.redirectionDefinition.Redirect(this));
            }
        }
    }
}