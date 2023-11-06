using FT.Inventory;

namespace FT.UI.Panel
{
    public class InventoryPanel : BasePanel
    {
        public override void InitializePanel(IInventory inventory)
        {
            base.InitializePanel(inventory);
            Inventory?.OnInventoryUpdate.AddObserver(UpdateInventory);
            Inventory?.InitializeInventory();
        }

        public override void HitSlot(InventoryItem draggedItem, int slotIndex) => 
            Inventory.UpdateInventory(draggedItem, slotIndex);

        public override void DragSlot(InventoryItem hitItem, int slotIndex) => 
            Inventory.UpdateInventory(hitItem, slotIndex);

        private void UpdateInventory(InventoryItem inventoryItem) => 
            transform.GetChild(inventoryItem.Index).GetComponent<IItemIcon>().InitializeItemIcon(inventoryItem);
    }
}