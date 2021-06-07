using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialPopup : MonoBehaviour
    {
        [SerializeField] private Text textArea;
        [SerializeField] private Text buttonText;
        [SerializeField] private Button confirmButton;

        public void Setup(Message message)
        {
            textArea.text = message.GetMessage();
            buttonText.text = message.GetButtonText();
            confirmButton.onClick.AddListener(Close);
        }

        private void Close()
        {
            Destroy(this.gameObject);
        }
    }
}