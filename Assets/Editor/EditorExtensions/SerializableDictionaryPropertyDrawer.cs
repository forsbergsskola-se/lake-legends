using UI;
using UnityEditor;

namespace EditorExtensions
{
    [CustomPropertyDrawer(typeof(StringSoDictionary)), CustomPropertyDrawer(typeof(LootBoxGoDictionary)), CustomPropertyDrawer(typeof(LootBoxStringDictionary))]
    public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}
}