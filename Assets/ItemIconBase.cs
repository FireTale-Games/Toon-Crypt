using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FT.UI
{
    public class ItemIconBase : MonoBehaviour, IItemIcon, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected ItemSlotType _itemSlotType;
        [SerializeField] private Image _itemRarityImage;
        [SerializeField] private Image _itemImage;
        
        [field: SerializeField] public InventoryItem InventoryItem { get; private set; }

        private IItemIconActionHandler<IItemIcon> _actionHandler;
        
        private void Awake() => 
            _actionHandler = GetComponentInParent<IItemIconActionHandler<IItemIcon>>();

        public void OnPointerDown(PointerEventData eventData) =>
            _actionHandler?.OnPointerDownAction(this);

        public void OnPointerUp(PointerEventData eventData) =>
            _actionHandler?.OnPointerUpAction(this);

        public void OnPointerEnter(PointerEventData eventData) =>
            _actionHandler?.OnPointerEnterAction(this);

        public void OnPointerExit(PointerEventData eventData) =>
            _actionHandler?.OnPointerExitAction();
        
        public void InitializeItemIcon(InventoryItem inventoryItem)
        {
            InventoryItem = inventoryItem;
            ToggleVisibility(InventoryItem.IsValid);
            if (!InventoryItem.IsValid)
                return;
            
            _itemRarityImage.sprite = Resources.Load<Sprite>($"General/Rarity/Item{InventoryItem.Item.Rarity}_Sprite");
            _itemImage.sprite = InventoryItem.Item.Sprite;
        }

        public void ToggleVisibility(bool isVisible)
        {
            _itemRarityImage.color = new Color(1, 1, 1, isVisible ? 1 : 0);
            _itemImage.color = new Color(1, 1, 1, isVisible ? 1 : 0);
        }
    }
}