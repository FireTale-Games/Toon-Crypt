using System;
using FT.Tools.Observers;
using FT.UI;

namespace FT.Inventory
{
    public interface IInventory
    {
        public IObservableAction<Action<InventoryItem>> OnInventoryUpdate { get; }
        public IObservableAction<Action<InventoryItem>> OnWeaponUpdate { get; }
        public IObservableAction<Action<int, int>> OnAbilityUpdate { get; }
        public void InitializeInventory();
        public void UpdateInventory(InventoryItem inventoryItem, int slotIndex);
        public void UpdateWeapon(InventoryItem inventoryItem, int slotIndex);
        public void UpdateAbility(InventoryItem inventoryItem, int slotIndex);
    }
}