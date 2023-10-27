using System;

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
    }
}