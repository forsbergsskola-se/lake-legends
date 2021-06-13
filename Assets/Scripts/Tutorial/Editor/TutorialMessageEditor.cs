using System;
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
            EditorGUILayout.LabelField("Content");
            messageContent = EditorGUILayout.TextArea(messageContent);
            EditorGUILayout.LabelField("ButtonText");
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
            EditTutorialSystemFile(className);
        }

        private void EditTutorialSystemFile(string className)
        {
            var tutorialSystemPath = "Assets/Scripts/Tutorial/TutorialSystem.cs";
            var tutorialSystemContent = File.ReadAllText(tutorialSystemPath);
            var startMethodIndex = tutorialSystemContent.IndexOf("void Start");
            var variableName = className.Remove(0, 1);
            variableName = variableName.Insert(0, Char.ToLower(className[0]).ToString());
            var addVariable = $"\n        [SerializeField] private Message {variableName};";
            if (!tutorialSystemContent.Contains(addVariable))
                tutorialSystemContent = tutorialSystemContent.Insert(startMethodIndex - 18, addVariable);
            var endIndex = tutorialSystemContent.IndexOf("}", startMethodIndex);
            var indentation = "      ";
            var subscribe = $"messageHandler.SubscribeTo<{className}>(On{className});";
            if (!tutorialSystemContent.Contains(subscribe))
                tutorialSystemContent = tutorialSystemContent.Insert(endIndex - (indentation+indentation).Length + 3, "\n"+ indentation+indentation + subscribe);
            var method = $"{indentation}  private void On{className}({className} eventRef)\n" +
                         indentation+"  {\n" +
                         indentation+$"     TryCall({variableName});\n" +
                         indentation+"  }\n";
            var classEnd = tutorialSystemContent.LastIndexOf("}", tutorialSystemContent.Length - 2);
            if (!tutorialSystemContent.Contains($"private void On{className}({className}"))
                tutorialSystemContent = tutorialSystemContent.Insert(classEnd-4, method);
            File.WriteAllText(tutorialSystemPath, tutorialSystemContent);
        }
    }
}