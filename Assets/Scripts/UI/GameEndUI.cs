using EventManagement;
using Events;
using Items;
using LootBoxes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class GameEndUI : MonoBehaviour
    {
        [SerializeField] private Text resultText, fishNameText, fishWorth;
        [SerializeField] private Image image;
        [SerializeField] private Image frameImage;
        [SerializeField] private Image silverImage;
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
                resultText.text = "You caught a";
                image.gameObject.SetActive(true);
                image.sprite = catchable.Sprite;
                fishNameText.text = catchable.Name;
                
                if (catchable is FishItem fishItem)
                {
                    fishWorth.text = $"It's worth {fishItem.silverValue}";
                    frameImage.sprite = fishItem.Rarity.frame;
                    silverImage.gameObject.SetActive(true);
                }
                else if (catchable is LootBox lootBox)
                {
                    fishWorth.text = "Open it in town!";
                    frameImage.sprite = null;
                    silverImage.gameObject.SetActive(false);
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
                silverImage.gameObject.SetActive(false);
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