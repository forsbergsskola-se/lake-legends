using Items;
using UnityEngine.UI;

namespace UI
{
    public class UpgradeSlot : Slot
    {
        string currentGearInstance;
        public string CurrentGearInstance
        {
            get => currentGearInstance;
            set
            {
                currentGearInstance = value;
                GetComponentInChildren<Text>().text = currentGearInstance;
            }
        }
        
        public override void Setup(IItem item, bool hasCaught = true)
        {
            Item = item;
            GetComponentInChildren<Text>().text = item.Name;
        }

        private void OnUpgrade()
        {
            
        }
    }
}