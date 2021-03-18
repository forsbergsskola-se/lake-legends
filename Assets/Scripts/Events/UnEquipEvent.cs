using Items.Gear;
using PlayerData;

namespace Events
{
    public class UnEquipEvent
    {
        public GearInstance Equippable;

        public UnEquipEvent(GearInstance equippable) => this.Equippable = equippable;
    }
}