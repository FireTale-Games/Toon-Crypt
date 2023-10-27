using UnityEngine;

namespace FT.UI
{
    public class CharacterScreen : MonoBehaviour, IItemActionHandler<IItemUi>
    {
        [SerializeField] private DescriptionPanelUi _descriptionPanel;
        
        public IItemUi CurrentItem { get; private set; }

        public void OnPointerDownAction(IItemUi item)
        {
            CurrentItem = item;
        }

        public void OnPointerUpAction()
        {
           
        }

        public void OnPointerEnterAction(IItemUi item)
        {
            if (item.InventoryItem._id == -1)
                return;
            
            _descriptionPanel.EnableDisplay(item.InventoryItem.item);
        }

        public void OnPointerExitAction()
        {
            _descriptionPanel.DisableDisplay();
        }
    }
}