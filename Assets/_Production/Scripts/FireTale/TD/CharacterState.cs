using FT.Tools.Observers;

namespace FT.TD
{
    public class CharacterState
    {
        public ObservableProperty<bool> IsShooting { get; } = new();
        public ObservableProperty<bool> IsInventory { get; } = new();
        public ObservableProperty<bool> IsEscape { get; } = new();
        public ObservableProperty<(int id, bool isAdd)> AddSpell { get; } = new();
    }
}