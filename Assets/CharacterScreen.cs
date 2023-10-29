using FT.Inventory;
using UnityEngine;

namespace FT.UI.Screen
{
    public class CharacterScreen : MonoBehaviour
    {
        [SerializeField] private Transform _inventoryTransform;

        private void Awake()
        {
            GameObject.FindWithTag("Player").GetComponent<IInventory>()?.OnItemAdded.AddObserver(ItemAdded);
        }

        private void ItemAdded(InventoryItem inventoryItem)
        {
            IItemIcon[] _iconsBase = _inventoryTransform.GetComponentsInChildren<IItemIcon>();
            foreach (IItemIcon itemIcon in _iconsBase)
            {
                if (itemIcon.InventoryItem.IsValid)
                    continue;
                
                itemIcon.InitializeItemIcon(inventoryItem);
                break;
            }
        }
    }
}