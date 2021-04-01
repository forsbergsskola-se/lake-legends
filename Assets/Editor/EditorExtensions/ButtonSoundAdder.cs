using System.Collections.Generic;
using System.Linq;
using Audio;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace EditorExtensions
{
    public class ButtonSoundAdder : EditorWindow
    {
        private Transform transform;
        private string eventName = "ButtonClickSound";
        [MenuItem("Window/Custom Editors/Button Sound Adder")]
        private static void ShowWindow()
        {
            GetWindow(typeof(ButtonSoundAdder), false, "Button Sound Adder");
        }

        private void OnGUI()
        {
            transform = (Transform) EditorGUILayout.ObjectField(transform, typeof(Transform), true);
            eventName = EditorGUILayout.TextField("EventName", eventName);
            if (GUILayout.Button("Add Button Sounds"))
            {
                AddButtonSounds();
            }
        }

        private void AddButtonSounds()
        {
            IEnumerable<GameObject> gameObjects;
            gameObjects = transform != null ? transform.GetComponentsInChildren<Button>(true).Select(button => button.gameObject) : FindObjectsOfType<Button>(true).Select(button => button.gameObject);
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.TryGetComponent(out AudioPlayer audio))
                    continue;
                var component = gameObject.AddComponent<AudioPlayer>();
                component.eventName = eventName;
                EditorUtility.SetDirty(gameObject);
            }

            gameObjects = FindObjectsOfType<WorldSpaceButton>(true).Select(button => button.gameObject);
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.TryGetComponent(out AudioPlayer audio))
                    continue;
                var component = gameObject.AddComponent<AudioPlayer>();
                component.eventName = eventName;
                EditorUtility.SetDirty(gameObject);
            }
        }
    }
}