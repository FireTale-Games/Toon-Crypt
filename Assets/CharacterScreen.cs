using UnityEngine;

namespace FT.UI
{
    public class CharacterScreen : MonoBehaviour, IItemActionHandler<IItemUi>
    {
        private IItemUi _currentItem;
        
        public void OnPointerDownAction(IItemUi item)
        {
            _currentItem = item;
            Debug.Log(_currentItem.InventoryItem._id);
        }

        public void OnPointerUpAction()
        {
            Debug.Log(_currentItem.InventoryItem._id);
            _currentItem = null;
        }

        public void OnPointerEnterAction(IItemUi item)
        {
            Debug.Log(item.InventoryItem._id);
        }

        public void OnPointerExitAction()
        {
            
        }
    }
}