using Items;
using UnityEngine;

namespace UI
{
    public abstract class Slot : MonoBehaviour
    {
        public abstract void Setup(IItem item);
        public IItem Item { get; protected set; }
    }
}