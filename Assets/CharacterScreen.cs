using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private DescriptionPanelUi _descriptionPanel;
        [SerializeField] private Image _dragImage;
        
        private bool isDragging;

        protected override void Start()
        {
            base.Start();
            
            Character.OnCharacterInitialized += OnStateInitialized;
            IInventory _inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<IInventory>();
            foreach (IBasePanel basePanel in GetComponentsInChildren<IBasePanel>())
                basePanel.InitializePanel(_inventory);
        }
        

        private void OnStateInitialized(CharacterState State)
        {
            State.IsInventory.AddObserver(ToggleInventory);
            Character.OnCharacterInitialized -= OnStateInitialized;
        }
        
        private void ToggleInventory(bool value)
        {
            if (value) Show();
            else Hide();
        }
        
        public void OnPointerDownAction(IItemIcon draggedIcon) => 
            StartCoroutine(OnItemClick(draggedIcon));

        public void OnPointerUpAction(IItemIcon draggedIcon, IBasePanel draggedPanel)
        {
            if (!isDragging)
                return;
            
            isDragging = false;
            List<RaycastResult> results = new();
            results.RaycastHits(Input.mousePosition);
            IItemIcon hitIcon = results.Count > 0 ? results[0].gameObject.GetComponentInParent<IItemIcon>() : null;
            IBasePanel hitPanel = hitIcon is ItemIconBase iconBase ? iconBase.GetComponentInParent<IBasePanel>() : null;  
            
            if (hitIcon == null || hitPanel == null)
            {
                draggedIcon.ToggleVisibility(true);
                return;
            }

            InventoryItem draggedItem = draggedIcon.InventoryItem;
            InventoryItem hitItem = hitIcon.InventoryItem;

            if (draggedPanel.TrySwapItem(draggedIcon.InventoryItem.Type, hitIcon.ItemSlotType) &&
                draggedPanel.TrySwapItem(hitIcon.InventoryItem.Type, draggedIcon.ItemSlotType) &&
                hitPanel.TrySwapItem(draggedIcon.InventoryItem.Type, hitIcon.ItemSlotType) &&
                hitPanel.TrySwapItem(hitIcon.InventoryItem.Type, draggedIcon.ItemSlotType))
            {
                int tempIndex = hitIcon.InventoryItem.Index;
                hitPanel.HitSlot(draggedItem, tempIndex);
                draggedPanel.DragSlot(hitItem, draggedIcon.InventoryItem.Index);
                return;
            }
            
            draggedIcon.ToggleVisibility(true);
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