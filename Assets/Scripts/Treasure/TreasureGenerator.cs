using System;
using PlayerData;
using UnityEngine;


namespace Treasure 
{
    public class TreasureGenerator : MonoBehaviour
    {
        public LootBox treasureLootBox;


        public void GenerateTreasure()

        {
            var treasure = treasureLootBox.GenerateLoot();
            FindObjectOfType<InventoryHandler>().AddItemToInventory(treasure);
        }

        public void AddTreasure()
        {
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K)) 
            {
               GenerateTreasure(); 
            }
        }
    }
    
    
    
    
}