using FT.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FT.Inventory
{
    public class InventoryItemUi : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IItem
    {
        [SerializeField] private Image _itemImage;
        [SerializeField] private Image _itemBackground;
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
            _itemBackground.sprite = item is Weapon weapon ? Resources.Load<Sprite>($"Rarity/Item{weapon.Rarity}_Sprite") 
                : Resources.Load<Sprite>("Rarity/ItemBackground_Sprite");
                
            
            Id = item.Id;
            _itemImage.sprite = item.Sprite;
            _itemImage.color = new Color(1, 1, 1, 1);
        }

        public void DeinitializeItem()
        {
            Id = -1;

            _itemImage.sprite = null;
            _itemBackground.sprite = Resources.Load<Sprite>("Rarity/ItemBackground_Sprite");
            _itemImage.color = new Color(1, 1, 1, 0);
        }

        public void ToggleVisibility(bool isVisible) => 
            _itemImage.color = new Color(1, 1, 1, isVisible ? 1 : 0);
    }
}