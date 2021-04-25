using System.Collections;
using EventManagement;
using Events;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Auth
{
    public class InitializeHandler : MonoBehaviour
    {
        public string sceneToLoad;
        public EULA eula;
        private IMessageHandler messageHandler;

        public bool HasAcceptedEula
        {
            get
            {
                var value = PlayerPrefs.GetInt("EULA_Accepted_0", 0);
                var accepted = value > 0;
                return accepted;
            }
            set
            {
                var accepted = value ? 1 : 0;
                PlayerPrefs.SetInt("EULA_Accepted_0", accepted);
            }
        }
        private void Start()
        {
            Application.targetFrameRate = 24;
            if (HasAcceptedEula)
            {
                messageHandler = FindObjectOfType<EventsBroker>();
                messageHandler.SubscribeTo<LoadedInventoryEvent>(OnLoadedInventory);
            }
            else
            {
                var instance = Instantiate(eula, FindObjectOfType<Canvas>().transform);
                instance.SetUp(this);
            }
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