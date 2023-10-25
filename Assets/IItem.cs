using UnityEngine;

namespace FT.Inventory
{
    public interface IItem
    {
        public int Id { get; }
        public Vector2 Position { get; }
        public void InitializeItem(int id);
        public void DeinitializeItem();
        public void ToggleVisibility(bool isVisible);
    }
}