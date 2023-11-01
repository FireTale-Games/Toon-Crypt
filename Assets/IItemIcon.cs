using System;
using FT.Data;
using UnityEngine;

namespace FT.UI
{
    public enum ItemSlotType : byte {All, Weapon, Ability}
    
    [Serializable]
    public struct InventoryItem
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public int Index { get; private set; }
        [field: SerializeField] public int[] _abilities;
        
        public InventoryItem(int id = 0, int index = 0, int[] abilities = null)
        {
            Id = id;
            Index = index;
            _abilities = ItemDatabase.Get(Id) is Weapon item ? new int[(byte)item.Rarity] : null;
            for (int i = 0; i < _abilities?.Length; i++)
                _abilities[i] = abilities == null ? 0 : abilities[i];
        }

        public Item Item => ItemDatabase.Get(Id);
        public Type Type => Item.GetType();
        public bool IsValid => Id != 0;
        public void SetIndex(int index) => Index = index;
    }
    
    public interface IItemIcon
    {
        public InventoryItem InventoryItem { get; }
        public ItemSlotType ItemSlotType { get; }
        public void InitializeItemIcon(InventoryItem inventoryItem);
        public void ToggleVisibility(bool isVisible);
    }
}