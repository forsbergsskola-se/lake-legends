using Items;
using UnityEngine;

namespace PlayerData
{
    public class PlayerEquipment : MonoBehaviour
    {
        [SerializeField] private IEquipmentSlot[] equipmentTypes;
        // GUID of items can be used to save/load what item is in the corresponding slot
        
        // Add EquipmentSaver in InventoryHandler
        
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
