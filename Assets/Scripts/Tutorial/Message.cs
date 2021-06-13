using System;
using JetBrains.Annotations;
using Tutorial.Redirection;
using UnityEngine;

namespace Tutorial
{
    public class Message : ScriptableObject
    {
        [TextArea] [SerializeField] private string message;
        [SerializeField] private TutorialButtonDefinition[] tutorialButtons;
        public bool shouldPauseGame;

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
        
        public TutorialButtonDefinition[] GetButtons()
        {
            return tutorialButtons;
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

    [Serializable]
    public class TutorialButtonDefinition
    {
        public string buttonText;
        public RedirectionDefinition redirectionDefinition;
    }
}