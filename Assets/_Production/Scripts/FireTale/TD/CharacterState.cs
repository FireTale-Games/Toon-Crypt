using FT.Tools.Observers;

namespace FT.TD
{
    public struct AbilityStruct
    {
        public readonly int _id;
        public readonly bool _isAdd;

        public AbilityStruct(int id, bool isAdd)
        {
            _id = id;
            _isAdd = isAdd;
        }
    }
    
    public class CharacterState
    {
        public ObservableProperty<bool> IsShooting { get; } = new();
        public ObservableProperty<bool> IsInventory { get; } = new();
        public ObservableProperty<bool> IsEscape { get; } = new();
        public ObservableProperty<AbilityStruct> AddSpell { get; } = new();
    }
}