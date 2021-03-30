using System.Collections;
using Items;
using UnityEngine;

namespace UI
{
    public class LootBoxItemSlot : Slot
    {
        public float secondsToLerp;
        public override void Setup(IItem item, bool hasCaught = true)
        {
            Item = item;
            ApplyImages();
        }

        public void FadeIn()
        {
            StartCoroutine(DoFadeIn());
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
                currentLerpValue += Time.deltaTime / secondsToLerp;
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
                currentLerpValue += Time.deltaTime / secondsToLerp;
                background.color = currentColor;
                frame.color = currentColor;
                middleGround.color = currentColor;
                yield return null;
            } while (currentLerpValue < 1);
        }
    }
}