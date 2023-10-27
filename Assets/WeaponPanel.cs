using System;
using FT.TD;
using UnityEngine;

namespace FT.UI
{
    public class WeaponPanel : BasePanel
    {
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
        }

        public override void DeinitializeItem(IItemUi item)
        {
            if (item.InventoryItem._itemType == ItemType.Ability)
                GameObject.FindWithTag("Player").GetComponent<Character>().State.AddSpell.Set(new SpellStruct(item.InventoryItem._id, false));
            
            base.DeinitializeItem(item);
        }
    }
}