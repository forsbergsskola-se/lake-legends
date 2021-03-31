using System.Collections;
using EventManagement;
using Events;
using UnityEngine;

namespace PlayerData
{
    public class BaitRegen : MonoBehaviour
    {
        [SerializeField] private int secondsToRegenOneBait = 5;
        [SerializeField] private float currentSecondsPassed;
        private IMessageHandler eventBroker;

        private void Start()
        {
            eventBroker = FindObjectOfType<EventsBroker>();
            eventBroker.SubscribeTo<TimeSinceLastLoginEvent>(OnLoginEvent);
            eventBroker.SubscribeTo<UnFocusEvent>(OnUnfocusEvent);
        }

        private void OnUnfocusEvent(UnFocusEvent eventRef)
        {
            StopAllCoroutines();
        }

        private void OnLoginEvent(TimeSinceLastLoginEvent eventRef)
        {
            CheckBaitAmountEarnedSinceLastLogin(eventRef.Seconds);
            StartCoroutine(Timer(1f));
        }

        private void CheckBaitAmountEarnedSinceLastLogin(float totalSecondsPassed)
        {
            var amount = totalSecondsPassed / secondsToRegenOneBait;
            var remainder = amount % 1;
            var secondsPassed = remainder * secondsToRegenOneBait;
            var baitAmount = (int)amount;
            currentSecondsPassed += secondsPassed;
            eventBroker.Publish(new IncreaseBaitEvent(baitAmount, false));
        }

        private IEnumerator Timer(float interval)
        {
            while (true)
            {
                yield return new WaitForSeconds(interval);
                currentSecondsPassed += interval;
                if (currentSecondsPassed >= secondsToRegenOneBait)
                {
                    eventBroker.Publish(new IncreaseBaitEvent(1, false));
                    currentSecondsPassed -= secondsToRegenOneBait;
                }
            }
        }
    }
}