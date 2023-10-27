using UnityEngine;

namespace FT.UI
{
    public class CharacterScreen : MonoBehaviour, IItemActionHandler<IItemUi>
    {
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
            
        }

        public void OnPointerExitAction()
        {
            
        }
    }
}