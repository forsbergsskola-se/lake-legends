using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class InventorySlot : MonoBehaviour
    {
        public void Setup(IItem item)
        {
            Item = item;
            GetComponentInChildren<Text>().text = item.Name;
        }

        public IItem Item { get; private set; }
    }
}