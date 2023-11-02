using System;
using FT.Data;
using FT.Inventory;
using UnityEngine;

namespace FT.UI
{
    public class WeaponPanel : BasePanel
    {
        [SerializeField] private RectTransform _abilityPanel;
        [SerializeField] private ItemIconBase _abilitySlot;
        
        public override void InitializePanel(IInventory inventory)
        {
            base.InitializePanel(inventory);
            Inventory?.OnWeaponUpdate.AddObserver(UpdateWeapon);
            Inventory?.InitializeInventory();
        }

        public override void HitSlot(InventoryItem draggedItem, int slotIndex) => 
            Inventory.UpdateWeapon(draggedItem, slotIndex);

        public override void DragSlot(InventoryItem hitItem, int slotIndex) => 
            Inventory.UpdateWeapon(hitItem, slotIndex);

        public override bool TrySwapItem(Type draggedItem, ItemSlotType hitItem)
        {
            if ((draggedItem == null || draggedItem == typeof(Weapon)) && hitItem == ItemSlotType.Weapon)
                return true;

            return false;
        }

        private void UpdateWeapon(InventoryItem inventoryItem)
        {
            transform.GetChild(0).GetComponent<IItemIcon>().InitializeItemIcon(inventoryItem);
            
            if (inventoryItem.IsValid)
                AddAbilitySlots((byte)inventoryItem.Item.Rarity);
            else
                RemoveAbilitySlots();
        }

        private void AddAbilitySlots(byte raritySlots)
        {
            RemoveAbilitySlots();
            for (byte i = 0; i < raritySlots; i++)
            {
                Instantiate(_abilitySlot, _abilityPanel);
                _abilityPanel.sizeDelta = new Vector2(74 + (raritySlots - 1) * 67, _abilityPanel.sizeDelta.y);
            }
        }
        
        private void RemoveAbilitySlots()
        {
            _abilityPanel.sizeDelta = new Vector2(0, _abilityPanel.sizeDelta.y);
            for (int i = _abilityPanel.childCount - 1; i >= 0; i--)
                DestroyImmediate(_abilityPanel.GetChild(i).gameObject);
        }
    }
}