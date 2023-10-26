using FT.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FT.Inventory
{
    public class InventoryItemUi : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IItem
    {
        [SerializeField] private Image _itemImage;
        public int Id { get; private set; } = -1;
        [field: SerializeField] public SlotType SlotType { get; private set; } = SlotType.All;

        private IItemActionHandler<IItem> ActionHandler => 
            GetComponentInParent<IItemActionHandler<IItem>>();

        public void OnPointerDown(PointerEventData eventData) => 
            ActionHandler?.ItemLeftClicked(this);

        public void OnPointerUp(PointerEventData eventData) => 
            ActionHandler?.ItemStopDrag(this);
        
        public void OnPointerEnter(PointerEventData eventData) =>
            ActionHandler?.MouseEnterFrame(this);

        public void OnPointerExit(PointerEventData eventData) =>
            ActionHandler?.MouseExitFrame();
        
        public void InitializeItem(int id)
        {
            Item item = ItemDatabase.Get(id);

            Id = item.Id;
            _itemImage.sprite = item.Sprite;
            _itemImage.color = new Color(1, 1, 1, 1);
        }

        public void DeinitializeItem()
        {
            Id = -1;

            _itemImage.sprite = null;
            _itemImage.color = new Color(1, 1, 1, 0);
        }

        public void ToggleVisibility(bool isVisible) => 
            _itemImage.color = new Color(1, 1, 1, isVisible ? 1 : 0);
    }
}