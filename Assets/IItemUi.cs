using System;
using FT.Data;

namespace FT.UI
{
    public enum ItemType : sbyte { All, Weapon, Ability }
    
    public readonly struct InventoryItem
    {
        public readonly Guid _guid;
        public readonly int _id;
        public readonly InventoryItem[] _abilities;
        
        public InventoryItem(Guid itemGuid, int itemId, InventoryItem[] abilities)
        {
            _guid = itemGuid;
            _id = itemId;
            _abilities = abilities;
        }

        public bool IsValid => _guid != Guid.Empty;
        public Item item => ItemDatabase.Get(_id);
        public Type itemType => item.GetType();
    }

    public interface IItemUi
    {
        public InventoryItem InventoryItem { get; }
        public ItemType ItemType { get; }
        public void Initialize(InventoryItem inventoryItem);
        public void DeinitializeItem();
        public void ToggleVisibility(bool isVisible);
    }
}