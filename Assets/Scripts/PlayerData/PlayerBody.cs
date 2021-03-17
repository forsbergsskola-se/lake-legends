using Items;
using UnityEngine;

namespace PlayerData
{
    public class PlayerBody : MonoBehaviour
    {
        [SerializeField] private BodyPart[] bodyParts;

        private float totalLineStrength;
        private float totalAttraction;
        private float totalAccuracy;

        public float TotalLineStrength
        {
            get
            {
                totalLineStrength = 0f;
                foreach (var bodyPart in bodyParts)
                {
                    if(bodyPart.EquippedItem is GearInstance reference) 
                        totalLineStrength += reference.CalculatedLineStrength;
                }

                return totalLineStrength;
            }   
        }

        public float TotalAttraction
        {
            get
            {
                totalAttraction = 0f;
                foreach (var bodyPart in bodyParts)
                {
                    if(bodyPart.EquippedItem is GearInstance reference) 
                        totalAttraction += reference.CalculatedAttraction;
                }

                return totalAttraction;
            }   
        }
        
        public float TotalAccuracy
        {
            get
            {
                totalAccuracy = 0f;
                foreach (var bodyPart in bodyParts)
                {
                    if(bodyPart.EquippedItem is GearInstance reference) 
                        totalAccuracy += reference.CalculatedAccuracy;
                }
                return totalAccuracy;
            }   
        }
    }
}