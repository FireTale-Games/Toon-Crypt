using System;
using FT.Data;
using UnityEngine;

namespace FT.Inventory
{
    public class WeaponPanel : MonoBehaviour, IBasePanel
    {
        [SerializeField] private RectTransform _weaponRectTransform; 
        [SerializeField] private InventoryItemUi _weaponSlot; 
        [SerializeField] private RectTransform _abilityUi; 
        
        public bool CanPlaceItem(SlotType currentItemSlotType, Type selectedItemType) => 
            currentItemSlotType.ToString() == selectedItemType.Name;

        public void Initialize(Item item)
        {
            if (item == null && _weaponSlot.Id == -1)
                RemoveAbilities();
            
            if (item is not Weapon weapon)
                return;
            
            AddAbilities(GetSlotNumber(weapon.Rarity));
        }

        private void RemoveAbilities()
        {
            for (int i = _weaponRectTransform.childCount - 1; i >= 2; i--)
                DestroyImmediate(_weaponRectTransform.GetChild(i).gameObject);

            _weaponRectTransform.sizeDelta = new Vector2(74 + (_weaponRectTransform.childCount - 1) * 67, _weaponRectTransform.sizeDelta.y);
        }
        
        private void AddAbilities(int slotNumber)
        {
            RemoveAbilities();
            for (int i = _weaponRectTransform.childCount - 1; i >= 2; i--)
                Destroy(_weaponRectTransform.GetChild(i).gameObject);

            for (int i = 0; i < slotNumber; i++)
                Instantiate(_abilityUi, _weaponRectTransform);

            _weaponRectTransform.sizeDelta = new Vector2(74 + (_weaponRectTransform.childCount - 1) * 67, _weaponRectTransform.sizeDelta.y);
        }

        private int GetSlotNumber(Rarity weaponRarity) =>
            weaponRarity switch
            {
                Rarity.Common => 1,
                Rarity.Uncommon => 2,
                Rarity.Rare => 3,
                Rarity.Epic => 4,
                Rarity.Legendary => 5,
                _ => 0
            };
    }
}