using System;
using FT.Inventory;
using UnityEngine;

namespace FT.UI
{
    public abstract class BasePanel : MonoBehaviour, IBasePanel
    {
        protected IInventory Inventory;
        
        private void InitializeSlots()
        {
            IItemIcon[] itemIcon = transform.GetComponentsInChildren<IItemIcon>();
            for (int i = 0; i < itemIcon.Length; i++)
                itemIcon[i].InitializeItemIcon(new InventoryItem(0, i));
        }

        public virtual bool TrySwapItem(Type draggedItem, ItemSlotType hitItem) => true;
        public abstract void HitSlot(InventoryItem draggedItem, int slotIndex);
        public abstract void DragSlot(InventoryItem hitItem, int slotIndex);
        
        public virtual void InitializePanel(IInventory inventory)
        {
            Inventory = inventory;
            InitializeSlots();
        }
    }
}