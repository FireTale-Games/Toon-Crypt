using System;
using FT.Data;
using UnityEngine;

namespace FT.UI
{
    public enum ItemSlotType : byte {All, Weapon, Ability}
    
    [Serializable]
    public struct InventoryItem
    {
        [field: SerializeField] public string GuidName { get; private set; }
        public readonly Guid _guid;
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public int Index { get; private set; }
        [field: SerializeField] public InventoryItem[] _abilities;
        
        
        public InventoryItem(Guid guid = default, int id = -1, int index = -1, InventoryItem[] abilities = null)
        {
            _guid = guid;
            Id = id;
            Index = index;
            GuidName = _guid.ToString();
            _abilities = ItemDatabase.Get(Id) is Weapon item ? new InventoryItem[(byte)item.Rarity] : null;
            for (int i = 0; i < _abilities?.Length; i++)
                _abilities[i] = abilities == null ? new InventoryItem(default, -1, i) : abilities[i];
        }

        public Item Item => ItemDatabase.Get(Id);
        public Type Type => Item.GetType();
        public bool IsValid => _guid != Guid.Empty;
    }
    
    public interface IItemIcon
    {
        public InventoryItem InventoryItem { get; }
        public ItemSlotType ItemSlotType { get; }
        public void InitializeItemIcon(InventoryItem inventoryItem);
        public void ToggleVisibility(bool isVisible);
    }
}