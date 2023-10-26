namespace FT.Inventory
{
    public enum SlotType : byte { All, Weapon, Ability }
    
    public interface IItem
    {
        public int Id { get; }
        public IBasePanel BasePanel { get; }
        public SlotType SlotType { get; }
        public void InitializeItem(int id = -1);
        public void ToggleVisibility(bool isVisible);
    }
}