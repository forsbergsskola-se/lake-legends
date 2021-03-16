using System;
using Items.Gear;
using Newtonsoft.Json;
using Random = UnityEngine.Random;

namespace PlayerData
{
    [Serializable]
    public class GearSaveData
    {
        public float lineStrength;
        public float attraction;
        public float accuracy;
        public int level = 1;
        public float experience = 0;
        
        public string equipID;
        public string instanceID;

        public GearSaveData(Equipment equipment)
        {
            lineStrength = Random.Range(0f, 1f);
            attraction = Random.Range(0f, 1f);
            accuracy = Random.Range(0f, 1f);

            equipID = equipment.ID;
            instanceID = Guid.NewGuid().ToString();
        }
        [JsonConstructor]
        public GearSaveData()
        {

        }
    }
}
