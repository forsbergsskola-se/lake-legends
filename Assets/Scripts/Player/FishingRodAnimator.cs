using System;
using EventManagement;
using Events;
using UnityEngine;

namespace Player
{
    public class FishingRodAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private IMessageHandler eventsBroker;
        
        void Start()
        {
            eventsBroker = FindObjectOfType<EventsBroker>();
            animator = this.gameObject.GetComponent<Animator>();
        
            eventsBroker.SubscribeTo<AnimationTriggerEvent>(TriggerAnimation);
            eventsBroker.SubscribeTo<AnimationBlendEvent>(BlendAnimation);
        }

        private void TriggerAnimation(AnimationTriggerEvent eventRef)
        {
            animator.SetTrigger(eventRef.triggerName);
        }
        
        private void BlendAnimation(AnimationBlendEvent eventRef)
        {
            animator.SetFloat(eventRef.blendName, eventRef.blendValue);
        }

        private void OnDestroy()
        {
            eventsBroker.UnsubscribeFrom<AnimationTriggerEvent>(TriggerAnimation);
            eventsBroker.UnsubscribeFrom<AnimationBlendEvent>(BlendAnimation);
        }
    }
}