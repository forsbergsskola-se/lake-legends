using System.IO;
using UnityEditor;
using UnityEngine;

namespace Tutorial
{
    public class TutorialMessageEditor : EditorWindow
    {
        private string messageName;
        private string messageContent;
        private const string SoPath = "Assets/ScriptableObjects/TutorialMessages";
        private const string EventFolder = "Assets/Scripts/Tutorial/Events";
        [MenuItem("Window/Custom Editors/Tutorial Message Editor")]
        private static void ShowWindow()
        {
            GetWindow(typeof(TutorialMessageEditor), false, "Tutorial Message Editor");
        }
        private void OnGUI()
        {
            EditorGUILayout.Space();
            messageName = EditorGUILayout.TextField("Name", messageName);
            messageContent = EditorGUILayout.TextArea(messageContent);
            if (GUILayout.Button("Create"))
            {
                CreateMessage();
                WriteCode();
            }
        }

        private void CreateMessage()
        {
            var message = CreateInstance<Message>();
            message.name = messageName;
            message.Init(messageContent);
            var fullPath = $"{SoPath}/{messageName}.asset";
            AssetDatabase.CreateAsset(message, fullPath);
        }

        private void WriteCode()
        {
            var className = $"{messageName}Event";
            var fullPath = $"{EventFolder}/{className}.cs";
            var content = "namespace Tutorial.Events {\n"+
                          $"public class {className}{{}}\n}}";
            if (!Directory.Exists(EventFolder))
                Directory.CreateDirectory(EventFolder);
            File.WriteAllText(fullPath, content);
        }
    }
}