using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace FT.UI
{
    public class CharacterScreen : MonoBehaviour, IItemActionHandler<IItemUi>
    {
        [SerializeField] private DescriptionPanelUi _descriptionPanel;
        [SerializeField] private Image _dragImage; 
        
        public IItemUi CurrentItem { get; private set; }
        private bool isDragging;

        public void OnPointerDownAction(IItemUi item) => 
            StartCoroutine(OnItemClick(item));

        public void OnPointerUpAction()
        {
           isDragging = false;
        }

        public void OnPointerEnterAction(IItemUi item)
        {
            if (item.InventoryItem._id == -1 || isDragging)
                return;
            
            _descriptionPanel.EnableDisplay(item.InventoryItem.item);
        }

        public void OnPointerExitAction() => 
            _descriptionPanel.DisableDisplay();

        private IEnumerator OnItemClick(IItemUi item)
        {
            if (item.InventoryItem._id == -1)
                yield break;
            
            CurrentItem = item;
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
            item.ToggleVisibility(true);
        }
    }
}