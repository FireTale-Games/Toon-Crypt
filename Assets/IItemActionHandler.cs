namespace FT.Inventory
{
    public interface IItemActionHandler<in T>
    {
        public void ItemLeftClicked(T t);
        public void ItemStopDrag();
        public void MouseEnterFrame(T t);
        public void MouseExitFrame();
    }
}