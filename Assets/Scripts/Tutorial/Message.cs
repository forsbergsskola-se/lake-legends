using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Tutorial
{
    public class Message : ScriptableObject
    {
        [TextArea] [SerializeField] private string message;
        [SerializeField] private string buttonText;

        public bool WasTriggered
        {
            get
            {
                var num = PlayerPrefs.GetInt(name, 0);
                return num != 0;
            }
            private set
            {
                var num = value == false ? 0 : 1;
                PlayerPrefs.SetInt(name, num);
            }
        }

        [CanBeNull]
        public string GetMessage()
        {
            WasTriggered = true;
            return message;
        }
        
        public string GetButtonText()
        {
            WasTriggered = true;
            return buttonText;
        }

        public void Init(string content, string buttonContent)
        {
            message = content;
            buttonText = buttonContent;
        }

        public void ResetTutorial()
        {
            WasTriggered = false;
        }
    }
}