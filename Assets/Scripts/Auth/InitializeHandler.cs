using System.Collections;
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
            //StartCoroutine(LoadSceneWithDelay(5));
        }

        private IEnumerator LoadSceneWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(sceneToLoad);
        }

        private void OnDestroy()
        {
            messageHandler?.UnsubscribeFrom<LoadedInventoryEvent>(OnLoadedInventory);
        }
    }
}