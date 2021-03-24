using System;
using Items.Gear;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PlayerData
{
    [Serializable]
    public class GearSaveData
    {
        public float lineStrength;
        public float attraction;
        public float accuracy;
        public GearLevel GearLevel = GearLevel.NewGearLevel;
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

    public struct GearLevel
    {
        public int Level;
        public int Experience;

        private int MaxLevel => 10;
        
        public static GearLevel operator +(GearLevel gearLevel, int amount) => gearLevel.IncreaseExp(amount);
        
        [JsonIgnore] public bool IsMaxLevel => Level == MaxLevel;

        private GearLevel(int level, int experience)
        {
            Level = level;
            Experience = experience;
        }

        private GearLevel IncreaseExp(int amount)
        {
            var newLevel = Level;
            var newExp = Experience + amount;
            while (newExp >= RequiredExp(newLevel) && !IsMaxLevel)
            {
                newExp -= RequiredExp(newLevel);
                newLevel++;
            }
            return new GearLevel(newLevel, newExp);
        }

        [JsonIgnore] public float DifferenceToNextLevel => Mathf.InverseLerp(0, RequiredExp(Level), Experience);

        private int RequiredExp(int level) => 100 * level;

        [JsonIgnore] public static GearLevel NewGearLevel => new GearLevel(1, 0);
    }
}
