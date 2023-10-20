using FT.Tools.Observers;
using UnityEngine;

namespace FT.TD
{
    public class CharacterState
    {
        public ObservableProperty<Vector3> MoveDirection = new();
        public ObservableProperty<bool> IsShooting = new();
    }
}