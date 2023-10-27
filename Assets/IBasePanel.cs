using System;

namespace FT.UI
{
    public interface IBasePanel
    {
        public bool CanSwapItem(ItemType targetType, Type itemType);
        public void InitializeItem(IItemUi item, int initializeId);
        public void DeinitializeItem(IItemUi item);
        public void SwapItems(IItemUi selectedItem, IItemUi hitItem);
    }
}