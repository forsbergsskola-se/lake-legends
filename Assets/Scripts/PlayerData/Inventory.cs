using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventManagement;
using Events;
using Items;
using Saving;

namespace PlayerData
{
    public class Inventory : IInventory
    {
        public int MaxSize => 50;
        public int TotalSizeOfInventory => items.Sum(item => item.Value);
        protected Dictionary<string, int> items = new Dictionary<string, int>();
        protected readonly IInventorySaver saver;
        private readonly GearInventory gearInventory;
        private IMessageHandler messageHandler;
        protected virtual string InventoryKey => "Inventory";

        protected Inventory(IInventorySaver saver, IMessageHandler messageHandler)
        {
            this.saver = saver;
        }
        
        
        public Inventory(IInventorySaver saver, IMessageHandler messageHandler, GearInventory gearInventory)
        {
            this.saver = saver;
            this.gearInventory = gearInventory;
            this.messageHandler = messageHandler;
            this.messageHandler.SubscribeTo<AddItemToInventoryEvent>(OnAddItem);
            this.messageHandler.SubscribeTo<RemoveItemFromInventoryEvent>(OnRemoveItem);
            this.messageHandler.SubscribeTo<EndFishOMeterEvent>(eve =>
            {
                if (eve.catchItem != null && eve.catchItem is IItem item && !(eve.catchItem is FishItem))
                    AddItem(item);
            });
        }

        private void OnAddItem(AddItemToInventoryEvent obj)
        {
            if (!(obj.Item is FishItem))
                AddItem(obj.Item);
            messageHandler.Publish(new UpdateInventoryEvent(true, obj.Item, gearInventory));
        }
        
        private void OnRemoveItem(RemoveItemFromInventoryEvent obj)
        {
            if (!(obj.Item is FishItem))
                RemoveItem(obj.Item);
            messageHandler.Publish(new UpdateInventoryEvent(false, obj.Item, gearInventory));
        }
        
        public virtual bool AddItem(IItem iItem)
        {
            if (iItem is GearInstance item)
                gearInventory.AddItem(item);
            if (TotalSizeOfInventory >= MaxSize)
                return false;
            if (items.ContainsKey(iItem.ID))
                items[iItem.ID]++;
            else
                items.Add(iItem.ID, 1);
            Serialize();
            return true;
        }

        public virtual bool RemoveItem(IItem iItem)
        {
            if (iItem is GearInstance item)
                gearInventory.RemoveItem(item);
            if (!items.ContainsKey(iItem.ID))
                return false;
            if (items.ContainsKey(iItem.ID))
                items[iItem.ID]--;
            if (items[iItem.ID] <= 0)
                items.Remove(iItem.ID);
            Serialize();
            return true;
        }

        public virtual Dictionary<string, int> GetAllItems()
        {
            return items;
        }
        
        public Dictionary<string, GearInstance> GetGear()
        {
            return gearInventory.GeneratedGear;
        }

        public virtual async Task Deserialize()
        {
            await gearInventory.Deserialize();
            var savedInventory = await saver.LoadInventory(InventoryKey);
            if (savedInventory == null)
                return;
            items = savedInventory;
        }

        public virtual void Serialize()
        {
            gearInventory.Serialize();
            saver.SaveInventory(InventoryKey, this);
        }
    }
}