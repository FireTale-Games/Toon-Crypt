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

        //public void UpdateInventory(InventoryItem draggedIcon, InventoryItem hitIcon)
        //{
        //    int index = draggedIcon.Index;
        //    draggedIcon.SetIndex(hitIcon.Index);
        //    hitIcon.SetIndex(index);
        //    
        //    _onInventoryUpdate.Action?.Invoke(draggedIcon);
        //    _onInventoryUpdate.Action?.Invoke(hitIcon);
        //}
        
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