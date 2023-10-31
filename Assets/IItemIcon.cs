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

        public InventoryItem(Guid guid = default, int id = -1, int index = -1)
        {
            _guid = guid;
            Id = id;
            Index = index;
            GuidName = _guid.ToString();
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