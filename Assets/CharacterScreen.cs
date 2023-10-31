using System.Collections;
using System.Collections.Generic;
using FT.Data;
using FT.Inventory;
using FT.TD;
using FT.Tools.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FT.UI.Screen
{
    public class CharacterScreen : ScreenBase, IItemIconActionHandler<IItemIcon>
    {
        [SerializeField] private RectTransform _inventoryTransform;
        [SerializeField] private RectTransform _abilityTransform;
        [SerializeField] private DescriptionPanelUi _descriptionPanel;
        [SerializeField] private Image _dragImage;
        [SerializeField] private ItemIconBase _weaponSlot;
        [SerializeField] private ItemIconBase _abilityIconSlot;
        
        public IInventory Inventory => _inventory ??= GameObject.FindGameObjectWithTag("Player").GetComponent<IInventory>();
        private IInventory _inventory;
        
        private bool isDragging;
        
        private void Awake()
        {
            Character.OnCharacterInitialized += OnStateInitialized;
            SetInventoryItemsIndex();
        }

        private void SetInventoryItemsIndex()
        {
            IItemIcon[] items = _inventoryTransform.GetComponentsInChildren<IItemIcon>();
            for (int i = 0; i < items.Length; i++)
                items[i].InitializeItemIcon(new InventoryItem(default, -1, i));
            
            _weaponSlot.InitializeItemIcon(new InventoryItem(default, -1, 255));
        }

        private void OnStateInitialized(CharacterState State)
        {
            State.IsInventory.AddObserver(ToggleInventory);
            Inventory?.OnInventoryUpdate.AddObserver(AddItemToSlot);
            Inventory?.OnWeaponUpdate.AddObserver(AddWeaponToSlot);
            Inventory?.OnAbilityUpdate.AddObserver(AddAbilityToWeapon);
            Inventory?.SetupSlots();
            Character.OnCharacterInitialized -= OnStateInitialized;
        }

        private void AddAbilityToWeapon(InventoryItem ability)
        {
            _abilityTransform.GetChild(ability.Index).GetComponent<IItemIcon>().InitializeItemIcon(ability);
        }

        private void AddWeaponToSlot(InventoryItem weaponItem)
        {
            _weaponSlot.InitializeItemIcon(weaponItem);
            if (weaponItem.IsValid)
                AddAbilitySlots(weaponItem);
            else
                RemoveAbilitySlots();
        }

        private void AddItemToSlot(InventoryItem inventoryItem) => 
            _inventoryTransform.GetChild(inventoryItem.Index).GetComponent<IItemIcon>().InitializeItemIcon(inventoryItem);

        private void ToggleInventory(bool value)
        {
            if (value) Show();
            else Hide();
        }
        
        public void OnPointerDownAction(IItemIcon draggedIcon) => 
            StartCoroutine(OnItemClick(draggedIcon));

        public void OnPointerUpAction(IItemIcon draggedIcon)
        {
            if (!isDragging)
                return;
            
            isDragging = false;
            List<RaycastResult> results = new();
            results.RaycastHits(Input.mousePosition);
            IItemIcon hitIcon = results.Count > 0 ? results[0].gameObject.GetComponentInParent<IItemIcon>() : null;
            
            if (hitIcon == null)
            {
                draggedIcon.ToggleVisibility(true);
                return;
            }

            if (draggedIcon != _weaponSlot.GetComponent<IItemIcon>() && hitIcon != _weaponSlot.GetComponent<IItemIcon>() &&
                draggedIcon.InventoryItem.Type != typeof(Data.Ability) && hitIcon.ItemSlotType != ItemSlotType.Ability)
            {
                Inventory?.UpdateInventory(draggedIcon.InventoryItem, hitIcon.InventoryItem);
                return;
            }

            if (IsWeaponEquip(draggedIcon, hitIcon))
            {
                Inventory?.UpdateWeapon(draggedIcon.InventoryItem, hitIcon.InventoryItem);
                return;
            }

            if (IsAbilityEquip(draggedIcon, hitIcon))
            {
                Inventory?.UpdateAbility(draggedIcon.InventoryItem, hitIcon.InventoryItem);
                return;
            }
            
            draggedIcon.ToggleVisibility(true);
        }

        private bool IsWeaponEquip(IItemIcon draggedIcon, IItemIcon hitIcon)
        {
            if (draggedIcon.InventoryItem.Type == typeof(Weapon) && !hitIcon.InventoryItem.IsValid)
                return true;
            
            if (hitIcon.ItemSlotType == ItemSlotType.Weapon && draggedIcon.InventoryItem.Type == typeof(Weapon))
                return true;
            
            return false;
        }

        private bool IsAbilityEquip(IItemIcon draggedIcon, IItemIcon hitIcon)
        {
            if (draggedIcon.InventoryItem.Type == typeof(Data.Ability) && !hitIcon.InventoryItem.IsValid)
                return true;

            if (hitIcon.ItemSlotType == ItemSlotType.Ability && draggedIcon.InventoryItem.Type == typeof(Data.Ability))
                return true;

            return false;
        }
        
        public void OnPointerEnterAction(IItemIcon draggedIcon)
        {
            if (!draggedIcon.InventoryItem.IsValid || isDragging)
                return;
            
            _descriptionPanel.EnableDisplay(draggedIcon.InventoryItem.Item);
        }

        public void OnPointerExitAction() => 
            _descriptionPanel.DisableDisplay();

        private void AddAbilitySlots(InventoryItem weaponItem)
        {   
            RemoveAbilitySlots();
            for (int i = _abilityTransform.childCount - 1; i >= 0; i--)
                DestroyImmediate(_abilityTransform.GetChild(i).gameObject);
                
            _abilityTransform.sizeDelta = new Vector2(74 + ((byte)weaponItem.Item.Rarity - 1) * 67, _abilityTransform.sizeDelta.y);

            for (int i = 0; i < (byte)weaponItem.Item.Rarity; i++)
            {
                IItemIcon abilityIcon = Instantiate(_abilityIconSlot, _abilityTransform);
                abilityIcon.InitializeItemIcon(weaponItem._abilities[i]);
            }
        }
        
        private void RemoveAbilitySlots()
        {
            _abilityTransform.sizeDelta = new Vector2(0, _abilityTransform.sizeDelta.y);
            for (int i = _abilityTransform.childCount - 1; i >= 0; i--)
                DestroyImmediate(_abilityTransform.GetChild(i).gameObject);
        }
        
        private IEnumerator OnItemClick(IItemIcon draggedIcon)
        {
            if (!draggedIcon.InventoryItem.IsValid)
                yield break;
            
            Vector2 clickPosition = Input.mousePosition;
            while (Vector2.Distance(clickPosition, Input.mousePosition) > 20)
                yield return null;

            isDragging = true;
            Image dragImage = Instantiate(_dragImage, transform);
            dragImage.sprite = draggedIcon.InventoryItem.Item.Sprite;
            draggedIcon.ToggleVisibility(false);
            while (isDragging)
            {
                dragImage.rectTransform.position = Input.mousePosition;
                yield return null;
            }
            
            Destroy(dragImage.gameObject);
        }
    }
}