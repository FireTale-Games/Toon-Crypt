using FT.Tools.Observers;
using UnityEngine;

namespace FT.TD
{
    public class CharacterState
    {
        public ObservableProperty<bool> IsShooting { get; }= new();
    }
}