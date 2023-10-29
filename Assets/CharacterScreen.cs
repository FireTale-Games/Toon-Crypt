using System.Collections;
using System.Collections.Generic;
using FT.Inventory;
using FT.Tools.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FT.UI.Screen
{
    public class CharacterScreen : MonoBehaviour, IItemIconActionHandler<IItemIcon>
    {
        [SerializeField] private Transform _inventoryTransform;
        [SerializeField] private DescriptionPanelUi _descriptionPanel;
        [SerializeField] private Image _dragImage;

        private bool isDragging;
        
        private void Awake()
        {
            GameObject.FindWithTag("Player").GetComponent<IInventory>()?.OnItemAdded.AddObserver(ItemAdded);
        }

        private void ItemAdded(InventoryItem inventoryItem)
        {
            IItemIcon _iconBase = _inventoryTransform.GetChild(inventoryItem.Index).GetComponent<IItemIcon>();
            _iconBase.InitializeItemIcon(inventoryItem);
        }

        public void OnPointerDownAction(IItemIcon item) => 
            StartCoroutine(OnItemClick(item));

        public void OnPointerUpAction(IItemIcon item)
        {
            if (!isDragging)
                return;
            
            isDragging = false;
            List<RaycastResult> results = new();
            results.RaycastHits(Input.mousePosition);
            IItemIcon hitItem = results.Count > 0 ? results[0].gameObject.GetComponentInParent<IItemIcon>() : null;
            if (hitItem == null)
            {
                item.ToggleVisibility(true);
                return;
            }
            
            InventoryItem inventoryItem = item.InventoryItem;
            item.InitializeItemIcon(hitItem.InventoryItem);
            hitItem.InitializeItemIcon(inventoryItem);
        }

        public void OnPointerEnterAction(IItemIcon item)
        {
            if (!item.InventoryItem.IsValid || isDragging)
                return;
            
            _descriptionPanel.EnableDisplay(item.InventoryItem.Item);
        }

        public void OnPointerExitAction() => 
            _descriptionPanel.DisableDisplay();

        private IEnumerator OnItemClick(IItemIcon item)
        {
            if (!item.InventoryItem.IsValid)
                yield break;
            
            Vector2 clickPosition = Input.mousePosition;
            while (Vector2.Distance(clickPosition, Input.mousePosition) > 20)
                yield return null;

            isDragging = true;
            Image dragImage = Instantiate(_dragImage, transform);
            dragImage.sprite = item.InventoryItem.Item.Sprite;
            item.ToggleVisibility(false);
            while (isDragging)
            {
                dragImage.rectTransform.position = Input.mousePosition;
                yield return null;
            }
            
            Destroy(dragImage.gameObject);
        }
    }
}