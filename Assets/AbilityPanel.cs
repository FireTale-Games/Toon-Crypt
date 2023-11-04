using System;
using FT.Inventory;

namespace FT.UI
{
    public class AbilityPanel : BasePanel
    {
        public override void InitializePanel(IInventory inventory)
        {
            base.InitializePanel(inventory);
            Inventory?.OnAbilityUpdate.AddObserver(UpdateAbility);
            Inventory?.InitializeInventory();
        }

        public override void HitSlot(InventoryItem draggedItem, int slotIndex) => 
            Inventory.UpdateAbility(draggedItem, slotIndex);

        public override void DragSlot(InventoryItem hitItem, int slotIndex) => 
            Inventory.UpdateAbility(hitItem, slotIndex);

        public override bool TrySwapItem(Type draggedItem, ItemSlotType hitItem) => 
            (draggedItem == null || draggedItem == typeof(Data.Ability)) && hitItem is ItemSlotType.Ability or ItemSlotType.All;

        private void UpdateAbility(int id, int index) => 
            transform.GetChild(index).GetComponent<IItemIcon>().InitializeItemIcon(new InventoryItem(id, index));
    }
}