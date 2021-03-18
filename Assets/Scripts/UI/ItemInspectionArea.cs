using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemInspectionArea : MonoBehaviour
    {
        public GameObject button;
        public Text description;
        public Text title;
        public void CreateButtons(Dictionary<string, Action> actions, string title, string description)
        {
            Clear();
            this.title.text = title;
            this.description.text = description;
            foreach (var action in actions)
            {
                var instance = Instantiate(button, transform);
                instance.GetComponentInChildren<Text>().text = action.Key;
                instance.GetComponent<Button>().onClick.AddListener(() => action.Value?.Invoke());
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

        private void Clear()
        {
            var children = transform.GetComponentsInChildren<Transform>()
                .Select(transform1 => transform1.gameObject).Where(o => o != gameObject);
            foreach (var child in children)
            { 
                Destroy(child);
            }
        }
    }
}