using System.Collections;
using System.Collections.Generic;
using FT.Tools.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FT.UI
{
    public class CharacterScreen : MonoBehaviour, IItemActionHandler<IItemUi>
    {
        [SerializeField] private DescriptionPanelUi _descriptionPanel;
        [SerializeField] private Image _dragImage; 

        private bool isDragging;

        public void OnPointerDownAction(IItemUi item) => 
            StartCoroutine(OnItemClick(item));

        public void OnPointerUpAction(IItemUi item, IBasePanel selectedPanel)
        {
            if (!isDragging)
                return;
            
            isDragging = false;
            List<RaycastResult> results = new();
            results.RaycastHits(Input.mousePosition);
            IItemUi hitItem = results.Count > 0 ? results[0].gameObject.GetComponentInParent<IItemUi>() : null;
            IBasePanel hitPanel = (hitItem as ItemUi)?.transform.GetComponentInParent<IBasePanel>();
            if (hitItem == null || !selectedPanel.CanSwapItem(hitItem.ItemType, item.InventoryItem.itemType) || 
                !hitPanel!.CanSwapItem(hitItem.ItemType, item.InventoryItem.itemType))
            {
                item.ToggleVisibility(true);
                return;
            }
            
            if (item.InventoryItem.IsValid && hitItem.InventoryItem.IsValid)
            {
                bool? swapDone = (hitPanel as WeaponPanel)?.SwapItems(item, hitItem);
                swapDone ??= selectedPanel.SwapItems(item, hitItem);
                if (!swapDone.Value)
                    item.ToggleVisibility(true);

                return;
            }
            
            InventoryItem inventoryItem = item.InventoryItem;
            selectedPanel.DeinitializeItem(item);
            hitPanel.InitializeItem(hitItem, inventoryItem);
        }

        public void OnPointerEnterAction(IItemUi item)
        {
            if (!item.InventoryItem.IsValid || isDragging)
                return;
            
            _descriptionPanel.EnableDisplay(item.InventoryItem.item);
        }

        public void OnPointerExitAction() => 
            _descriptionPanel.DisableDisplay();

        private IEnumerator OnItemClick(IItemUi item)
        {
            if (!item.InventoryItem.IsValid)
                yield break;
            
            Vector2 clickPosition = Input.mousePosition;
            while (Vector2.Distance(clickPosition, Input.mousePosition) > 20)
                yield return null;

            isDragging = true;
            Image dragImage = Instantiate(_dragImage, transform);
            dragImage.sprite = item.InventoryItem.item.Sprite;
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