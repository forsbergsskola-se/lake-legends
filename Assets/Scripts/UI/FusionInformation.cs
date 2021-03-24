using Items.Gear;
using PlayerData;

namespace UI
{
    public class FusionInformation
    {
        public readonly int RarityValue;
        public readonly bool FusionIsOpen;
        public readonly EquipmentType EquipmentType;
        public readonly GearInstance GearInstance;

        public FusionInformation(bool fusionIsOpen, GearInstance gearInstance = null, int rarityValue = 0, EquipmentType equipmentType = null)
        {
            RarityValue = rarityValue;
            FusionIsOpen = fusionIsOpen;
            EquipmentType = equipmentType;
            GearInstance = gearInstance;
        }
    }
}