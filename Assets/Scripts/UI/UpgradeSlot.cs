using System;
using System.Collections.Generic;
using Events;
using Items;
using Items.Gear;
using PlayerData;
using Sacrifice;
using UnityEngine;
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
            
        }
    }
}