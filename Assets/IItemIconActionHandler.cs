namespace FT.UI
{
    public interface IItemIconActionHandler<in T>
    {
        public void OnPointerDownAction(T t);
        public void OnPointerUpAction(T t, IBasePanel draggedPanel);
        public void OnPointerEnterAction(T t);
        public void OnPointerExitAction();
    }
}