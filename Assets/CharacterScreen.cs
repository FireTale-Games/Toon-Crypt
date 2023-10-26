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
        [SerializeField] private InventoryItemUi _abilityUi;
        [SerializeField] private Transform _inventoryPanel;
        [SerializeField] private RectTransform _weaponPanel;
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
                _currentItem?.InitializeItem();
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
            
            List<RaycastResult> results = new();
            results.GetHitResults(Input.mousePosition);
            Debug.Log(item.SlotType);
            Debug.Log(_currentItem.GetType());
            IItem hitItem = results.Count > 0 ? results[0].gameObject.GetComponentInParent<IItem>() : null;
            if (hitItem?.BasePanel == null || !hitItem.BasePanel.CanPlaceItem(item.SlotType, _currentItem.GetType()))
            {
                _currentItem.ToggleVisibility(true);
                Destroy(_dragItem.gameObject);
                StopAllCoroutines();
                isDragging = false;
                return;
            }

            _descriptionPanel.DisplayItem(ItemDatabase.Get(_currentItem.Id));
            
            int Id = hitItem.Id;
            hitItem.InitializeItem(item.Id);
            item.InitializeItem(Id);

             
            Destroy(_dragItem.gameObject);
            StopAllCoroutines();
            isDragging = false;
        }

        public void MouseEnterFrame(IItem item)
        {
            if (item.Id == -1 || isDragging)
                return;
            
            _descriptionPanel.DisplayItem(ItemDatabase.Get(item.Id));
        }

        public void MouseExitFrame()
        {
            _descriptionPanel.ToggleVisibility(false);
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

//(hitItem.SlotType != SlotType.All && hitItem.SlotType.ToString() != dataItem.GetType().Name)

/* if (hitItem.SlotType == SlotType.Weapon)
{
    for (int i = _weaponPanel.childCount - 1; i >= 2; i--)
        Destroy(_weaponPanel.GetChild(i).gameObject);

    for (int i = 0; i < 3; i++)
        Instantiate(_abilityUi, _weaponPanel);
                
    _weaponPanel.sizeDelta = new Vector2(74 + (_weaponPanel.childCount - 1) * 67, _weaponPanel.sizeDelta.y);
}

if (_currentItem.SlotType == SlotType.Weapon)
{
    for (int i = _weaponPanel.childCount - 1; i >= 2; i--)
        DestroyImmediate(_weaponPanel.GetChild(i).gameObject);

    _weaponPanel.sizeDelta = new Vector2(74 + (_weaponPanel.childCount - 1) * 67, _weaponPanel.sizeDelta.y);
} */