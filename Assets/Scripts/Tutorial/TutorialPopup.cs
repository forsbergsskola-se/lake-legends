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
            if (message.shouldPauseGame)
                Pause();
            textArea.text = message.GetMessage();
            var buttons = message.GetButtons();
            foreach (var buttonDefinition in buttons)
            {
                var button = Instantiate(buttonPrefab, buttonArea);
                button.GetComponentInChildren<Text>().text = buttonDefinition.buttonText;
                button.onClick.AddListener(() => buttonDefinition.redirectionDefinition.Redirect(this));
            }
        }

        public void Pause()
        {
            Time.timeScale = 0;
        }
        
        public void UnPause()
        {
            Time.timeScale = 1;
        }
    }
}