using System;
using PlayerData;
using UnityEngine;


namespace Treasure 
{
    public class TreasureGenerator : MonoBehaviour
    {
        public Factory treasureFactory;


        public void GenerateTreasure()

        {
            var treasure = treasureFactory.GenerateTreasure();
            FindObjectOfType<InventoryHandler>().AddItemToInventory(treasure);
        }

        public void AddTreasure()
        {
            FindObjectOfType<InventoryHandler>().AddItemToInventory(treasureFactory);
            
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