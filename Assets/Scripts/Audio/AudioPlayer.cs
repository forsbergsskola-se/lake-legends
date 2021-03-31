using EventManagement;
using Events;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        public string eventName = "ButtonClickSound";
        private IMessageHandler eventBroker;

        private void Awake()
        {
            GetComponent<Button>()?.onClick.AddListener(PlayAudio);
            GetComponent<WorldSpaceButton>()?.onRelease.AddListener(PlayAudio);
            eventBroker = FindObjectOfType<EventsBroker>();
        }

        public void PlayAudio()
        {
            eventBroker ??= FindObjectOfType<EventsBroker>();
            eventBroker.Publish(new PlaySoundEvent(SoundType.Sfx, eventName));
        }
    }
}