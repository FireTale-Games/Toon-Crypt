namespace FT.Inventory
{
    public enum SlotType : byte { All, Weapon, Ability }
    
    public interface IItem
    {
        public int Id { get; }
        public SlotType SlotType { get; }
        public void InitializeItem(int id);
        public void DeinitializeItem();
        public void ToggleVisibility(bool isVisible);
    }
}