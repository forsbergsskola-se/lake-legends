using Items;
using PlayerData;
using UnityEngine.UI;

namespace UI
{
    public class SacrificeSlot : Slot
    {
        string currentGearInstance;
        public GearInstance gearInstance;
        
        private Text gearNameText;
        
        private void Start()
        {
            SetDefaultImages();
        }
        
        private void FixedUpdate()
        {
            //gearNameText.text = gearInstance != null ? gearInstance.Name : string.Empty;
        }
        
        public void ClearName()
        {
            //gearNameText.text = string.Empty;
        }
        
        public override void Setup(IItem item, bool hasCaught = true)
        {
            gearInstance = item as GearInstance;
            Item = item;
            ApplyImages();
        }
    }
}