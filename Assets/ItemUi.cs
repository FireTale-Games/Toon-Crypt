using System;
using UnityEngine;


namespace FT.UI
{
    public class ItemUi : MonoBehaviour, IItemUi
    {
        [SerializeField] private ItemType _itemType = ItemType.All;
        
        public InventoryItem InventoryItem => _inventoryItem ??= new InventoryItem(Guid.NewGuid(), -1, _itemType);
        private InventoryItem? _inventoryItem;
        
    }
}