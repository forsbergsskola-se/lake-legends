using System.Collections;
using Items;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LootBoxItemSlot : Slot
    {
        public float secondsToFadeIn;
        public float secondsToFadeOut;
        public Text text;
        public override void Setup(IItem item, bool hasCaught = true)
        {
            Item = item;
            text.text = "";
            ApplyImages();
        }

        public void FadeIn()
        {
            StartCoroutine(DoFadeIn());
            StartCoroutine(WriteOutText(Item.Name, secondsToFadeIn));
        }

        private IEnumerator WriteOutText(string message, float totalTime)
        {
            var allChars = message.ToCharArray();
            var currentMessage = "";
            var time = 0.0f;
            var currentIndex = 0;
            var timeBetweenIncrement = totalTime / allChars.Length;
            while (currentMessage.Length < message.Length)
            {
                time += Time.deltaTime;
                if (time > timeBetweenIncrement)
                {
                    time -= timeBetweenIncrement;
                    currentMessage += allChars[currentIndex];
                    currentIndex++;
                    text.text = currentMessage;
                }

                yield return null;
            }
        }

        public void FadeOut()
        {
            StartCoroutine(DoFadeOut());
        }

        IEnumerator DoFadeIn()
        {
            var currentLerpValue = 0f;
            do
            {
                var currentColor = Color.Lerp(new Color(1, 1, 1, 0), Color.white, currentLerpValue);
                currentLerpValue += Time.deltaTime / secondsToFadeIn;
                background.color = currentColor;
                frame.color = currentColor;
                middleGround.color = currentColor;
                yield return null;
            } while (currentLerpValue < 1);
        }
        
        IEnumerator DoFadeOut()
        {
            var currentLerpValue = 0f;
            do
            {
                var currentColor = Color.Lerp(Color.white, new Color(1, 1, 1, 0), currentLerpValue);
                currentLerpValue += Time.deltaTime / secondsToFadeOut;
                background.color = currentColor;
                frame.color = currentColor;
                middleGround.color = currentColor;
                yield return null;
            } while (currentLerpValue < 1);
        }
    }
}