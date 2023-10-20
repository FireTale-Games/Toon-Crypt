using System.Collections.Generic;

namespace FT.Shooting
{
    public interface IHit
    {
        public void RegisterAbilityStates(List<Data.Ability> abilities);
    }
}