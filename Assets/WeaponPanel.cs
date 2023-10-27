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

        public override void InitializeItem(IItemUi item, int initializeId)
        {
            base.InitializeItem(item, initializeId);
            if (item.InventoryItem._itemType == ItemType.Ability)
                GameObject.FindWithTag("Player").GetComponent<Character>().State.AddSpell.Set(new SpellStruct(item.InventoryItem._id, true));
            else if (item.InventoryItem._itemType == ItemType.Weapon)
                AddAbilitySlots(item.InventoryItem.item.Rarity);
        }

        //public override void SwapItems(IItemUi selectedItem, IItemUi hitItem)
        //{
        //    base.SwapItems(selectedItem, hitItem);
        //    if (selectedItem.InventoryItem._itemType == ItemType.Weapon)
        //        AddAbilitySlots(selectedItem.InventoryItem.item.Rarity);
        //    else if (hitItem.InventoryItem._itemType == ItemType.Weapon)
        //        AddAbilitySlots(hitItem.InventoryItem.item.Rarity);
        //}

        private void AddAbilitySlots(Rarity rarity)
        {
            RemoveAbilitySlots();

            int childNumber = rarity switch
            {
                Rarity.Common => 1,
                Rarity.Uncommon => 2,
                Rarity.Rare => 3,
                Rarity.Epic => 4,
                Rarity.Legendary => 5,
                _ => 0
            };

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
            if (item.InventoryItem._itemType == ItemType.Ability)
                GameObject.FindWithTag("Player").GetComponent<Character>().State.AddSpell.Set(new SpellStruct(item.InventoryItem._id, false));

            if (item.InventoryItem._itemType == ItemType.Weapon)
                RemoveAbilitySlots();
            
            base.DeinitializeItem(item);
        }
    }
}