using System.Collections.Generic;
using FT.Shooting;
using UnityEngine;

namespace FT.Ability
{
    public abstract class AbilityState : MonoBehaviour, IAbilityState
    {
        public virtual HashSet<IHit> GatherData(IHit hit) { return new HashSet<IHit>(); }

        public virtual void SingleTarget(IHit hit) { }

        public virtual void ExecuteCall(HashSet<IHit> hits) { }
    }
}