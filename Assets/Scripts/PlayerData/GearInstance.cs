using Items;
using Items.Gear;
using UnityEngine;

namespace PlayerData
{
    public class GearInstance : MonoBehaviour
    {
        private GearSaveData gearSaveData;
        private Equipment equipment;

        private float CalculatedLineStrength => Mathf.Lerp(Equipment.lineStrength.Min, Equipment.lineStrength.Max, gearSaveData.LineStrength);
        private float CalculatedAttraction => Mathf.Lerp(Equipment.attraction.Min, Equipment.attraction.Max, gearSaveData.Attraction);
        private float CalculatedAccuracy => Mathf.Lerp(Equipment.accuracy.Min, Equipment.accuracy.Max, gearSaveData.Accuracy);

        private Equipment Equipment
        {
            get
            {
                if (equipment == null) equipment = AllItems.ItemIndexer.indexer[gearSaveData.equipID] as Equipment;
                return equipment;
            }
        }

        public GearInstance(GearSaveData gearSaveData)
        {
            this.gearSaveData = gearSaveData;
        }
    }
}
