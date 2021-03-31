using System.Collections;
using EventManagement;
using Events;
using UnityEngine;
using UnityEngine.VFX;

namespace Player
{
    public class FloaterLogic : MonoBehaviour
    {
        [SerializeField] private Vector3 hookedPos;
        [SerializeField] private VisualEffect splash;
        private Vector3 startPos;
        private EventsBroker eventsBroker;

        private void Start()
        {
            splash.Stop();
            eventsBroker = FindObjectOfType<EventsBroker>();
            startPos = transform.position;
            eventsBroker.SubscribeTo<EndFishOMeterEvent>(OnEndFishing);
            eventsBroker.SubscribeTo<StartFishOMeterEvent>(OnStartFishing);
        }

        private void OnEndFishing(EndFishOMeterEvent endFishOMeterEvent)
        {
            transform.position = startPos;
        }

        private void OnStartFishing(StartFishOMeterEvent startFishOMeterEvent)
        {
            StartCoroutine(MoveDown(1f));
            transform.position = hookedPos;
        }

        private IEnumerator MoveDown(float time)
        {
            splash.Play();
            var t = 0.0f;
            do
            {
                transform.position = Vector3.Lerp(startPos, hookedPos, t);
                t += Time.deltaTime / time;
                yield return null;
            } while (t < 1);
            splash.Stop();
        }

        private void OnDestroy()
        {
            eventsBroker?.UnsubscribeFrom<EndFishOMeterEvent>(OnEndFishing);
            eventsBroker?.UnsubscribeFrom<StartFishOMeterEvent>(OnStartFishing);
        }
    }
}
