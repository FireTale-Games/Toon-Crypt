using System;
using System.Collections.Generic;
using System.Linq;
using FT.Data;
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
        
        public IObservableAction<Action<InventoryItem>> OnInventoryUpdate => _onInventoryUpdate;

        private readonly ObservableAction<Action<InventoryItem>> _onInventoryUpdate = new();
        
        private void Awake()
        {
            if (_debugItems != null) 
                InitializeDebugItems();
        }

        public void UpdateInventory(InventoryItem draggedIcon, InventoryItem hitIcon)
        {
            _onInventoryUpdate.Action?.Invoke(draggedIcon);
            _onInventoryUpdate.Action?.Invoke(hitIcon);
        }
        
        public void InitializeInventory()
        {
            foreach (InventoryItem inventoryItem in _items)
                _onInventoryUpdate.Action?.Invoke(inventoryItem);
        }

        private void InitializeDebugItems()
        {
            foreach (Item debugItem in _debugItems)
            {
                InventoryItem inventoryItem = new(debugItem.Id, Random.Range(0, 27));
                _items.Add(inventoryItem);
                _onInventoryUpdate.Action?.Invoke(inventoryItem);
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