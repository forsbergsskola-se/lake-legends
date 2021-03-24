using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugToggleMenu : MonoBehaviour
{
    public Transform content;
    public Button prefab;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Clear();
        Setup(arg0);
    }

    private void Setup(Scene scene)
    {
        var loadedScene = scene;
        var rootObjects = loadedScene.GetRootGameObjects().Where(go => go != gameObject && go.GetComponent<EventSystem>() == null);
        foreach (var rootObject in rootObjects)
        {
            var instance = Instantiate(prefab, content);
            instance.onClick.AddListener(() =>
            {
                rootObject.SetActive(!rootObject.activeSelf);
                var startOffString = rootObject.activeSelf ? "Turn Off" : "Turn On";
                instance.GetComponentInChildren<Text>().text = $"{startOffString} {rootObject.name}";
            });
            var startOffString = rootObject.activeSelf ? "Turn Off" : "Turn On";
            instance.GetComponentInChildren<Text>().text = $"{startOffString} {rootObject.name}";
        }
    }

    private void Clear()
    {
        foreach (var child in content.GetComponentsInChildren<Transform>())
        {
            if (child != content)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
