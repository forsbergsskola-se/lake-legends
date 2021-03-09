using UnityEngine;

namespace Items
{
    public class AllItems
    {
        private static ItemIndexer itemIndexer;
        public static ItemIndexer ItemIndexer
        {
            get
            {
                if (itemIndexer == null)
                    itemIndexer = Resources.Load<ItemIndexer>("Global Item Index");
                return itemIndexer;
            }
        }
    }
}