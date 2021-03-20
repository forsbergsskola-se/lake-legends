using System;
using UnityEngine;

namespace EditorExtensions
{
    public class PlayButtonColors : ScriptableObject
    {
        [SerializeField] private Color buttonColor = Color.white;
        [SerializeField] private Color backGroundColor = new Color(0f, 0.6f, 0);
        [SerializeField] private Color highlightedColor = new Color(0f, 0.8f, 0);
        public event Action AnyValueChange;

        public Color HighlightedColor => highlightedColor;
        
        public Color BackGroundColor => backGroundColor;

        public Color ButtonColor => buttonColor;

        public void CallValueChange()
        {
            AnyValueChange?.Invoke();
        }
    }
}