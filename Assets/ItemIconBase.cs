using UnityEngine;
using UnityEngine.UI;

namespace FT.UI
{
    public class ItemIconBase : MonoBehaviour, IItemIcon
    {
        [SerializeField] private Image _itemRarityImage;
        [SerializeField] private Image _itemImage;
        
        [field: SerializeField] public InventoryItem InventoryItem { get; private set; }
        
        public void InitializeItemIcon(InventoryItem inventoryItem)
        {
            InventoryItem = inventoryItem;
            ToggleVisibility(InventoryItem.IsValid);
        }

        private void ToggleVisibility(bool isVisible)
        {
            _itemRarityImage.sprite = isVisible ? Resources.Load<Sprite>($"General/Rarity/Item{InventoryItem.Item.Rarity}_Sprite") : null;
            _itemRarityImage.color = new Color(1, 1, 1, isVisible ? 1 : 0);
            _itemImage.sprite = isVisible ? InventoryItem.Item.Sprite : null;
            _itemImage.color = new Color(1, 1, 1, isVisible ? 1 : 0);
        }
    }
}