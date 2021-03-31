using EventManagement;
using Events;
using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private string eventName = "ButtonClickSound";
        private IMessageHandler eventBroker;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(PlayAudio);
            eventBroker = FindObjectOfType<EventsBroker>();
        }

        public void PlayAudio()
        {
            eventBroker ??= FindObjectOfType<EventsBroker>();
            eventBroker.Publish(new PlaySoundEvent(SoundType.Sfx, eventName));
        }
    }
}