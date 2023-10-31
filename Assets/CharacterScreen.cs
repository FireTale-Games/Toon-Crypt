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
        [SerializeField] private Transform _inventoryTransform;
        [SerializeField] private DescriptionPanelUi _descriptionPanel;
        [SerializeField] private Image _dragImage;
        [SerializeField] private ItemIconBase _weaponSlot;
        
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
            Inventory?.SetupSlots();
            Character.OnCharacterInitialized -= OnStateInitialized;
        }

        private void AddWeaponToSlot(InventoryItem weaponItem)
        {
            _weaponSlot.InitializeItemIcon(weaponItem);
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

            if (draggedIcon != _weaponSlot.GetComponent<IItemIcon>() && hitIcon != _weaponSlot.GetComponent<IItemIcon>())
                Inventory?.UpdateInventory(draggedIcon.InventoryItem, hitIcon.InventoryItem);
            
            if (IsWeaponEquip(draggedIcon, hitIcon))
                Inventory?.UpdateWeapon(draggedIcon.InventoryItem, hitIcon.InventoryItem);
            
        }

        private bool IsWeaponEquip(IItemIcon draggedIcon, IItemIcon hitIcon)
        {
            IItemIcon weaponSlot = _weaponSlot.GetComponent<IItemIcon>();
            if (draggedIcon ==weaponSlot && (!hitIcon.InventoryItem.IsValid || hitIcon.InventoryItem.Type == typeof(Weapon)))
                return true;

            if (hitIcon == weaponSlot && draggedIcon.InventoryItem.Type == typeof(Weapon))
                return true;
            
            if (draggedIcon == weaponSlot || hitIcon == weaponSlot)
                draggedIcon.ToggleVisibility(true);
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