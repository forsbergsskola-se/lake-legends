using EventManagement;
using Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Auth
{
    public class InitializeHandler : MonoBehaviour
    {
        public string sceneToLoad;
        private IMessageHandler messageHandler;
        private void Start()
        {
            messageHandler = FindObjectOfType<EventsBroker>();
            messageHandler.SubscribeTo<LoadedInventoryEvent>(OnLoadedInventory);
        }

        private void OnLoadedInventory(LoadedInventoryEvent obj)
        {
            SceneManager.LoadScene(sceneToLoad);
        }

        private void OnDestroy()
        {
            messageHandler?.UnsubscribeFrom<LoadedInventoryEvent>(OnLoadedInventory);
        }
    }
}