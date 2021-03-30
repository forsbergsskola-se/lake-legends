using UI;
using UnityEditor;

namespace EditorExtensions
{
    [CustomPropertyDrawer(typeof(StringSoDictionary)), CustomPropertyDrawer(typeof(LootBoxGoDictionary))]
    public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}
}