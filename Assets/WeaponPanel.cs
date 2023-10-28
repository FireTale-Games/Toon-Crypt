using System;
using FT.Data;
using FT.TD;
using UnityEngine;

namespace FT.UI
{
    public class WeaponPanel : BasePanel
    {
        [SerializeField] private RectTransform _abilityRectTransform;
        [SerializeField] private RectTransform _itemUi;
        
        public override bool CanSwapItem(ItemType targetType, Type itemType)
        {
            if (targetType == ItemType.All) 
                return true;
            
            return targetType.ToString() == itemType.Name;
        }

        public override void InitializeItem(IItemUi item, InventoryItem inventoryItem)
        {
            base.InitializeItem(item, inventoryItem);
            if (item.ItemType == ItemType.Ability)
                GameObject.FindWithTag("Player").GetComponent<Character>().State.AddSpell.Set(new SpellStruct(item.InventoryItem._id, true));
            else if (item.ItemType == ItemType.Weapon)
                AddAbilitySlots((int)item.InventoryItem.item.Rarity);
        }

        public override bool SwapItems(IItemUi selectedItem, IItemUi hitItem)
        {
            if (selectedItem.InventoryItem.item.GetType().Name != hitItem.InventoryItem.itemType.Name)
                return false;
            
            base.SwapItems(selectedItem, hitItem);
                
            if (selectedItem.ItemType == ItemType.Weapon)
                AddAbilitySlots((int)selectedItem.InventoryItem.item.Rarity);
            else if (hitItem.ItemType == ItemType.Weapon)
                AddAbilitySlots((int)hitItem.InventoryItem.item.Rarity);
                
            return true;

        }

        private void AddAbilitySlots(int childNumber)
        {
            RemoveAbilitySlots();
            
            _abilityRectTransform.sizeDelta = new Vector2(74 + (childNumber - 1) * 67, _abilityRectTransform.sizeDelta.y);
            for (int i = 0; i < childNumber; i++)
                Instantiate(_itemUi, _abilityRectTransform);
        }

        private void RemoveAbilitySlots()
        {
            for (int i = _abilityRectTransform.childCount - 1; i >= 0; i--)
                DestroyImmediate(_abilityRectTransform.GetChild(i).gameObject);

            _abilityRectTransform.sizeDelta = new Vector2(0, _abilityRectTransform.sizeDelta.y);
        }

        public override void DeinitializeItem(IItemUi item)
        {
            if (item.ItemType == ItemType.Ability)
                GameObject.FindWithTag("Player").GetComponent<Character>().State.AddSpell.Set(new SpellStruct(item.InventoryItem._id, false));

            if (item.ItemType == ItemType.Weapon)
                RemoveAbilitySlots();
            
            base.DeinitializeItem(item);
        }
    }
}