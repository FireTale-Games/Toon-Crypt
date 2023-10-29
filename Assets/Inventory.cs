using System.Collections.Generic;
using FT.Tools.Extensions;
using FT.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FT.Inventory
{
    public class Inventory : MonoBehaviour, IInventory
    {
        public void InventoryChanged(IItemIcon draggedItem)
        {
            List<RaycastResult> results = new();
            results.RaycastHits(Input.mousePosition);
            IItemIcon hitItem = results.Count > 0 ? results[0].gameObject.GetComponentInParent<IItemIcon>() : null;
            if (hitItem == null)
            {
                draggedItem.ToggleVisibility(true);
                return;
            }
            
            InventoryItem dragItem = draggedItem.InventoryItem;
            if (draggedItem.InventoryItem.IsValid && hitItem.InventoryItem.IsValid)
            {
                draggedItem.Initialize(hitItem.InventoryItem);
                hitItem.Initialize(dragItem);
                
                return;
            }
            
            hitItem.Initialize(dragItem);
            draggedItem.Initialize(hitItem.InventoryItem);
        }
    }
}