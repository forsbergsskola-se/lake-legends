using UnityEditor;
using UnityEngine;

namespace EditorExtensions
{
    [CustomPropertyDrawer(typeof(StringSoDictionary))]
    public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}
}