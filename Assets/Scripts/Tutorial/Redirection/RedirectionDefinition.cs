using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tutorial.Redirection
{
    [CreateAssetMenu(menuName = "ScriptableObjects/RedirectionDefinition")]
    public class RedirectionDefinition : ScriptableObject
    {
#if UNITY_EDITOR
        public UnityEditor.SceneAsset sceneToLoad;
#endif
        public string sceneName;
        public string panelName;

        public virtual void Redirect(TutorialPopup tutorialPopup)
        {
            tutorialPopup.UnPause();
            SceneManager.LoadScene(sceneName);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        protected virtual void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            var menuPanel = FindObjectsOfType<MenuPanel>(true).First(panel => panel.name == panelName);
            menuPanel.gameObject.SetActive(true);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (sceneToLoad != null)
                sceneName = sceneToLoad.name;
        }
#endif
    }
}