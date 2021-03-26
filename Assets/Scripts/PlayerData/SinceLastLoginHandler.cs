using System.Threading.Tasks;
using EventManagement;
using Events;
using Saving;
using UnityEngine;

namespace PlayerData
{
    public class SinceLastLoginHandler : MonoBehaviour
    {
        private IMessageHandler eventBroker;
        private SinceLastLoginSaver sinceLastLoginSaver;

        private void Start()
        {
            eventBroker = FindObjectOfType<EventsBroker>();
            eventBroker?.SubscribeTo<LoadedInventoryEvent>(LoginListener);
        }

        private async void LoginListener(LoadedInventoryEvent obj)
        {
            sinceLastLoginSaver = new SinceLastLoginSaver(new JsonSerializer(), obj.Saver);
            await SendLoginEvent();
            sinceLastLoginSaver.Save();
        }

        private async void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                sinceLastLoginSaver.Save();
                eventBroker.Publish(new UnFocusEvent());
            }
            else
            {
                if (sinceLastLoginSaver != null)
                    await SendLoginEvent();
            }
        }

        private void OnApplicationQuit()
        {
            sinceLastLoginSaver.Save();
        }

        private async Task SendLoginEvent()
        {
            var time = await sinceLastLoginSaver.GetTimeSinceLastLogin();
            eventBroker.Publish(new TimeSinceLastLoginEvent(time));
        }
    }
}