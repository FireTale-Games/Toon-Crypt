using System.Collections.Generic;
using FT.TD;

namespace FT.Shooting
{
    public interface IHit
    {
        public void RegisterAbilityStates(List<Data.Ability> abilities,
            CharacterStatsController characterStatsController);
    }
}