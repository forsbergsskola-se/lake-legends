using PlayerData;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerFullStatusViewUI : MonoBehaviour
    {
        [SerializeField] private Text lineStrenghtText;
        [SerializeField] private Text attractionText;
        [SerializeField] private Text accuracyText;

        private PlayerBody playerBody;

        void Start()
        {
            playerBody = FindObjectOfType<PlayerBody>();
        }
    
        void Update()
        {
            lineStrenghtText.text = $"Line Strength: {playerBody.TotalLineStrength:F0}";
            attractionText.text = $"Attraction: {playerBody.TotalAttraction:F0}";
            accuracyText.text = $"Accuracy: {playerBody.TotalAccuracy:F0}";
        }
    }
}
