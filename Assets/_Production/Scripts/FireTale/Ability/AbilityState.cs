using System.Collections.Generic;
using FT.Shooting;
using UnityEngine;

namespace FT.Ability
{
    public abstract class AbilityState : MonoBehaviour, IAbilityState
    {
        public virtual HashSet<IHit> GatherData() { return new HashSet<IHit>(); }
        public virtual void ExecuteCall(HashSet<IHit> hits) { }
    }
}