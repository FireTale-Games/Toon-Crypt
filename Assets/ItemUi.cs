using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace FT.UI
{
    public class ItemUi : MonoBehaviour, IItemUi, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private ItemType _itemType = ItemType.All;
        
        public InventoryItem InventoryItem => _inventoryItem ??= new InventoryItem(Guid.NewGuid(), -1, _itemType);
        private InventoryItem? _inventoryItem;

        private IItemActionHandler<IItemUi> _actionHandler;
        
        private void Awake() => 
            _actionHandler = GetComponentInParent<IItemActionHandler<IItemUi>>();

        public void OnPointerDown(PointerEventData eventData) =>
            _actionHandler?.OnPointerDownAction(this);

        public void OnPointerUp(PointerEventData eventData) =>
            _actionHandler?.OnPointerUpAction();

        public void OnPointerEnter(PointerEventData eventData) =>
            _actionHandler?.OnPointerEnterAction(this);

        public void OnPointerExit(PointerEventData eventData) =>
            _actionHandler?.OnPointerExitAction();
    }
}