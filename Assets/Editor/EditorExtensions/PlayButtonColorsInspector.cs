using UnityEditor;
using UnityEngine;

namespace EditorExtensions
{
    [CustomEditor(typeof(PlayButtonColors))]
    public class PlayButtonCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Update"))
            {
                var buttonColors = serializedObject.targetObject as PlayButtonColors;
                buttonColors.CallValueChange();
            }
        }
    }
}