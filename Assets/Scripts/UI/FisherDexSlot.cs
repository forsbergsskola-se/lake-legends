using Items;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class FisherDexSlot : Slot, IPointerClickHandler
    {
        private bool hasCaught;
        public override void Setup(IItem item, bool hasCaught = true)
        {
            this.hasCaught = hasCaught;
            Item = item;
            if (hasCaught)
                GetComponentInChildren<Text>().text = item.Name;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (hasCaught)
                FindObjectOfType<BioArea>().Setup(Item as FishItem);
        }
    }
}