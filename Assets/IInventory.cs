using FT.UI;

namespace FT.Inventory
{
    public interface IInventory
    {
        public void InventoryChanged(IItemIcon draggedItem);
    }
}