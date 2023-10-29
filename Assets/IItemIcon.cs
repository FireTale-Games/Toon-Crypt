using System;
using FT.Data;
using UnityEngine;

namespace FT.UI
{
    [Serializable]
    public struct InventoryItem
    {
        [field: SerializeField] public string GuidName { get; private set; }
        public readonly Guid _guid;
        [field: SerializeField] public int Id { get; private set; }

        public InventoryItem(int id)
        {
            _guid = Guid.NewGuid();
            Id = id;
            GuidName = _guid.ToString();
        }

        public Item Item => ItemDatabase.Get(Id);
        public bool IsValid => _guid != Guid.Empty;
    }
    
    public interface IItemIcon
    {
        public InventoryItem InventoryItem { get; }
        public void InitializeItemIcon(InventoryItem inventoryItem);
    }
}