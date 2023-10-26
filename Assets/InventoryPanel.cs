using System;
using FT.Data;
using UnityEngine;

namespace FT.Inventory
{
    public class InventoryPanel : MonoBehaviour, IBasePanel
    {
        public bool CanPlaceItem(SlotType currentItemSlotType, Type selectedItemType) => true;
        public void Initialize(Item item) { }
    }
}