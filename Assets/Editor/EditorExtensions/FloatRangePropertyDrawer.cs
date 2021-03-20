using UnityEditor;
using UnityEngine;

namespace EditorExtensions
{
    [CustomPropertyDrawer(typeof(FloatRange))]
    public class FloatRangePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            var offset = position.width / 11f;
            var minLabel = new Rect(position.x, position.y, offset, position.height);
            var min = new Rect(minLabel.x + offset, position.y, position.width / 3f, position.height);
            var maxLabel = new Rect(position.x + min.width + offset * 2, position.y, offset, position.height);
            var max = new Rect(maxLabel.x + offset, position.y, position.width / 3f, position.height);
            EditorGUI.LabelField(minLabel, "Min");
            EditorGUI.LabelField(maxLabel, "Max");
            EditorGUI.PropertyField(min, property.FindPropertyRelative("min"), GUIContent.none);
            EditorGUI.PropertyField(max, property.FindPropertyRelative("max"), GUIContent.none);
            
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}