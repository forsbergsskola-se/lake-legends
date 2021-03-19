using System;
using EventManagement;
using Events;
using PlayerData;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Items.Gear
{
    public class GearInfoPanel : MonoBehaviour
    {
        public Text title;
        public Text rarity;
        public Text equipmentType;
        public Text lineStrength;
        public Text attraction;
        public Text accuracy;
        public GameObject panel;

        private GearInstance gearInstance;

        public void Setup(GearInstance gearInstance)
        {
            if(gearInstance == null) return;
            this.gearInstance = gearInstance;
            title.text = gearInstance.Name;
            rarity.text = gearInstance.Rarity.ToString();
            equipmentType.text = gearInstance.EquipmentType.ToString();
            lineStrength.text = gearInstance.CalculatedLineStrength.ToString();
            attraction.text = gearInstance.CalculatedAttraction.ToString();
            accuracy.text = gearInstance.CalculatedAccuracy.ToString();
        }

        public void DoEquip()
        {
            var broker = Object.FindObjectOfType<EventsBroker>();
            broker.Publish(new CheckAndDoEquipEvent(gearInstance));
        }
    }
}