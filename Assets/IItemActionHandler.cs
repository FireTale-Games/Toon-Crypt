namespace FT.UI
{
    public interface IItemActionHandler<in T>
    {
        public void OnPointerDownAction(T t);
        public void OnPointerUpAction(T t, IBasePanel selectedPanel);
        public void OnPointerEnterAction(T t);
        public void OnPointerExitAction();
    }
}