using System.Linq;
using UnityEngine;
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
        Setup();
    }

    private void Setup()
    {
        var rootObjects = FindObjectsOfType<Transform>().Where(transform1 => transform1.parent == null && transform1 != transform);
        foreach (var rootObject in rootObjects)
        {
            var instance = Instantiate(prefab, content);
            instance.onClick.AddListener(() =>
            {
                rootObject.gameObject.SetActive(!rootObject.gameObject.activeSelf);
                var startOffString = rootObject.gameObject.activeSelf ? "Turn Off" : "Turn On";
                instance.GetComponentInChildren<Text>().text = $"{startOffString} {rootObject.name}";
            });
            var startOffString = rootObject.gameObject.activeSelf ? "Turn Off" : "Turn On";
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
