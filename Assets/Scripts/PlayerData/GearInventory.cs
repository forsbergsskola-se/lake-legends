using System.Collections.Generic;
using System.Threading.Tasks;
using Saving;
using UnityEngine;

namespace PlayerData
{
    public class GearInventory
    {
        private readonly IGearSaver saver;
        private Dictionary<string, GearInstance> generatedGear = new Dictionary<string, GearInstance>();
        private const string GearKey = "Gear";

        public Dictionary<string, GearInstance> GeneratedGear => generatedGear;

        public GearInventory(IGearSaver saver)
        {
            this.saver = saver;
        }

        public void AddItem(GearInstance gearInstance)
        {
            GeneratedGear.Add(gearInstance.ID, gearInstance);
        }

        public void RemoveItem(GearInstance gearInstance)
        {
            GeneratedGear.Remove(gearInstance.ID);
        }

        public void PrintInventory()
        {
            Debug.Log(new JsonSerializer().SerializeObject(generatedGear));
        }
        
        public async Task Deserialize()
        {
            var savedInventory = await saver.LoadGear(GearKey);
            if (savedInventory == null)
                return;
            generatedGear = savedInventory;
        }

        public void Serialize()
        {
            saver.SaveGear(GearKey, this);
        }
    }
}