using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FT.Inventory
{
    public class InventoryItemUI : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IItem
    {
        [SerializeField] private Image _itemImage;

        private IInventory<IItem> ActionHandler => 
            GetComponentInParent<IInventory<IItem>>();

        public void OnPointerDown(PointerEventData eventData) => 
            ActionHandler?.ItemLeftClicked(this);

        public void OnPointerUp(PointerEventData eventData) => 
            ActionHandler?.ItemStopDrag(this);

        public string GetImageName() =>
            _itemImage.sprite.name;
    }
}