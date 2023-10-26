using System;
using UnityEngine;

namespace FT.Inventory
{
    public class WeaponPanel : MonoBehaviour, IBasePanel
    {
        public bool CanPlaceItem(SlotType currentItemSlotType, Type selectedItemType)
        {
            Debug.Log(currentItemSlotType.ToString());
            Debug.Log(selectedItemType.Name);
            return currentItemSlotType.ToString() == selectedItemType.Name;
        }
    }
}