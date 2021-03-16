using System;
using Items.Gear;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PlayerData
{
    [Serializable]
    public class GearSaveData : MonoBehaviour
    {
        public readonly float LineStrength;
        public readonly float Attraction;
        public readonly float Accuracy;
        public readonly int Level = 1;
        public readonly float Experience = 0;
        
        public string equipID;

        public GearSaveData(Equipment equipment)
        {
            LineStrength = Random.Range(0f, 1f);
            Attraction = Random.Range(0f, 1f);
            Accuracy = Random.Range(0f, 1f);

            equipID = equipment.ID;
        }
        
    }
}
