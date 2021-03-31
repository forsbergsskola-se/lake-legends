using UI;
using UnityEditor;

namespace EditorExtensions
{
    [CustomPropertyDrawer(typeof(StringSoDictionary)), CustomPropertyDrawer(typeof(LootBoxGoDictionary)), CustomPropertyDrawer(typeof(LootBoxStringDictionary)), CustomPropertyDrawer(typeof(LootBoxColorDictionary))]
    public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}
}