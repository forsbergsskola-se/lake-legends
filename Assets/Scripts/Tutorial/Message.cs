using JetBrains.Annotations;
using UnityEngine;

namespace Tutorial
{
    public class Message : ScriptableObject
    {
        [TextArea] [SerializeField] private string message;

        private bool WasTriggered
        {
            get
            {
                var num = PlayerPrefs.GetInt(name, 0);
                return num != 0;
            }
            set
            {
                var num = value == false ? 0 : 1;
                PlayerPrefs.SetInt(name, num);
            }
        }

        [CanBeNull]
        public string GetMessage()
        {
            if (WasTriggered) return null;
            WasTriggered = true;
            return message;
        }

        public void Init(string content)
        {
            message = content;
        }

        public void ResetTutorial()
        {
            WasTriggered = false;
        }
    }
}