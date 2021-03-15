using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Items
{
    public class InventorySlot : MonoBehaviour, IPointerClickHandler
    {
        public void Setup(IItem item)
        {
            Item = item;
            GetComponentInChildren<Text>().text = item.Name;
        }

        public IItem Item { get; private set; }
        public void OnPointerClick(PointerEventData eventData)
        {
            Item?.Use();
        }
    }
}