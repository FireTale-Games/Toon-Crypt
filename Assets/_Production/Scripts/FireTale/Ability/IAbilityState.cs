using System.Collections.Generic;
using FT.Shooting;

namespace FT.Ability
{
    public interface IAbilityState
    {
        public HashSet<IHit> GatherData(IHit hit);
        public void SingleTarget(IHit hit);
        public void ExecuteCall(HashSet<IHit> hits);
    }
}