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
        [SerializeField] private Text resultText, fishNameText, fishWorth, treasureBoxText;
        [SerializeField] private Image image;
        [SerializeField] private Image frameImage;
        [SerializeField] private Image silverImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private GameObject successUI;
        [SerializeField] private GameObject failUI;

        [SerializeField] private string IdleAnimation = "Idle";
        
        public ICatchable catchable;
        public IMessageHandler eventsBroker;
        
        void OnEnable()
        {
            failUI.gameObject.SetActive(false);
            successUI.gameObject.SetActive(false);
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
                successUI.gameObject.SetActive(true);
                failUI.gameObject.SetActive(false);
                image.sprite = catchable.Sprite;
                fishNameText.text = catchable.Name;
                frameImage.sprite = catchable.Rarity.frame;
                backgroundImage.sprite = catchable.Rarity.background;
                
                if (catchable is FishItem fishItem)
                {
                    treasureBoxText.text = "";
                    fishWorth.text = $"It's worth {fishItem.silverValue}";
                    silverImage.gameObject.SetActive(true);
                }
                else if (catchable is LootBox lootBox)
                {
                    fishWorth.text = "";
                    treasureBoxText.text = "Open it in town!";
                    silverImage.gameObject.SetActive(false);
                }
                else
                {
                    fishNameText.text = catchable.Name;
                }
            }
            else
            {
                failUI.gameObject.SetActive(true);
            }
        }

        public void FishAgain()
        {
            eventsBroker.Publish(new AnimationTriggerEvent(IdleAnimation));
            eventsBroker.Publish(new FishAgainEvent());
            this.gameObject.SetActive(false);
            failUI.gameObject.SetActive(false);
            successUI.gameObject.SetActive(false);
        }

        public void BackToHub()
        {
            SceneManager.LoadScene("MainUI");
            this.gameObject.SetActive(false);
            failUI.gameObject.SetActive(false);
            successUI.gameObject.SetActive(false);
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