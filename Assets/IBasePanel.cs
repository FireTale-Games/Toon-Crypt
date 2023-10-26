using System;
using FT.Data;

namespace FT.Inventory
{
    public interface IBasePanel
    {
        public bool CanPlaceItem(SlotType currentItemSlotType, Type selectedItemType);
        public void Initialize(Item item);
    }
}