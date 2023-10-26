using System.Collections.Generic;

namespace FT.Shooting
{
    public interface IHit
    {
        public void RegisterAbilityStates(HashSet<Data.Ability> abilities);
    }
}