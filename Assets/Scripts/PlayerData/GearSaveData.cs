using System;
using Items;
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

        [JsonIgnore] public bool IsMaxLevel => Level >= MaxLevel;

        private GearLevel(int level, int experience)
        {
            Level = level;
            Experience = experience;
        }

        public GearLevel IncreaseExp(int amount, Rarity rarity)
        {
            var newLevel = Level;
            var newExp = Experience + amount;
            while (newExp >= RequiredExp(rarity) && !IsMaxLevel)
            {
                newExp -= RequiredExp(rarity);
                newLevel++;
            }

            if (newLevel > MaxLevel)
            {
                newLevel = MaxLevel;
                newExp = 0;
            }
            return new GearLevel(newLevel, newExp);
        }

        public float DifferenceToNextLevel(Rarity rarity) => Mathf.InverseLerp(0, RequiredExp(rarity), Experience);

        private int RequiredExp(Rarity rarity) => rarity.expPerLevel;

        [JsonIgnore] public static GearLevel NewGearLevel => new GearLevel(1, 0);
    }
}
