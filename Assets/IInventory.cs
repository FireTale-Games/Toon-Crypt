using System;
using FT.Tools.Observers;
using FT.UI;

namespace FT.Inventory
{
    public interface IInventory
    {
        public IObservableAction<Action<InventoryItem>> OnInventoryUpdate { get; }
        public IObservableAction<Action<InventoryItem>> OnWeaponUpdate { get; }
        public IObservableAction<Action<InventoryItem>> OnAbilityUpdate { get; }
        public void SetupSlots();
        public void UpdateInventory(InventoryItem draggedIcon, InventoryItem hitIcon);
        public void UpdateWeapon(InventoryItem draggedIcon, InventoryItem hitIcon);
        public void UpdateAbility(InventoryItem draggedIcon, InventoryItem hitIcon);
    }
}