using FT.Data;
using UnityEngine;
using UnityEngine.UI;

namespace FT.UI
{
    public class InventoryPanel : BasePanel
    {
        [SerializeField] private Button _addItem; 
        [SerializeField] private Button _removeItem; 
        
        private void Awake()
        {
            _addItem.onClick.AddListener(() =>
            {
                foreach (IItemUi itemUi in GetComponentsInChildren<IItemUi>())
                {
                    if (itemUi.InventoryItem._id != -1)
                        continue;

                    Item item = ItemDatabase.GetRandomItem();
                    itemUi.Initialize(item.Id);
                    break;
                }
            });
            
            _removeItem.onClick.AddListener(() => GetComponentInParent<CharacterScreen>()?.CurrentItem?.DeinitializeItem());
        }
    }
}