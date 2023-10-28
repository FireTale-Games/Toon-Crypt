using System;
using UnityEngine;

namespace FT.UI
{
    public class BasePanel : MonoBehaviour, IBasePanel
    {
        public virtual bool CanSwapItem(ItemType targetType, Type itemType) => true;

        public virtual void InitializeItem(IItemUi item, InventoryItem inventoryItem) => 
            item.Initialize(inventoryItem);

        public virtual void DeinitializeItem(IItemUi item) => 
            item.DeinitializeItem();

        public virtual bool SwapItems(IItemUi selectedItem, IItemUi hitItem)
        {
            InventoryItem item = selectedItem.InventoryItem;
            selectedItem.Initialize(hitItem.InventoryItem);
            hitItem.Initialize(item);
            return true;
        }
    }
}