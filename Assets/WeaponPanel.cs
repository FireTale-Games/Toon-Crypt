using System;
using FT.Data;
using UnityEngine;

namespace FT.Inventory
{
    public class WeaponPanel : MonoBehaviour, IBasePanel
    {
        [SerializeField] 
        
        public bool CanPlaceItem(SlotType currentItemSlotType, Type selectedItemType) => 
            currentItemSlotType.ToString() == selectedItemType.Name;

        public void Initialize(Item item)
        {
            if (item is not Weapon weapon)
                return;
            
            
        }
    }
}

/* if (hitItem.SlotType == SlotType.Weapon)
{
    for (int i = _weaponPanel.childCount - 1; i >= 2; i--)
        Destroy(_weaponPanel.GetChild(i).gameObject);

    for (int i = 0; i < 3; i++)
        Instantiate(_abilityUi, _weaponPanel);

    _weaponPanel.sizeDelta = new Vector2(74 + (_weaponPanel.childCount - 1) * 67, _weaponPanel.sizeDelta.y);
}

if (_currentItem.SlotType == SlotType.Weapon)
{
    for (int i = _weaponPanel.childCount - 1; i >= 2; i--)
        DestroyImmediate(_weaponPanel.GetChild(i).gameObject);

    _weaponPanel.sizeDelta = new Vector2(74 + (_weaponPanel.childCount - 1) * 67, _weaponPanel.sizeDelta.y);
} */