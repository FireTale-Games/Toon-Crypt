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

        public override bool TrySwapItem(Type draggedItem, ItemSlotType hitItem) => 
            (draggedItem == null || draggedItem == typeof(Weapon)) && hitItem is ItemSlotType.Weapon or ItemSlotType.All;

        private void UpdateWeapon(InventoryItem inventoryItem)
        {
            transform.GetChild(0).GetComponent<IItemIcon>().InitializeItemIcon(inventoryItem);
            
            if (inventoryItem.IsValid)
                AddAbilitySlots((byte)inventoryItem.Item.Rarity, inventoryItem._abilities);
            else
                RemoveAbilitySlots();
        }

        private void AddAbilitySlots(byte raritySlots, int[] abilities)
        {
            RemoveAbilitySlots();
            for (byte i = 0; i < raritySlots; i++)
            {
                IItemIcon _ability = Instantiate(_abilitySlot, _abilityPanel);
                _ability.InitializeItemIcon(abilities[i] != 0 ? new InventoryItem(abilities[i], i) : new InventoryItem(0, i));
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