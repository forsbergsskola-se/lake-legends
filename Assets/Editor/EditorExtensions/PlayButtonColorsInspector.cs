using UnityEditor;
using UnityEngine;

namespace EditorExtensions
{
    [CustomEditor(typeof(ToolbarCustomization))]
    public class ToolbarCustomizationInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Convert Scene Names To Paths"))
            {
                var buttonColors = serializedObject.targetObject as ToolbarCustomization;
                buttonColors.Validate();
            }
            if (GUILayout.Button("Update Settings"))
            {
                var buttonColors = serializedObject.targetObject as ToolbarCustomization;
                buttonColors.UpdateSettings();
            }
        }
    }
}