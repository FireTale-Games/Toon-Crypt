using FT.Tools.Observers;
using UnityEngine;

namespace FT.TD
{
    public class CharacterState
    {
        public ObservableProperty<bool> IsShooting { get; } = new();
        public ObservableProperty<bool> IsInventory { get; } = new();
        public ObservableProperty<bool> IsEscape { get; } = new();
        public ObservableProperty<Vector3> LookDirection { get; set; } = new();
    }
}