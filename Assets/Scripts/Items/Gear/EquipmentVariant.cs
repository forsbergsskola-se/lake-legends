using UnityEngine;

namespace Items.Gear
{
    [CreateAssetMenu(fileName = "New Equipment Variant", menuName =  "ScriptableObjects/Equipment Variant")]
    public class EquipmentVariant : ScriptableObject
    {
        [SerializeField] private EquipmentType equipmentType;
        public EquipmentType EquipmentType => equipmentType;
    }
}