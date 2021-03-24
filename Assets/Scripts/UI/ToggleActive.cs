using UnityEngine;

namespace UI
{
    public class ToggleActive : MonoBehaviour
    {
        public GameObject target;
    
        public void Toggle()
        {
            if(target == null)
                gameObject.SetActive(!gameObject.activeSelf);
            else
                target.SetActive(!target.activeSelf);
        }
    }
}
