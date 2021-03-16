using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
    public class WorldSpaceButton : MonoBehaviour
    {
        public UnityEvent onRelease;

    
        private void OnMouseUp()
        {
            if(!EventSystem.current.IsPointerOverGameObject())
                onRelease.Invoke();
        }
    }
}
