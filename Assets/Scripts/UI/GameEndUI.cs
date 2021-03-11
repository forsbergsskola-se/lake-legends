using EventManagement;
using Items;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class GameEndUI : MonoBehaviour
    {
        [SerializeField] private Text resultText, fishNameText, fishWorth;
        [SerializeField] private Image image;
        public FishItem fish;
        public IMessageHandler eventsBroker;
        
        void OnEnable()
        {
            UpdateUI();
        }
        
        private void UpdateUI()
        {
            if (fish != null)
            {
                resultText.text = "You caught something!";
                // TODO: Add image of caught fish sprite here
                //image.sprite = fish.type.sprite;
                fishNameText.text = fish.Name;
                fishWorth.text = $"It's worth {fish.goldValue} !";
            }
            else
            {
                resultText.text = "Oh no! It got away!";
                image.gameObject.SetActive(false);
                fishNameText.text = "";
                fishWorth.text = "";
            }
        }

        public void FishAgain()
        {
            eventsBroker.Publish(new FishAgainEvent());
            this.gameObject.SetActive(false);
        }

        public void BackToHub()
        {
            SceneManager.LoadScene("MainUI");
            this.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            eventsBroker = null;
            if (fish != null) fish = null; 
            resultText.text = "";
            fishNameText.text = "";
            fishWorth.text = "";
        }
    }
}