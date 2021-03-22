using UnityEditor;
using UnityEngine;

namespace EditorExtensions
{
    [CustomPropertyDrawer(typeof(ScenePathRef))]
    public class ScenePathRefDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            
            var pathRect = new Rect(position.x, position.y, position.width, position.height);

            EditorGUI.PropertyField(pathRect, property.FindPropertyRelative("path"), GUIContent.none);

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}