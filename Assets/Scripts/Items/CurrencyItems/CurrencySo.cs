using System;
using EventManagement;
using Events;
using UnityEngine;

namespace Items.CurrencyItems
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Currency")]
    public class CurrencySo : ScriptableObject, IItem
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private int amount;
        [SerializeField] private string ItemID;
        [SerializeField] private CurrencyType currencyType;
        [SerializeField] private Rarity rarity;
        public string ID {
            get
            {
                if (!string.IsNullOrEmpty(ItemID)) return ItemID;
                Debug.LogError("Item IDs Aren't Set Up Correctly!");
                throw new Exception("Item IDs Aren't Set Up Correctly!");
            }
        }
        
        void Awake()
        {
            ItemID = Guid.NewGuid().ToString();
        }

        public void GenerateNewGuid()
        {
            ItemID = Guid.NewGuid().ToString();
        }

        public string Name => name;
        public int RarityValue => 0;
        public Sprite Sprite => sprite;
        public Rarity Rarity => rarity;
        public int Amount => amount;
        public CurrencyType CurrencyType => currencyType;
        public void Use()
        {
            switch (CurrencyType)
            {
                case CurrencyType.Silver:
                    FindObjectOfType<EventsBroker>().Publish(new IncreaseSilverEvent(Amount));
                    break;
                case CurrencyType.Gold:
                    FindObjectOfType<EventsBroker>().Publish(new IncreaseGoldEvent(Amount));
                    break;
                case CurrencyType.Bait:
                    FindObjectOfType<EventsBroker>().Publish(new IncreaseBaitEvent(Amount));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}