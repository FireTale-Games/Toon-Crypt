using System;
using FT.Data;

namespace FT.UI
{
    public enum ItemType : sbyte { All, Weapon, Ability }
    
    [System.Serializable]
    public readonly struct InventoryItem
    {
        public readonly Guid _guid;
        public readonly int _id;
        public readonly ItemType _itemType;
        
        public InventoryItem(Guid itemGuid, int itemId, ItemType itemType)
        {
            _guid = itemGuid;
            _id = itemId;
            _itemType = itemType;
        }

        public Item item => ItemDatabase.Get(_id);
    }

    public interface IItemUi
    {
        public InventoryItem InventoryItem { get; }
    }
}