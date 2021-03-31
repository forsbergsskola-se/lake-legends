using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
    public class WorldSpaceButton : MonoBehaviour
    {
        public UnityEvent onRelease;

    
        private void OnMouseDown()
        {
            if (MenuPanel.AllAreClosed)
                onRelease.Invoke();
        }
    }
}
