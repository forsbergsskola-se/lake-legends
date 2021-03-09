using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(StringSoDictionary))]
    public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}
}