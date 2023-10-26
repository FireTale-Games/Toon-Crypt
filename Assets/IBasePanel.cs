using System;

namespace FT.Inventory
{
    public interface IBasePanel
    {
        public bool CanPlaceItem(SlotType currentItemSlotType, Type selectedItemType);
    }
}