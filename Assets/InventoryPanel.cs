using System;
using UnityEngine;

namespace FT.Inventory
{
    public class InventoryPanel : MonoBehaviour, IBasePanel
    {
        public bool CanPlaceItem(SlotType currentItemSlotType, Type selectedItemType) => true;
    }
}