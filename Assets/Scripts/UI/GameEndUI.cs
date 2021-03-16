using EventManagement;
using Events;
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
        public ICatchable catchable;
        public IMessageHandler eventsBroker;
        
        void OnEnable()
        {
            UpdateUI();
        }

        private void Update()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (catchable != null)
            {
                resultText.text = "You caught something!";
                image.gameObject.SetActive(true);
                if (catchable is FishItem fishItem)
                {
                    image.sprite = fishItem.type.sprite;
                    fishNameText.text = fishItem.Name;
                    fishWorth.text = $"It's worth {fishItem.silverValue}!";   
                }
                else
                {
                    fishNameText.text = catchable.Name;
                }
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
            catchable = null; 
            resultText.text = "";
            fishNameText.text = "";
            fishWorth.text = "";
        }
    }
}