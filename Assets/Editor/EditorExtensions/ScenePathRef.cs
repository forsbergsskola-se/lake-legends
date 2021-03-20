using System;
using System.Collections.Generic;
using System.IO;

namespace EditorExtensions
{
    [Serializable]
    public class ScenePathRef
    {
        public ScenePathRef(string path)
        {
            this.path = path;
        }
        public string path;

        public void Validate(IEnumerable<string> paths)
        {
            if (string.IsNullOrEmpty(path))
                return;
            foreach (var scenePath in paths)
            {
                var newPath = Path.GetFileNameWithoutExtension(scenePath);
                if (newPath == path)
                {
                    path = scenePath;
                }
            }
        }
    }
}