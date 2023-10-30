using System;
using FT.Tools.Observers;
using FT.UI;

namespace FT.Inventory
{
    public interface IInventory
    {
        public IObservableAction<Action<InventoryItem>> OnItemAdded { get; }
        public IObservableAction<Action<InventoryItem>> OnItemRemoved { get; }
        public IObservableAction<Action<InventoryItem>> OnWeaponEquipped { get; }
        public IObservableAction<Action<InventoryItem>> OnWeaponUnequipped { get; }
        public InventoryItem[] SetupSlots();
    }
}