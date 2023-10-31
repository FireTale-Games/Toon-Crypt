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

        [SerializeField] private InventoryItem _weaponItem;
        
        public IObservableAction<Action<InventoryItem>> OnInventoryUpdate => _onInventoryUpdate;
        private readonly ObservableAction<Action<InventoryItem>> _onInventoryUpdate = new();

        public IObservableAction<Action<InventoryItem>> OnWeaponUpdate => _onWeaponUpdate;
        private readonly ObservableAction<Action<InventoryItem>> _onWeaponUpdate = new();
        
        private void Awake()
        {
            if (_debugItems != null) 
                InitializeDebugItems();
        }
        
        public void SetupSlots()
        {
            foreach (InventoryItem inventoryItem in _items)
                _onInventoryUpdate.Action?.Invoke(inventoryItem);
        }

        public void UpdateInventory(InventoryItem draggedIcon, InventoryItem hitIcon)
        {
            InventoryItem draggedItem = new(hitIcon._guid, hitIcon.Id, draggedIcon.Index);
            InventoryItem hitItem = new(draggedIcon._guid, draggedIcon.Id, hitIcon.Index);
            
            if (_items.Contains(hitIcon))
            {
                int index = _items.FindIndex(item => item.Equals(hitIcon));
                _items[index] = draggedItem;
            }

            int index1 = _items.FindIndex(item => item.Equals(draggedIcon));
            _items[index1] = hitItem;
            
            _onInventoryUpdate.Action?.Invoke(draggedItem);
            _onInventoryUpdate.Action?.Invoke(hitItem);
        }

        public void UpdateWeapon(InventoryItem draggedIcon, InventoryItem hitIcon)
        {
            InventoryItem draggedItem = new(hitIcon._guid, hitIcon.Id, draggedIcon.Index);
            InventoryItem hitItem = new(draggedIcon._guid, draggedIcon.Id, hitIcon.Index);
    
            bool isWeaponHit = _weaponItem.IsValid && hitIcon.Equals(_weaponItem);
            bool isWeaponDrag = _weaponItem.IsValid && draggedIcon.Equals(_weaponItem);

            InventoryItem inventoryItemToFind = isWeaponHit ? draggedIcon : hitIcon;
            InventoryItem newWeaponItem = isWeaponHit ? hitItem : draggedItem;
            InventoryItem newInventoryItem = isWeaponHit ? draggedItem : hitItem;

            if (isWeaponHit || isWeaponDrag)
            {
                int index = _items.FindIndex(item => item.Equals(inventoryItemToFind));
        
                if (index == -1)
                    _items.Add(newInventoryItem);
                else
                    _items[index] = newInventoryItem;
        
                _weaponItem = new InventoryItem(newWeaponItem._guid, newWeaponItem.Id, 255);
        
                _onWeaponUpdate.Action?.Invoke(_weaponItem);
                _onInventoryUpdate.Action?.Invoke(newInventoryItem);
                return;
            }
            _weaponItem = hitItem;
            _items.Remove(draggedIcon);

            _onWeaponUpdate.Action?.Invoke(_weaponItem);
            _onInventoryUpdate.Action?.Invoke(draggedItem);
        }

        private void InitializeDebugItems()
        {
            foreach (Item debugItem in _debugItems)
            {
                InventoryItem inventoryItem = new(Guid.NewGuid(), debugItem.Id, Random.Range(0, 27));
                _items.Add(inventoryItem);
            }

            Item item = ItemDatabase.GetRandomItem();
            for (int i = 0; i < 27; i++)
            {
                if (_items.Any(itemData => itemData.Index == i)) 
                    continue;
                
                InventoryItem inventoryItem = new(Guid.NewGuid(), item.Id, i);
                _items.Add(inventoryItem);
                break;
            }
        }
    }
}