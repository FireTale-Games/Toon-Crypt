using System;
using System.Collections.Generic;
using System.Linq;
using FT.Data;
using FT.TD;
using FT.Tools.Observers;
using FT.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FT.Inventory
{
    public class Inventory : MonoBehaviour, IInventory
    {
        [SerializeField] private List<InventoryItem> _items = new();
        [SerializeField] private Item[] _debugItems;
        
        public IObservableAction<Action<InventoryItem>> OnItemAdded => _onItemAdded;
        private readonly ObservableAction<Action<InventoryItem>> _onItemAdded = new();
        
        public IObservableAction<Action<InventoryItem>> OnItemRemoved => _onItemRemoved;
        private readonly ObservableAction<Action<InventoryItem>> _onItemRemoved = new();

        public IObservableAction<Action<InventoryItem>> OnWeaponEquipped => _onWeaponEquipped;
        private readonly ObservableAction<Action<InventoryItem>> _onWeaponEquipped = new();
        
        public IObservableAction<Action<InventoryItem>> OnWeaponUnequipped => _onWeaponUnequipped;
        private readonly ObservableAction<Action<InventoryItem>> _onWeaponUnequipped = new();
        
        private void Awake()
        {
            if (_debugItems != null) 
                InitializeDebugItems();
        }

        public InventoryItem[] SetupSlots() => _items.ToArray();
        
        private void InitializeDebugItems()
        {
            foreach (Item debugItem in _debugItems)
            {
                InventoryItem inventoryItem = new(debugItem.Id, Random.Range(0, 27));
                _items.Add(inventoryItem);
            }

            Item item = ItemDatabase.GetRandomItem();
            for (int i = 0; i < 27; i++)
            {
                if (_items.Any(itemData => itemData.Index == i)) 
                    continue;
                
                InventoryItem inventoryItem = new(item.Id, i);
                _items.Add(inventoryItem);
                break;
            }
        }
    }
}