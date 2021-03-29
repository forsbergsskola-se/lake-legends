using EventManagement;
using Events;
using UnityEngine;

namespace Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private string eventName;
        private IMessageHandler eventBroker;

        private void Awake()
        {
            eventBroker = FindObjectOfType<EventsBroker>();
        }

        public void PlayAudio()
        {
            eventBroker ??= FindObjectOfType<EventsBroker>();
            eventBroker.Publish(new PlaySoundEvent(SoundType.Sfx, eventName));
        }
    }
}