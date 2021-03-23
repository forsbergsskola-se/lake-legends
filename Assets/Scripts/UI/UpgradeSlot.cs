using Items;
using Items.Gear;
using UnityEngine.UI;

namespace UI
{
    public class UpgradeSlot : Slot
    {
        public override void Setup(IItem item, bool hasCaught = true)
        {
            Item = item;
            GetComponentInChildren<Text>().text = item.Name;
            if (Item is IEquippable equippable)
            {
                equippable.PlaceInUpgrade += OnPlaceInUpgradeItem;
            }
        }

        private void OnUpgrade()
        {
            /*GetComponentInChildren<Text>().text = "{Empty}";
            Item = null;*/
        }

        private void OnPlaceInUpgradeItem()
        {
            GetComponent<Text>().text = Item.Name;
        }
    }
}