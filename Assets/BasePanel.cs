using System;
using UnityEngine;

namespace FT.Inventory
{
    public class BasePanel : MonoBehaviour, IBasePanel
    {
        public virtual bool CanPlaceItem(SlotType currentItemSlotType, Type selectedItemType) => true;

        public virtual void InitializeItems(IItem initializeItem, IItem deinitializeItem)
        {
            deinitializeItem.InitializeItem(initializeItem.Id);
            initializeItem.DeinitializeItem();
        }

        public virtual void InitializePanel(int id, bool isDrag) { }

        public virtual void SwapItems(IItem draggedItem, IItem selectedItem)
        {
            int Id = draggedItem.Id;
            draggedItem.InitializeItem(selectedItem.Id);
            selectedItem.InitializeItem(Id);
        }
    }
}