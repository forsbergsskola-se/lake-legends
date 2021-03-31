using UnityEngine;
using UnityEngine.VFX;

namespace Player
{
    public class StopEffectOnStart : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<VisualEffect>().Stop();
        }
    }
}