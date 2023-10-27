using System;

namespace FT.Inventory
{
    public interface IBasePanel
    {
        public bool CanPlaceItem(SlotType currentItemSlotType, Type selectedItemType);
        public void InitializeItems(IItem initializeItem, IItem deinitializeItem);
        public void InitializePanel(int id, bool isDrag);
        public void SwapItems(IItem draggedItem, IItem selectedItem);
    }
}