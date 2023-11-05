using System;
using System.Collections.Generic;
using FT.Data;
using FT.Tools.Observers;
using FT.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FT.Inventory
{
    public class Inventory : MonoBehaviour, IInventory
    {
        private readonly List<InventoryItem> _items = new();
        private InventoryItem _weapon;
        [SerializeField] private Item[] _debugItems;
        
        public IObservableAction<Action<InventoryItem>> OnInventoryUpdate => _onInventoryUpdate;
        private readonly ObservableAction<Action<InventoryItem>> _onInventoryUpdate = new();
        
        public IObservableAction<Action<InventoryItem>> OnWeaponUpdate => _onWeaponUpdate;
        private readonly ObservableAction<Action<InventoryItem>> _onWeaponUpdate = new();
        
        public IObservableAction<Action<int, int>> OnAbilityUpdate => _onAbilityUpdate;
        private readonly ObservableAction<Action<int, int>> _onAbilityUpdate = new();
        
        private void Awake()
        {
            if (_debugItems != null) 
                InitializeDebugItems();
        }

        public void InitializeInventory()
        {
            foreach (InventoryItem inventoryItem in _items)
                _onInventoryUpdate.Action?.Invoke(inventoryItem);
        }

        public void UpdateInventory(InventoryItem inventoryItem, int slotIndex)
        {
            int itemIndex = _items.FindIndex(item => item.Index == slotIndex);
            if (!inventoryItem.IsValid)
            {
                if (itemIndex != -1)
                    _items.Remove(_items[itemIndex]);
                
                _onInventoryUpdate.Action?.Invoke(new InventoryItem(0, slotIndex));
                return;
            }
                
            inventoryItem.SetIndex(slotIndex);
            if (itemIndex != -1)
                _items[itemIndex] = inventoryItem;
            else
                _items.Add(inventoryItem);

            _onInventoryUpdate.Action?.Invoke(inventoryItem);
        }

        public void UpdateWeapon(InventoryItem inventoryItem, int slotIndex)
        {
            inventoryItem.SetIndex(slotIndex);
            _weapon = inventoryItem;
            _onWeaponUpdate.Action?.Invoke(inventoryItem);
        }

        public void UpdateAbility(InventoryItem inventoryItem, int slotIndex)
        {
            _weapon._abilities[slotIndex] = inventoryItem.Id;
            _onAbilityUpdate.Action?.Invoke(inventoryItem.Id, slotIndex);
        }

        private void InitializeDebugItems()
        {
            foreach (Item debugItem in _debugItems)
            {
                InventoryItem inventoryItem = new(debugItem.Id, Random.Range(0, 27));
                _items.Add(inventoryItem);
                _onInventoryUpdate.Action?.Invoke(inventoryItem);
            }
        }
    }
}