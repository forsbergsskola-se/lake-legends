using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fish
{
    [CreateAssetMenu(menuName = "ScriptableObjects/FishType")]
    public class FishType : ScriptableObject
    {
        public Sprite sprite;
        private string TypeID;

        [ContextMenu("GenerateFishItems")]
        public void GenerateFishItems()
        {
            
        }
    }
}
