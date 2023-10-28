using System;
using System.Linq;
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
                    if (itemUi.InventoryItem.IsValid)
                        continue;

                    Item item = ItemDatabase.GetRandomItem();
                    itemUi.Initialize(new InventoryItem(Guid.NewGuid(),  item.Id, item is Weapon ? Enumerable.Repeat(new InventoryItem(Guid.NewGuid(),  item.Id, null), (int)item.Rarity).ToArray() : null ));
                    break;
                }
            });
        }
    }
}