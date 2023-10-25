using System.Collections;
using System.Collections.Generic;
using FT.Data;
using FT.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FT.Inventory
{
    public class InventoryScreen : MonoBehaviour, IInventory<IItem>
    {
        [SerializeField] private Image _itemUi;
        [SerializeField] private Transform _inventoryPanel;
        [SerializeField] private DescriptionPanelUi _descriptionPanel;
        [SerializeField] private Button _addItemButton;
        [SerializeField] private Button _removeItemButton;
        private GraphicRaycaster _graphicRaycaster;
        
        private IItem _currentItem;
        private bool isDragging;
        private Image _dragItem;
        
        private void Awake()
        {
            _graphicRaycaster = GetComponent<GraphicRaycaster>();
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

        public void ItemStopDrag(IItem item)
        {
            if (!_dragItem)
            {
                StopAllCoroutines();
                return;
            }
            
            PointerEventData pointerEventData = new(EventSystem.current);
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new();
            _graphicRaycaster.Raycast(pointerEventData, results);

            if (results.Count <= 0)
            {
                _currentItem.ToggleVisibility(true);
                Destroy(_dragItem.gameObject);
                StopAllCoroutines();
                isDragging = false;
                return;
            }
            
            IItem hitItem = results[0].gameObject.GetComponentInParent<IItem>();
            if (hitItem == null)
                return;

            _descriptionPanel.DisplayItem(ItemDatabase.Get<Data.Ability>(_currentItem.Id));
            _descriptionPanel.gameObject.SetActive(true);
            
            int Id = hitItem.Id;
            hitItem.InitializeItem(item.Id);
            
            if (Id != -1) item.InitializeItem(Id);
            else item.DeinitializeItem();
            
            Destroy(_dragItem.gameObject);
            StopAllCoroutines();
            isDragging = false;
        }

        public void MouseEnterFrame(IItem item)
        {
            if (item.Id == -1 || isDragging)
                return;
            
            _descriptionPanel.DisplayItem(ItemDatabase.Get<Data.Ability>(item.Id));
            _descriptionPanel.gameObject.SetActive(true);
        }

        public void MouseExitFrame()
        {
            _descriptionPanel.gameObject.SetActive(false);
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