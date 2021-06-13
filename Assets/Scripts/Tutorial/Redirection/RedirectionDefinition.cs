using System.Linq;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tutorial.Redirection
{
    [CreateAssetMenu(menuName = "ScriptableObjects/RedirectionDefinition")]
    public class RedirectionDefinition : ScriptableObject
    {
        public SceneAsset sceneToLoad;
        public string panelName;

        public virtual void Redirect()
        {
            SceneManager.LoadScene(sceneToLoad.name);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        protected virtual void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            var menuPanel = FindObjectsOfType<MenuPanel>(true).First(panel => panel.name == panelName);
            menuPanel.gameObject.SetActive(true);
        }
    }
}