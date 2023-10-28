using FT.Tools.Observers;

namespace FT.TD
{
    public struct SpellStruct
    {
        public readonly int _id;
        public readonly bool _isAdd;

        public SpellStruct(int id, bool isAdd)
        {
            _id = id;
            _isAdd = isAdd;
        }
    }
    
    public class CharacterState
    {
        public ObservableProperty<bool> IsShooting { get; } = new();
        public ObservableProperty<SpellStruct> AddSpell { get; } = new();
        public ObservableProperty<bool> IsInventory { get; } = new();
        public ObservableProperty<bool> IsEscape { get; } = new();
    }
}