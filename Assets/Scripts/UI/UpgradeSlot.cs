using System;
using Items;
using PlayerData;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UpgradeSlot : Slot
    {
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

        private void OnUpgrade()
        {
            
        }
    }
}