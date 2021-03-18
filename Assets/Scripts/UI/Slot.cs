using Items;
using UnityEngine;

namespace UI
{
    public abstract class Slot : MonoBehaviour
    {
        public abstract void Setup(IItem item, bool hasCaught = true);
        public IItem Item { get; protected set; }
    }
}