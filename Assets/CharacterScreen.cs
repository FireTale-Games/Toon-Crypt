using System.Collections;
using System.Collections.Generic;
using FT.Data;
using FT.Tools.Extensions;
using FT.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FT.Inventory
{
    public class CharacterScreen : MonoBehaviour, IItemActionHandler<IItem>
    {
        [SerializeField] private Image _itemUi;
        [SerializeField] private Transform _inventoryPanel;
        [SerializeField] private DescriptionPanelUi _descriptionPanel;
        [SerializeField] private Button _addItemButton;
        [SerializeField] private Button _removeItemButton;
        
        private IItem _currentItem;
        private bool isDragging;
        private Image _dragItem;
        
        private void Awake()
        {
            _addItemButton.onClick.AddListener(() =>
            {
                foreach (IItem itemUi in _inventoryPanel.GetComponentsInChildren<IItem>())
                {
                    if (itemUi.Id != -1)
                        continue;

                    Item item = ItemDatabase.GetRandomItem();
                    itemUi.InitializeItem(item.Id);
                    break;
                }
            });
            
            
            _removeItemButton.onClick.AddListener(() => 
            { 
                _currentItem?.DeinitializeItem();
                _currentItem = null;
            });
        }

        public void ItemLeftClicked(IItem item) =>
            StartCoroutine(ItemDrag(item));

        public void MouseEnterFrame(IItem item)
        {
            if (item.Id == -1 || isDragging)
                return;
            
            _descriptionPanel.DisplayItem(ItemDatabase.Get(item.Id));
        }

        public void MouseExitFrame() => 
            _descriptionPanel.ToggleVisibility(false);
        
        public void ItemStopDrag()
        {
            if (!_dragItem)
            {
                StopAllCoroutines();
                return;
            }
            
            List<RaycastResult> results = new();
            results.GetHitResults(Input.mousePosition);
            IItem hitItem = results.Count > 0 ? results[0].gameObject.GetComponentInParent<IItem>() : null;
            Item tempItem = ItemDatabase.Get(_currentItem.Id);
            if (hitItem?.BasePanel == null || !hitItem.BasePanel.CanPlaceItem(hitItem.SlotType, tempItem.GetType()))
            {
                _currentItem.ToggleVisibility(true);
                Destroy(_dragItem.gameObject);
                StopAllCoroutines();
                isDragging = false;
                return;
            }

            ExecuteEndDrag(_currentItem, hitItem);
            
            Destroy(_dragItem.gameObject);
            StopAllCoroutines();
            isDragging = false;
            
            _descriptionPanel.DisplayItem(tempItem);
        }

        private void ExecuteEndDrag(IItem draggedItem, IItem selectedItem)
        {
            IBasePanel draggedPanel = draggedItem.BasePanel;
            IBasePanel selectedPanel = selectedItem.BasePanel;

            if (draggedPanel == selectedPanel)
            {
                selectedPanel.SwapItems(draggedItem, selectedItem);
                return;
            }

            if (draggedPanel is WeaponPanel)
            {
                draggedPanel.InitializeItems(draggedItem, selectedItem);
                draggedPanel.InitializePanel(selectedItem.Id, false);
            }
            else
            {
                draggedPanel.InitializeItems(draggedItem, selectedItem);
                selectedPanel.InitializePanel(selectedItem.Id, true);
            }
        }
        
        private IEnumerator ItemDrag(IItem item)
        {
            if (item.Id == -1)
                yield break;
            
            _currentItem = item;
            Vector2 mouseClickPosition = Input.mousePosition;
            while (!isDragging)
            {
                if (Vector2.Distance(mouseClickPosition, Input.mousePosition) >= 20)
                    isDragging = true;

                yield return null;
            }

            _dragItem = Instantiate(_itemUi, transform);
            _dragItem.sprite = ItemDatabase.Get(_currentItem.Id).Sprite;
            _currentItem.ToggleVisibility(false);
            while (true)
            {
                _dragItem.rectTransform.position = Input.mousePosition;
                yield return null;
            }
        }
    }
}