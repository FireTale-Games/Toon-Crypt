using System.Collections;
using FT.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace FT.UI
{
    public class CharacterScreen : MonoBehaviour, IItemActionHandler<IItemIcon>
    {
        [SerializeField] private DescriptionPanelUi _descriptionPanel;
        [SerializeField] private Image _dragImage;

        private IInventory _inventory;
        private bool isDragging;

        private void Start() => 
            _inventory = GameObject.FindWithTag("Player").GetComponent<IInventory>();

        public void OnPointerDownAction(IItemIcon item) => 
            StartCoroutine(OnItemClick(item));

        public void OnPointerUpAction(IItemIcon item)
        {
            if (!isDragging)
                return;
            
            isDragging = false;
            _inventory.InventoryChanged(item);
        }

        public void OnPointerEnterAction(IItemIcon item)
        {
            if (!item.InventoryItem.IsValid || isDragging)
                return;
            
            _descriptionPanel.EnableDisplay(item.InventoryItem.item);
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