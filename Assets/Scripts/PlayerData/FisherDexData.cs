﻿using EventManagement;
using Events;
using Items;
using Saving;

namespace PlayerData
{
    public class FisherDexData : Inventory
    {
        protected override string InventoryKey => "FisherDex";
        public FisherDexData(IInventorySaver saver, IMessageHandler messageHandler) : base(saver, messageHandler)
        {
            messageHandler?.SubscribeTo<EndFishOMeterEvent>(eve =>
            {
                if (eve.fishItem != null)
                    AddItem(eve.fishItem);
            });
        }

        public override bool AddItem(IItem iItem)
        {
            if (items.ContainsKey(iItem.ID))
                items[iItem.ID]++;
            else
                items.Add(iItem.ID, 1);
            return true;
        }

        public override bool RemoveItem(IItem iItem)
        {
            return false;
        }
    }
}