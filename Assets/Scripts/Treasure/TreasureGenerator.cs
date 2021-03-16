using System;
using PlayerData;
using UnityEngine;


namespace Treasure 
{
    public class TreasureGenerator : MonoBehaviour
    {
        public TreasureFactory treasureTreasureFactory;


        public void GenerateTreasure()

        {
            var treasure = treasureTreasureFactory.GenerateTreasure();
            FindObjectOfType<InventoryHandler>().AddItemToInventory(treasure);
        }

        public void AddTreasure()
        {
            FindObjectOfType<InventoryHandler>().AddItemToInventory(treasureTreasureFactory);
            
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