using System;
using FT.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace FT.UI
{
    public class ItemUi : MonoBehaviour, IItemUi, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _itemRarityImage;
        [SerializeField] private Image _itemImage;
        [SerializeField] private ItemType _itemType = ItemType.All;
        
        public InventoryItem InventoryItem => _inventoryItem ??= new InventoryItem(new Guid(), -1, _itemType);
        private InventoryItem? _inventoryItem;

        private IItemActionHandler<IItemUi> _actionHandler;
        
        private void Awake() => 
            _actionHandler = GetComponentInParent<IItemActionHandler<IItemUi>>();

        public void OnPointerDown(PointerEventData eventData) =>
            _actionHandler?.OnPointerDownAction(this);

        public void OnPointerUp(PointerEventData eventData) =>
            _actionHandler?.OnPointerUpAction(this, GetComponentInParent<IBasePanel>());

        public void OnPointerEnter(PointerEventData eventData) =>
            _actionHandler?.OnPointerEnterAction(this);

        public void OnPointerExit(PointerEventData eventData) =>
            _actionHandler?.OnPointerExitAction();
        
        public void Initialize(int id)
        {
            Item item = ItemDatabase.Get(id);
            if (item == null)
                return;

            _inventoryItem = new InventoryItem(Guid.NewGuid(), id, _itemType);
            _itemImage.sprite = item.Sprite;
            _itemRarityImage.sprite = Resources.Load<Sprite>($"General/Rarity/Item{item.Rarity}_Sprite");
            ToggleVisibility(true);
        }

        public void DeinitializeItem()
        {
            _inventoryItem = new InventoryItem(new Guid(), -1, _itemType);
            ToggleVisibility(false);
        }

        public void ToggleVisibility(bool isVisible)
        {
            _itemRarityImage.color = new Color(1, 1, 1, isVisible ? 1 : 0);
            _itemImage.color = new Color(1, 1, 1, isVisible ? 1 : 0);
        }
    }
}