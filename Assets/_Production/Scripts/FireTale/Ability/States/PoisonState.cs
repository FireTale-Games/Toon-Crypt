using System.Collections.Generic;
using FT.Shooting;
using UnityEngine;

namespace FT.Ability.States
{
    public class PoisonState : AbilityState
    {
        public override void ExecuteCall(HashSet<IHit> hits)
        {
            Debug.Log("Hello");
        }
    }
}