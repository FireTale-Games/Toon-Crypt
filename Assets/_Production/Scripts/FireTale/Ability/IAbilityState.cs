using System.Collections.Generic;
using FT.Shooting;

namespace FT.Ability
{
    public interface IAbilityState
    {
        public HashSet<IHit> GatherData();
        public void ExecuteCall(HashSet<IHit> hits);
    }
}