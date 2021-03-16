using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class WorldSpaceButton : MonoBehaviour
    {
        public UnityEvent onRelease;

    
        private void OnMouseUp()
        {
            onRelease.Invoke();
        }
    }
}
