using System;
using System.Collections.Generic;
using System.Linq;
using Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemInspectionArea : MonoBehaviour
    {
        public GameObject button;
        public Transform statsParent;
        public Text title;
        public GameObject textPrefab;
        public void CreateButtons(Dictionary<string, Action> actions, Dictionary<string, Callback> callbacks, string title, params string[] stats)
        {
            Clear();
            this.title.text = title;
            foreach (var stat in stats)
            {
                var instance = Instantiate(textPrefab, statsParent);
                instance.GetComponent<Text>().text = stat;
            }
            foreach (var action in actions)
            {
                var instance = Instantiate(button, transform);
                instance.GetComponentInChildren<Text>().text = action.Key;
                instance.GetComponent<Button>().onClick.AddListener(() => action.Value?.Invoke());
            }
            foreach (var action in callbacks)
            {
                var instance = Instantiate(button, transform);
                instance.GetComponentInChildren<Text>().text = action.Key;
                instance.GetComponent<Button>().onClick.AddListener(() => action.Value?.Method?.Invoke(action.Value.CallbackMethod));
            }
        }
        
        // Reflection
        /*public void CreateButtons(IItem item, List<MethodInfo> methodInfos)
        {
            Clear();
            for (var i = 0; i < methodInfos.Count; i++)
            {
                var instance = Instantiate(button, transform);
                instance.GetComponentInChildren<Text>().text = methodInfos[i].GetCustomAttribute<InteractAttribute>().Name;
                var j = i;
                instance.GetComponent<Button>().onClick.AddListener(() => methodInfos[j].Invoke(item, null));
            }
        }*/

        public void Clear()
        {
            var children = transform.GetComponentsInChildren<Transform>()
                .Select(transform1 => transform1.gameObject).Where(o => o != gameObject && o != statsParent.gameObject);
            this.title.text = "";
            foreach (var child in children)
            { 
                Destroy(child);
            }
        }
    }
}