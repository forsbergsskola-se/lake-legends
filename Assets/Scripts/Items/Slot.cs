using UnityEngine;

namespace Items
{
    public abstract class Slot : MonoBehaviour
    {
        public abstract void Setup(IItem item);
        public IItem Item { get; protected set; }
    }
}