using System;
using FT.Tools.Observers;
using FT.UI;

namespace FT.Inventory
{
    public interface IInventory
    {
        public IObservableAction<Action<InventoryItem>> OnItemAdded { get; }
        public IObservableAction<Action<InventoryItem>> OnItemRemoved { get; }
    }
}