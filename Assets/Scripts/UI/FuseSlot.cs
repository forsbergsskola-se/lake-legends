using Items;
using PlayerData;
using UnityEngine.UI;

namespace UI
{
    public class FuseSlot : Slot
    {
        // Copy of SacrificeSlot.cs
        
        string currentGearInstance;
        private string currentGearInstanceID;
        public GearInstance gearInstance;
        
        private Text gearNameText;
        
        private void Start()
        {
            //gearNameText = gameObject.GetComponentInChildren<Text>();
            //gearNameText.text = string.Empty;
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
            SetDefaultImages();
            ApplyImages();
        }
    }
}