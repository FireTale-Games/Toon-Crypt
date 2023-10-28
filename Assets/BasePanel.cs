using System;
using UnityEngine;

namespace FT.UI
{
    public class BasePanel : MonoBehaviour, IBasePanel
    {
        public virtual bool CanSwapItem(ItemType targetType, Type itemType) => true;

        public virtual void InitializeItem(IItemUi item, int initializeId) => 
            item.Initialize(initializeId);

        public virtual void DeinitializeItem(IItemUi item) => 
            item.DeinitializeItem();

        public virtual bool SwapItems(IItemUi selectedItem, IItemUi hitItem)
        {
            int id = selectedItem.InventoryItem._id;
            selectedItem.Initialize(hitItem.InventoryItem._id);
            hitItem.Initialize(id);
            return true;
        }
    }
}