using System;
using System.Collections.Generic;
using FT.Data;
using FT.Tools.Observers;
using FT.UI;
using UnityEngine;

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

        private void Awake()
        {
            OnItemAdded.AddObserver(ItemAdded);
            
            if (_debugItems == null) 
                return;
            
            foreach (Item debugItem in _debugItems)
            {
                InventoryItem inventoryItem = new(debugItem.Id);
                _items.Add(inventoryItem);
                _onItemAdded.Action?.Invoke(inventoryItem);
            }
        }

        private void ItemAdded(InventoryItem inventoryItem)
        {
            Debug.Log($"Item guid: {inventoryItem._guid}, Item Id: {inventoryItem.Id}");
        }
    }
}