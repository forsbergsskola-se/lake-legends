using UnityEditor;
using UnityEngine;

namespace EditorExtensions
{
    [CustomEditor(typeof(ToolbarCustomization))]
    public class PlayButtonCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Convert Scene Names To Paths"))
            {
                var buttonColors = serializedObject.targetObject as ToolbarCustomization;
                buttonColors.Validate();
            }
        }
    }
}