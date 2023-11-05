using System;
using FT.Inventory;

namespace FT.UI
{
    public interface IBasePanel
    {
        public bool TrySwapItem(Type draggedItem, ItemSlotType hitItem);
        public void HitSlot(InventoryItem draggedItem, int slotIndex);
        public void DragSlot(InventoryItem hitItem, int slotIndex);
        public void InitializePanel(IInventory inventory);
    }
}