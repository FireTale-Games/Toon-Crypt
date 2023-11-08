using System.Collections.Generic;
using FT.TD;
using UnityEngine;

namespace FT.Shooting
{
    public interface IHit
    {
        public void RegisterAbilityStates(List<Data.Ability> abilities,
            CharacterStatsController characterStatsController);

        public void ApplyFlatDamage(float damage, Transform damageParticle = null);
    }
}