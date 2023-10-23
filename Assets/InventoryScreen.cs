using UnityEngine;

namespace FT.Inventory
{
    public class ItemScreen : MonoBehaviour, IInventory<IItem>
    {
        public void ItemLeftClicked(IItem item)
        {
            Debug.Log(item.GetImageName());
        }

        public void ItemStopDrag(IItem item)
        {
            Debug.Log(item.GetImageName());
        }
    }
}