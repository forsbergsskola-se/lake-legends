using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialPopup : MonoBehaviour
    {
        [SerializeField] private Text textArea;
        [SerializeField] private Button confirmButton;

        public void Setup(Message message)
        {
            textArea.text = message.GetMessage();
            confirmButton.onClick.AddListener(Close);
        }

        private void Close()
        {
            Destroy(this.gameObject);
        }
    }
}