using Items.Gear;

namespace UI
{
    public class FusionInformation
    {
        public readonly int RarityValue;
        public readonly bool FusionIsOpen;
        public readonly EquipmentType EquipmentType;

        public FusionInformation(bool fusionIsOpen, int rarityValue = 0, EquipmentType equipmentType = null)
        {
            RarityValue = rarityValue;
            FusionIsOpen = fusionIsOpen;
            EquipmentType = equipmentType;
        }
    }
}